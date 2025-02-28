using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum WeaponType
{
    Sword, Bow, Shield, knife, Castle, Wizard, Shielder, Healer, Warrior, Knight, Money, Nothing, Spear
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
        mercenary.animator.Play("Idle");
        mercenary.agent.ResetPath();
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
        if(mercenary.weaponType == WeaponType.Shielder || mercenary.weaponType == WeaponType.Healer)
        {
            mercenary.viewDetector.FindMinTarget();
        }
        else
        {
            mercenary.viewDetector.FindTarget();
        }

        if (mercenary.viewDetector.Target != null)
        {
            if (mercenary.isAtkCool)
            {
                mercenary.ChangeState(MercenaryState.Wlak);
            }
        }
    }
}

public class WalkState : BaseState<Mercenary>
{


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
        
        if(mercenary.weaponType == WeaponType.Shielder || mercenary.weaponType == WeaponType.Healer)
        {
            mercenary.viewDetector.FindMinTarget();
            mercenary.viewDetector.FindMinAtkTarget();
        }
        else
        {
            mercenary.viewDetector.FindTarget();
            mercenary.viewDetector.FindAttackTarget();
        }

        
        if (mercenary.viewDetector.Target != null)
        {
            mercenary.agent.SetDestination(mercenary.viewDetector.Target.transform.position);
        }
        else
        {
            mercenary.ChangeState(MercenaryState.Idle);
        }


