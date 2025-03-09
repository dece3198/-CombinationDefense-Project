using System;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceManager : MonoBehaviour
{
    public static AttendanceManager instance;
    [SerializeField] private GameObject calendar;
    public GameObject check;
    public AttendanceSlot[] slots;
    private bool isClick = false;
    [SerializeField] private Sprite money;
    [SerializeField] private Sprite crystal;
    [SerializeField] private Sprite key;
    [SerializeField] private Sprite ticket;
    public Dictionary<AttendanceType, Sprite> attDic = new Dictionary<AttendanceType, Sprite>();

    private void Awake()
    {
        instance = this;
        attDic.Add(AttendanceType.Money, money);
        attDic.Add(AttendanceType.Crystal, crystal);
        attDic.Add(AttendanceType.Key, key);
        attDic.Add(AttendanceType.Ticket, ticket);
    }

    public void CalendarClick()
    {
        isClick = !isClick;

        if(isClick)
        {
            StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[0]);
            calendar.SetActive(true);
        }
        else
        {
            StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[1]);
            calendar.SetActive(false);
        }
    }
}
