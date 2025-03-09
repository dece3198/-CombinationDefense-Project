using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold = 0;
    public int money = 0;
    public int crystal = 0;
    public int key = 0;
    public int ticket = 0;
    public int clearCount = 0;
    public int bossCount = 0;
    public float BGVolume = -20;
    public float SFXVolume = -20;

    public string lastCheckTimeString;

    public string tutorialState;

    public bool isTimeCompensation = true;
    public bool isMix = false;
    public bool isFirstTutorial = true;

    public List<int> stageStarCount = new List<int>();
    public List<int> bossStarCount = new List<int>();
    public List<int> inventoryCard = new List<int>();
    public List<int> playerCard = new List<int>();
    public List<int> playerSpecialCard = new List<int>();
    public List<int> upGradeCard = new List<int>();
    public List<int> cardLevel = new List<int>();
    public List<int> admobCount = new List<int>();
    public List<bool> attendance = new List<bool>();
    public List<bool> attendanceCheck = new List<bool>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public PlayerData curData = new PlayerData();
    public string path;
    public TextMeshProUGUI continueText;
    [SerializeField] private MenuButton groundButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this);

        path = Application.persistentDataPath + "/";

        if(File.Exists(path + "Guarding the Castle With Luck"))
        {
            continueText.color = Color.white;
        }
        else
        {
            continueText.color = Color.gray;
        }
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(curData);
        File.WriteAllText(path + "Guarding the Castle With Luck", data);
    }

    public void DeleteData(string name)
    {
        File.Delete(path + name);
    }

    public void LoadData()
    {
        if(File.Exists(path + "Guarding the Castle With Luck"))
        {
            string data = File.ReadAllText(path + "Guarding the Castle With Luck");
            curData = JsonUtility.FromJson<PlayerData>(data);
        }
    }

    public void ClearData()
    {
        curData = new PlayerData();
        continueText.color = Color.gray;
        curData.inventoryCard.Add(1);
        curData.playerCard.Add(1);
        curData.upGradeCard.Add(0);
        curData.upGradeCard.Add(1);
        curData.cardLevel.Add(0);
        curData.cardLevel.Add(0);

        curData.isFirstTutorial = true;
        curData.isTimeCompensation = true;

        SoundManager.instance.bgValue = -30f;
        SoundManager.instance.sfxValue = -30f;
        SoundManager.instance.ResetSlider();

        ButtonManager.instance.curButton.menu.SetActive(false);
        ButtonManager.instance.curButton.image.sprite = ButtonManager.instance.curButton.button;
        groundButton.menu.gameObject.SetActive(true);
        groundButton.image.sprite = groundButton.pressed;

        for(int i = 0; i < StageManager.instance.stages.Length; i++)
        {
            StageManager.instance.stages[i].gameObject.SetActive(false);
            StageManager.instance.stages[i].star = 0;
            StageManager.instance.stages[i].isStage = true;
            StageManager.instance.stages[i].isFirst = true;
            StageManager.instance.stages[i].clear.SetActive(false);
            if(StageManager.instance.stages[i].compensation != null)
            {
                StageManager.instance.stages[i].compensation.SetActive(true);
            }
        }
        StageManager.instance.stages[0].gameObject.SetActive(true);

        for (int i = 0; i < StageManager.instance.bossStages.Length; i++)
        {
            StageManager.instance.bossStages[i].gameObject.SetActive(false);
            StageManager.instance.bossStages[i].star = 0;
            StageManager.instance.bossStages[i].isStage = true;
            StageManager.instance.bossStages[i].isFirst = true;
            StageManager.instance.bossStages[i].clear.SetActive(false);
        }

        foreach(var slot in Inventory.instance.slots.Where(slot => slot.card != null))
        {
            if(slot.isCheck == true)
            {
                Inventory.instance.RemoveSlot(slot.card);
                slot.isCheck = false;
            }
            slot.card = null;
            slot.gameObject.SetActive(false);
        }

        foreach(var slot in UpGradeManager.instance.slots.Where(slot => slot.card != null))
        {
            if(slot.card.cardType == CardType.Mercenary)
            {
                Card curCard = slot.card;

                while(curCard != null)
                {
                    curCard.level = 0;
                    curCard = curCard.nextCard;
                }
            }
            slot.card = null;
            slot.gameObject.SetActive(false);
        }
    }
}
