using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AttendanceType
{
    Money, Crystal, Key, Ticket
}

public class AttendanceSlot : MonoBehaviour
{
    public Outline outline;
    public GameObject checkImage;
    public AttendanceType type;
    [SerializeField] private Image attendanceImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private int count = 0;
    public bool isAttendance = false;
    public bool isCheck = false;

    private void Start()
    {
        attendanceImage.sprite = AttendanceManager.instance.attDic[type];
        countText.text = count.ToString();
    }

    public void AttendanceSlotClick()
    {
        if(isAttendance)
        {
            switch(type)
            {
                case AttendanceType.Money: GameManager.instance.Money += count; break;
                case AttendanceType.Crystal: GameManager.instance.Crystal += count; break;
                case AttendanceType.Key: GameManager.instance.Key += count; break;
                case AttendanceType.Ticket: GameManager.instance.Ticket += count; break;
            }
            checkImage.SetActive(true);
            isCheck = true;
            isAttendance = false;
            AttendanceManager.instance.check.SetActive(false);
            GameManager.instance.SaveData();
        }
    }
}
