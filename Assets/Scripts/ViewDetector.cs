using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDetector : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public GameObject Target { get { return target; } }
    [SerializeField] private GameObject atkTarget;
    public GameObject AtkTarget { get { return atkTarget; } }

    [SerializeField] private float radiu;
    [SerializeField] private float angle;
    [SerializeField] private float atkRadiu;
    [SerializeField] private float atkAngle;
    [SerializeField] private LayerMask layerMask;

    public void FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radiu, layerMask);
        float min = Mathf.Infinity;

        foreach(Collider collider in targets)
        {
            Vector3 findTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawRay(transform.position, findTarget * distance, Color.red);

            if (distance < min)
            {
                min = distance;
                target = collider.gameObject;
            }
        }

        if(targets.Length <= 0)
        {
            target = null;
        }
        
    }

    public void FindMinTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radiu, layerMask);
        float min = Mathf.Infinity;

        foreach (Collider collider in targets)
        {
            if (collider.GetComponent<Mercenary>().Hp < min)
            {
                min = collider.GetComponent<Mercenary>().Hp;
                target = collider.gameObject;
            }
        }

        if (targets.Length <= 0)
        {
            target = null;
        }
    }

    public void FindMinAtkTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, atkRadiu, layerMask);
        float min = Mathf.Infinity;

        foreach (Collider collider in targets)
        {
            if (collider.GetComponent<Mercenary>().Hp < min)
            {
                min = collider.GetComponent<Mercenary>().Hp;
                atkTarget = collider.gameObject;
            }    
        }

        if (targets.Length <= 0)
        {
            atkTarget = null;
        }
    }

    public void FindAttackTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, atkRadiu, layerMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 findTarget = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(atkAngle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }
            float findTargetRange = Vector3.Distance(transform.position, targets[i].transform.position);
            Debug.DrawRay(transform.position, findTarget * findTargetRange, Color.red);

            atkTarget = targets[i].gameObject;
            return;
        }
        atkTarget = null;
    }


    public void FindRangeAttack(float damage, Mercenary mercenary)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, atkRadiu, layerMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 findTarget = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(atkAngle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }
            float findTargetRange = Vector3.Distance(transform.position, targets[i].transform.position);
            Debug.DrawRay(transform.position, findTarget * findTargetRange, Color.red);

            atkTarget = targets[i].gameObject;

            targets[i].GetComponent<Mercenary>().TakeHit(damage, mercenary);
        }
        atkTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiu);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRadiu);

        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * radiu, Color.green);
        Debug.DrawRay(transform.position, rightDir * radiu, Color.green);
        Debug.DrawRay(transform.position, leftDir * radiu, Color.green);

        Vector3 atkRightDir = AngleToDir(transform.eulerAngles.y + atkAngle * 0.5f);
        Vector3 atkLeftDir = AngleToDir(transform.eulerAngles.y - atkAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * atkRadiu, Color.red);
        Debug.DrawRay(transform.position, atkRightDir * atkRadiu, Color.red);
        Debug.DrawRay(transform.position, atkLeftDir * atkRadiu, Color.red);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
