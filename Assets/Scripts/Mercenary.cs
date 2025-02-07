using UnityEngine;
using UnityEngine.AI;

public enum MercenaryType
{
    Sword, Bow, Shield
}

public enum MercenaryState
{
    Idle, Wlak, Attack, Hit, Die
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
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
    }
}

public class WalkState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
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
    }
}

public class AttackState : BaseState<Mercenary>
{
    public override void Enter(Mercenary mercenary)
    {
    }

    public override void Exit(Mercenary mercenary)
    {
    }

    public override void Update(Mercenary mercenary)
    {
    }
}





public class Mercenary : MonoBehaviour
{
    public Animator animator;
    public int rating;
    public MercenaryType type;
    public MercenaryState state;
    private StateMachine<MercenaryState, Mercenary> stateMachine = new StateMachine<MercenaryState, Mercenary>();
    public ViewDetector viewDetector;
    public NavMeshAgent agent;

    private void Awake()
    {
        stateMachine.Reset(this);
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.AddState(MercenaryState.Idle, new IdleState());
        stateMachine.AddState(MercenaryState.Wlak, new WalkState());
        stateMachine.AddState(MercenaryState.Attack, new AttackState());
    }

    private void OnEnable()
    {
        ChangeState(MercenaryState.Wlak);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Attack");
        }
    }

    public void ChangeState(MercenaryState _state)
    {
        state = _state;
        stateMachine.ChangeState(state);
    }
}
