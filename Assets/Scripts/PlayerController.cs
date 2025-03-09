using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hp;
    public float Hp 
    { 
        get { return hp; }
        set 
        { 
            hp = value; 
        }
    }

    public float atk;
    public float def;


    public Animator animator;
    [SerializeField] private ViewDetector viewDetector;
    public bool isMove;
    public float moveSpeed;
    public bool isAtk = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AttackButton();
        }
    }


    public void AttackButton()
    {
        if(isAtk)
        {
            animator.Play("Attack");
            StartCoroutine(AttackCo());
            viewDetector.FindAttackTarget();
            if (viewDetector.AtkTarget != null)
            {
                transform.LookAt(viewDetector.AtkTarget.transform.position);
            }
        }
    }

    public void Attack()
    {
        viewDetector.FindAttackTarget();
        if(viewDetector.AtkTarget != null)
        {
            viewDetector.AtkTarget.GetComponent<Monster>().TakeHit(atk);
            viewDetector.AtkTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);
        }
    }

    public void TakeHit(float damage)
    {
        Hp -= damage - (def * 0.5f);
        animator.Play("Hit");
    }

    private IEnumerator AttackCo()
    {
        isAtk = false;
        yield return new WaitForSeconds(1.5f);
        isAtk = true;
    }

}
