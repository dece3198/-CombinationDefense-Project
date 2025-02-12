using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum WeaponType
{
    Sword, Bow, Shield, knife, Castle
}

public enum MercenaryState
{
    Idle, Wlak, Attack, Hit, Die
}

public enum MecenaryType
{
    Mercenary, Monster
}

public abstract class BaseState<T>
{
    public abstract void Enter(T mercenary);
    public abstract void Exit(T mercenary);
    public abstract void Update(T mercenary);
}

public class IdleState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
        mercenary.animator.SetBool("Walk", false);
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {

        mercenary.viewDetector.FindTarget();
        if (mercenary.viewDetector.Target != null)
        {
            mercenary.ChangeState(MercenaryState.Wlak);
        }
    }
}

public class WalkState : BaseState<Mercenary>
{
    private bool isAtkCool = true;

    public override void Enter(Mercenary mercenary)
    {
        if (mercenary.animator != null)
            mercenary.animator.SetBool("Walk", true);
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
        mercenary.viewDetector.FindTarget();
        if (mercenary.viewDetector.Target != null)
        {
            mercenary.agent.SetDestination(mercenary.viewDetector.Target.transform.position);
        }
        else
        {
            mercenary.ChangeState(MercenaryState.Idle);
        }

        mercenary.viewDetector.FindAttackTarget();

        if(mercenary.viewDetector.AtkTarget != null)
        {
            if(isAtkCool)
            {
                mercenary.transform.LookAt(mercenary.viewDetector.AtkTarget.transform);
                mercenary.StartCoroutine(AtkCo(mercenary));
            }
        }
    }

    private IEnumerator AtkCo(Mercenary mercenary)
    {
        isAtkCool = false;
        mercenary.ChangeState(MercenaryState.Attack);
        yield return new WaitForSeconds(mercenary.atkCool);
        isAtkCool = true;
    }
}

public class AttackState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
        switch(mercenary.weaponType)
        {
            case WeaponType.Sword :mercenary.animator.Play("Attack"); break;
            case WeaponType.Bow: Bow(mercenary); break;
            case WeaponType.Shield: Shield(mercenary); break;
            case WeaponType.knife: mercenary.animator.Play("knife"); break;
        }
        mercenary.agent.ResetPath();
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
        if(mercenary.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mercenary.ChangeState(MercenaryState.Idle);
        }
    }

    private void Bow(Mercenary mercenary)
    {
        mercenary.animator.SetTrigger("BowAttack");
        mercenary.ExitArrow();
    }

    private void Shield(Mercenary mercenary)
    {
        mercenary.StartCoroutine(ShieldCo(mercenary));
    }

    private IEnumerator ShieldCo(Mercenary mercenary)
    {
        mercenary.animator.SetBool("Shield", true);
        mercenary.animator.Play("Shield");
        mercenary.def = mercenary.def * 2;
        yield return new WaitForSeconds(5);
        mercenary.animator.SetBool("Shield", false);
        mercenary.def = mercenary.def / 2;
    }
}

public class DieState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
        if(mercenary.weaponType == WeaponType.Castle)
        {
            GameManager.instance.GameReset();
            mercenary.Hp = mercenary.maxHp;
            return;
        }

        if(mercenary.mecenaryType == MecenaryType.Monster)
        {
            GameManager.instance.gold += mercenary.gold;
        }
        mercenary.gameObject.layer = 0;
        mercenary.animator.Play("Die");
        mercenary.StartCoroutine(DieCo(mercenary));
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
    }

    private IEnumerator DieCo(Mercenary mercenary)
    {
        yield return new WaitForSeconds(3f);
        mercenary.gameObject.layer = mercenary.curLayer;
        mercenary.Hp = mercenary.maxHp;
        if (mercenary.mecenaryType == MecenaryType.Mercenary)
        {
            Generator.instance.EnterCard(mercenary.gameObject);
        }
        else if (mercenary.mecenaryType == MecenaryType.Monster)
        {
            MonsterGenerator.instance.EnterMonster(mercenary.gameObject);
        }
    }
}





public class Mercenary : MonoBehaviour
{
    public Animator animator;
    public ViewDetector viewDetector;
    public NavMeshAgent agent;


    public Card card;
    public WeaponType weaponType;
    public MercenaryState state;
    public MecenaryType mecenaryType;
    public int curLayer;


    private StateMachine<MercenaryState, Mercenary> stateMachine = new StateMachine<MercenaryState, Mercenary>();
    [SerializeField] private Slider hpBar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowPos;
    public Transform headPos;
    private Stack<GameObject> arrowStack = new Stack<GameObject>();


    public float hp;
    public float Hp
    {
        get { return hp; }
        set 
        {
            hp = value; 
            if(hp <= 0)
            {
                ChangeState(MercenaryState.Die);
            }
        }
    }
    public float maxHp;
    public float atk;
    public float def;
    public int gold;
    public float atkCool = 0;


    private void Awake()
    {
        stateMachine.Reset(this);
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.AddState(MercenaryState.Idle, new IdleState());
        stateMachine.AddState(MercenaryState.Wlak, new WalkState());
        stateMachine.AddState(MercenaryState.Attack, new AttackState());
        stateMachine.AddState(MercenaryState.Die, new DieState());
        atk = card.atk;
        def = card.def;
        maxHp = card.hp;
        Hp = maxHp;
    }

    private void Start()
    {
        if(weaponType == WeaponType.Bow)
        {
            for(int i = 0; i < 5; i++)
            {
                GameObject _arrow = Instantiate(arrow, transform);
                _arrow.transform.position = arrowPos.position;
                _arrow.GetComponent<Arrow>().mercenary = this;
                arrowStack.Push(_arrow);
            }
        }
    }

    private void OnEnable()
    {
        Hp = maxHp;
        ChangeState(MercenaryState.Idle);
        curLayer = gameObject.layer;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void Attack()
    {
        viewDetector.FindAttackTarget();
        if (viewDetector.AtkTarget != null)
        {
            viewDetector.AtkTarget.GetComponent<Mercenary>().TakeHit(atk);
        }
    }

    public void ExitArrow()
    {
        GameObject _arrow = arrowStack.Pop();
        viewDetector.FindAttackTarget();
        if(viewDetector.AtkTarget != null)
        {
            _arrow.GetComponent<Arrow>().target = viewDetector.AtkTarget;
        }
        _arrow.SetActive(true);
    }

    public void EnterArrow(GameObject _arrow)
    {
        _arrow.GetComponent<Arrow>().mercenary.arrowStack.Push(_arrow);
        _arrow.transform.position = arrowPos.position;
        _arrow.SetActive(false);
    }

    public void TakeHit(float damage)
    {
        if(Hp > 0)
        {
            Hp -= (damage - (def * 0.5f));
            hpBar.value = Hp / maxHp;
        }
    }

    public void ChangeState(MercenaryState _state)
    {
        state = _state;
        stateMachine.ChangeState(state);
    }
}
