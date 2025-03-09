using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum JoystickType
{
    Move, Rotate
}

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform joyStick;
    [SerializeField] private RectTransform backGround;
    [SerializeField] private PlayerController player;


    private float radius;
    private bool isTouch = false;
    private Vector3 movePosition;

    private void Start()
    {
        radius = backGround.rect.width * 0.5f;
    }

    private void Update()
    {
        if (isTouch)
        {
            if (player.isAtk)
            {
                player.transform.position += movePosition;
                player.isMove = movePosition.magnitude > 0;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)backGround.position;
        value = Vector2.ClampMagnitude(value, radius);
        joyStick.localPosition = value;

        float distance = Vector2.Distance(backGround.position, joyStick.position) / radius;
        value = value.normalized;

        movePosition = new Vector3(value.x * player.moveSpeed * distance * Time.deltaTime, 0, value.y * player.moveSpeed * distance * Time.deltaTime);
        if (value.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg, 0f);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 10f * Time.deltaTime);
            //player.transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg, 0f);

            player.animator.SetBool("Walk", player.isMove);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        joyStick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
        player.isMove = false;
        player.animator.SetBool("Walk", player.isMove);
    }
}
