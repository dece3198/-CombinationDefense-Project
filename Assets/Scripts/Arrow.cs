using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Mercenary mercenary;
    public GameObject target;
    private Rigidbody rigid;
    [SerializeField] private float speed;
    private IEnumerator arrowCo;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(target != null)
        {
            arrowCo = ArrowCo();
            StartCoroutine(arrowCo);
        }
    }

    private void Update()
    {
        if(target != null)
        {
            transform.LookAt(target.GetComponent<Mercenary>().headPos.position);
            rigid.linearVelocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Mercenary>() != null)
        {
            if(mercenary.mecenaryType == MecenaryType.Mercenary)
            {
                if (other.GetComponent<Mercenary>().mecenaryType == MecenaryType.Monster)
                {
                    other.GetComponent<Mercenary>().TakeHit(mercenary.atk);
                    StopCoroutine(arrowCo);
                    target = null;
                    mercenary.EnterArrow(gameObject);
                }
            }
            else
            {
                if (other.GetComponent<Mercenary>().mecenaryType == MecenaryType.Mercenary)
                {
                    other.GetComponent<Mercenary>().TakeHit(mercenary.atk);
                    StopCoroutine(arrowCo);
                    target = null;
                    mercenary.EnterArrow(gameObject);
                }
            }
        }
    }

    private IEnumerator ArrowCo()
    {
        yield return new WaitForSeconds(3f);
        target = null;
        mercenary.EnterArrow(gameObject);
        gameObject.SetActive(false);
    }
}
