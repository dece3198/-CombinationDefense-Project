using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerController player;
    public float dist = 10f;
    public float height = 5;
    public float smoothRotate = 5f;
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }


    private void LateUpdate()
    {
        if(player.isAtk)
        {

            float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);

            Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

            tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);

            tr.LookAt(target);
        }
    }
}
