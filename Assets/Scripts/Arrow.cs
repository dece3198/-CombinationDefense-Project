using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Mercenary mercenary;
    public GameObject target;
    private Rigidbody rigid;
    [SerializeField] private float speed;
    private IEnumerator arrowCo;
    private bool isArrow = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(target != null)
        {
            rigid.isKinematic = true;
            transform.LookAt(target.GetComponent<Mercenary>().headPos.position);
            arrowCo = ArrowCo();
            StartCoroutine(arrowCo);
        }
    }

    private void Update()
    {
        if(target != null)
        {
            if(isArrow)
            {
                transform.LookAt(target.GetComponent<Mercenary>().headPos.position);
                rigid.linearVelocity = transform.forward * speed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Mercenary>() != null)
        {
            if(other.transform.tag != mercenary.transform.tag)
            {
                isArrow = false;
                other.GetComponent<Mercenary>().TakeHit(mercenary.atk, mercenary);
                StopCoroutine(arrowCo);
                target = null;
                mercenary.EnterArrow(gameObject);
            }
        }
    }

    private IEnumerator ArrowCo()
    {
        yield return new WaitForSeconds(0.1f);
        isArrow = true;
        rigid.isKinematic = false;
        yield return new WaitForSeconds(3f);
        target = null;
        mercenary.EnterArrow(gameObject);
        gameObject.SetActive(false);
    }
}
