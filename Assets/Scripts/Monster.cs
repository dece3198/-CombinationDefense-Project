using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterIdle : BaseState<Monster>
{
    public override void Enter(Monster monster)
    {
        monster.animator.SetBool("Walk", false);
    }

    public override void Exit(Monster monster)
    {
    }

    public override void Update(Monster monster)
    {
        monster.viewDetector.FindTarget();
        if(monster.viewDetector.Target != null)
        {
            monster.ChangeState(MercenaryState.Wlak);
        }
    }
}

public class MonsterWalk : BaseState<Monster>
{
    public override void Enter(Monster monster)
    {
        monster.animator.SetBool("Walk", true);
    }

    public override void Exit(Monster monster)
    {
    }

    public override void Update(Monster monster)
    {
        monster.viewDetector.FindAttackTarget();

        monster.viewDetector.FindTarget();
        if (monster.viewDetector.Target != null)
        {
            Vector3 dir = monster.viewDetector.Target.transform.position - monster.transform.position;
            dir.Normalize();
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.viewDetector.Target.transform.position, 3f * Time.deltaTime);
            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 6);
        }
        else
        {
            monster.ChangeState(MercenaryState.Idle);
        }

        if (monster.viewDetector.AtkTarget != null)
        {
            if(monster.state != MercenaryState.Hit)
            {
                if (monster.isAtkCool)
                {
                    monster.StartCoroutine(AtkCo(monster));
                }
            }
        }
    }

    private IEnumerator AtkCo(Monster monster)
    {
        monster.isAtkCool = false;
        monster.ChangeState(MercenaryState.Attack);
        yield return new WaitForSeconds(1f);
        monster.isAtkCool = true;
    }
}

public class MonsterHit : BaseState<Monster>
{
    public override void Enter(Monster monster)
    {
        monster.StartCoroutine(HitCo(monster));
        monster.animator.SetTrigger("Hit");
    }

    public override void Exit(Monster monster)
    {
    }

    public override void Update(Monster monster)
    {
        if (monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            monster.ChangeState(MercenaryState.Idle);
        }
    }

    private IEnumerator HitCo(Monster monster)
    {
        for (int i = 0; i < monster.skinnedMesh.Length; i++)
        {
            monster.skinnedMesh[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < monster.skinnedMesh.Length; i++)
        {
            monster.skinnedMesh[i].material.color = Color.white;
        }
    }
}

public class MonsterAttack : BaseState<Monster>
{
    public override void Enter(Monster monster)
    {
        monster.animator.Play("Attack");
    }

    public override void Exit(Monster monster)
    {
    }

    public override void Update(Monster monster)
    {
        if(monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            monster.ChangeState(MercenaryState.Idle);
        }
    }
}

public class MonsterDie : BaseState<Monster>
{
    public override void Enter(Monster monster)
    {
        monster.gameObject.layer = 0;
        monster.animator.Play("Die");
    }

    public override void Exit(Monster monster)
    {
    }

    public override void Update(Monster monster)
    {
    }

    private IEnumerator DieCo(Monster monster)
    {
        yield return new WaitForSeconds(3f);
        monster.gameObject.layer = monster.curLayer;
        monster.Hp = monster.maxHp;
    }
}


public class Monster : MonoBehaviour
{
    [SerializeField] private float hp;
    public float Hp 
    {   get { return hp; } 
        set 
        {
            hp = value;
            hpBar.value = Hp / maxHp;
            if (hp <= 0)
            {
                ChangeState(MercenaryState.Die);
            }
        }
    }
    public float maxHp;
    private float atk;
    private float def;

    public Card card;
    public Animator animator;
    public ViewDetector viewDetector;
    public MercenaryState state;
    public StateMachine<MercenaryState, Monster> stateMachine = new StateMachine<MercenaryState, Monster>();
    [SerializeField] private Slider hpBar;
    public GameObject damageText;
    public SkinnedMeshRenderer[] skinnedMesh;
    public bool isAtkCool = true;
    public int curLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        stateMachine.Reset(this);
        stateMachine.AddState(MercenaryState.Idle, new MonsterIdle());
        stateMachine.AddState(MercenaryState.Wlak, new MonsterWalk());
        stateMachine.AddState(MercenaryState.Hit, new MonsterHit());
        stateMachine.AddState(MercenaryState.Attack, new MonsterAttack());
        stateMachine.AddState(MercenaryState.Die, new MonsterDie());
    }

    private void OnEnable()
    {
        atk = card.atk + (card.level * 1f);
        def = card.def + (card.level * 1f);
        maxHp = card.hp + (card.level * 10f);
        Hp = maxHp;
        hpBar.value = Hp / maxHp;
        curLayer = gameObject.layer;
        ChangeState(MercenaryState.Idle);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void TakeHit(float damage)
    {
        if(Hp > 0)
        {
            if ((damage - (def * 0.5f)) <= 0)
            {
                Hp -= 0.1f;
            }
            else
            {
                Hp -= (damage - (def * 0.5f));
            }
            if(state != MercenaryState.Die)
            {
                ChangeState(MercenaryState.Hit);
            }
        }
    }

    public void Attack()
    {
        viewDetector.FindAttackTarget();
        if(viewDetector.AtkTarget != null)
        {
            viewDetector.AtkTarget.GetComponent<PlayerController>().TakeHit(atk);
            viewDetector.AtkTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);
        }
    }

    public void ChangeState(MercenaryState _state)
    {
        stateMachine.ChangeState(_state);
        state = _state;
    }

}
