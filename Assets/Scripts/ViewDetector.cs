using UnityEngine;

public class ViewDetector : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public GameObject Target { get { return target; } }

    [SerializeField] private float radiu;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask layerMask;

    public void FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radiu, layerMask);
        float min = Mathf.Infinity;

        foreach(Collider collider in targets)
        {
            Vector3 findTarget = (collider.transform.position - transform.position).normalized;
            if(Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(angle * 0.5f* Mathf.Deg2Rad))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawRay(transform.position, findTarget * distance, Color.red);

            if(distance < min)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiu);

        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * radiu, Color.red);
        Debug.DrawRay(transform.position, rightDir * radiu, Color.red);
        Debug.DrawRay(transform.position, leftDir * radiu, Color.red);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