        if (mercenary.viewDetector.AtkTarget != null)
        {
            mercenary.StartCoroutine(AtkCo(mercenary));
        }
    }

    private IEnumerator AtkCo(Mercenary mercenary)
    {
        mercenary.isAtkCool = false;
        mercenary.ChangeState(MercenaryState.Attack);
        yield return new WaitForSeconds(mercenary.atkCool);
        mercenary.isAtkCool = true;
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
            case WeaponType.Shielder: mercenary.StartCoroutine(ShielderCo(mercenary)); break;
            case WeaponType.Healer: mercenary.StartCoroutine(HealerCo(mercenary));  break;
            case WeaponType.knife: mercenary.animator.Play("knife"); break;
            case WeaponType.Wizard: mercenary.StartCoroutine(MagicCo(mercenary)); break;
            case WeaponType.Warrior: mercenary.StartCoroutine(WarriorCo(mercenary)); break;
            case WeaponType.Knight: mercenary.StartCoroutine(KnightCo(mercenary)); break;
            case WeaponType.Spear: mercenary.animator.SetTrigger("Spear"); break;
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


    private IEnumerator ShielderCo(Mercenary mercenary)
    {
        mercenary.animator.SetTrigger("Magic");
        mercenary.viewDetector.FindMinAtkTarget();
        if (mercenary.viewDetector.AtkTarget != null)
        {
            yield return new WaitForSeconds(1f);
            mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().shield.Play();
            yield return new WaitForSeconds(0.25f);
            mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().Shield(mercenary.atk);
        }

        yield return new WaitForSeconds(3f);
        mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().shieldBar.gameObject.SetActive(false);
    }

    private IEnumerator HealerCo(Mercenary mercenary)
    {
        mercenary.animator.SetTrigger("Magic");
        mercenary.viewDetector.FindMinAtkTarget();
        if (mercenary.viewDetector.AtkTarget != null)
        {
            yield return new WaitForSeconds(1f);
            mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().heel.Play();
            yield return new WaitForSeconds(0.25f);
            mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().Heel(mercenary.atk);
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

    private IEnumerator MagicCo(Mercenary mercenary)
    {

        mercenary.animator.SetTrigger("Magic");
        yield return new WaitForSeconds(1f);
        mercenary.viewDetector.FindAttackTarget();
        if (mercenary.viewDetector.AtkTarget != null)
        {
            mercenary.skill.transform.position = mercenary.viewDetector.AtkTarget.GetComponent<Mercenary>().heel.transform.position;
            mercenary.skill.gameObject.SetActive(true);
            mercenary.skill.Play();
        }
    }

    private IEnumerator WarriorCo(Mercenary mercenary)
    {
        mercenary.animator.Play("Attack");
        mercenary.skill.Play();
        yield return new WaitForSeconds(1f);
        mercenary.viewDetector.FindRangeAttack(mercenary.atk, mercenary);
    }

    private IEnumerator KnightCo(Mercenary mercenary)
    {
        mercenary.animator.SetBool("Slash", false);
        mercenary.animator.Play("Slash");
        mercenary.skill.Play();
        yield return new WaitForSeconds(2f);
        mercenary.animator.SetBool("Slash", true);
        yield return new WaitForSeconds(0.5f);
        mercenary.viewDetector.FindRangeAttack(mercenary.atk, mercenary);
    }
}

public class DieState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
        mercenary.agent.ResetPath();
        if(mercenary.mecenaryType == MecenaryType.Monster)
        {
            GameManager.instance.gold += mercenary.card.gold;
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
        mercenary.hpBar.value = mercenary.Hp / mercenary.maxHp;
        if (mercenary.mecenaryType == MecenaryType.Mercenary)
        {
            GameManager.instance.mecrenary.Remove(mercenary.gameObject);
            Generator.instance.EnterCard(mercenary.gameObject);
        }
        else if (mercenary.mecenaryType == MecenaryType.Monster)
        {
            GameManager.instance.monster.Remove(mercenary.gameObject);
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
    public Slider hpBar;
    public Slider shieldBar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowPos;
    [SerializeField] private SkinnedMeshRenderer[] skinned;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public Transform headPos;
    private Stack<GameObject> arrowStack = new Stack<GameObject>();

    public ParticleSystem skill;
    public ParticleSystem shield;
    public ParticleSystem heel;

    public bool isAtkCool = true;


    public float hp;
    public float Hp
    {
        get { return hp; }
        set 
        {
            hp = value; 
            if(hp <= 0)
            {
                if(weaponType != WeaponType.Castle)
                {
                    ChangeState(MercenaryState.Die);
                }
            }
        }
    }
    public float maxHp;
    private float shieldHp;
    public float ShieldHp
    {
        get { return shieldHp; }
        set 
        {
            shieldHp = value; 
            if(shieldHp <= 0)
            {
                shieldBar.gameObject.SetActive(false);
            }
        }
    }
    public float maxShieldHp;
    public float atk;
    public float def;
    public float atkCool = 0;

    private Queue<Color> mercenaryColor = new Queue<Color>();

    private void Awake()
    {
        stateMachine.Reset(this);
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        stateMachine.AddState(MercenaryState.Idle, new IdleState());
        stateMachine.AddState(MercenaryState.Wlak, new WalkState());
        stateMachine.AddState(MercenaryState.Attack, new AttackState());
        stateMachine.AddState(MercenaryState.Die, new DieState());
    }
    private void OnEnable()
    {
        atk = card.atk + (card.level * 0.1f);
        def = card.def + (card.level * 0.1f);
        maxHp = card.hp + (card.level * 1f);
        Hp = maxHp;
        ChangeState(MercenaryState.Idle);
        curLayer = gameObject.layer;
        hpBar.value = Hp / maxHp;
        isAtkCool = true;
    }

    private void Start()
    {
        if (weaponType == WeaponType.Bow)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject _arrow = Instantiate(arrow, transform);
                _arrow.transform.position = arrowPos.position;
                _arrow.GetComponent<Arrow>().mercenary = this;
                arrowStack.Push(_arrow);
            }
        }

        for(int i = 0; i < skinned.Length; i++)
        {
            mercenaryColor.Enqueue(skinned[i].material.color);
        }
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
            viewDetector.AtkTarget.GetComponent<Mercenary>().TakeHit(atk, this);
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

    public void TakeHit(float damage, Mercenary mercenary)
    {
        if(shieldBar.gameObject.activeSelf)
        {
            shieldHp -= damage;
            shieldBar.value = shieldHp / maxShieldHp;
        }
        else
        {
            if (Hp > 0)
            {
                if((damage - (def * 0.5f)) <= 0)
                {
                    Hp -= 0.1f;
                }
                else
                {
                    Hp -= (damage - (def * 0.5f));
                }
                StartCoroutine(HitCo(mercenary));
                hpBar.value = Hp / maxHp;
            }
        }
    }

    public void Shield(float damage)
    {
        shieldBar.gameObject.SetActive(true);
        shieldHp = damage;
        maxShieldHp = damage;
        shieldBar.value = ShieldHp / maxShieldHp;
    }

    public void Heel(float damage)
    {
        if(Hp < maxHp)
        {
            if(Hp + damage >= maxHp)
            {
                Hp = maxHp;
                hpBar.value = Hp / maxHp;
            }
            else
            {
                Hp += damage;
                hpBar.value = Hp / maxHp;
            }
        }
    }

    public void ChangeState(MercenaryState _state)
    {
        state = _state;
        stateMachine.ChangeState(state);
    }

    private IEnumerator HitCo(Mercenary mercenary)
    {
        if(weaponType != WeaponType.Castle)
        {
            if(mercenary.weaponType == WeaponType.Wizard)
            {
                audioSource.PlayOneShot(audioClips[1]);
            }
            else
            {
                audioSource.PlayOneShot(audioClips[0]);
            }
        }

        for (int i = 0; i < skinned.Length; i++)
        {
            skinned[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < skinned.Length; i++)
        {
            skinned[i].material.color = mercenaryColor.Dequeue();
        }

        for (int i = 0; i < skinned.Length; i++)
        {
            mercenaryColor.Enqueue(skinned[i].material.color);
        }
    }
}
