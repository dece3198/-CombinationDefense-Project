using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold = 0;
    public int money = 0;
    public int crystal = 0;
    public int clearCount = 0;
    public int bossCount = 0;
    public float BGVolume = -20;
    public float SFXVolume = -20;

    public string tutorialState;

    public bool isTimeCompensation = true;
    public bool isFirstStage = false;
    public bool isFirstStageClear = false;
    public bool isMix = false;
    public bool isFirstTutorial = false;

    public List<int> stageStarCount = new List<int>();
    public List<int> bossStarCount = new List<int>();
    public List<int> inventoryCard = new List<int>();
    public List<int> playerCard = new List<int>();
    public List<int> upGradeCard = new List<int>();
    public List<int> cardLevel = new List<int>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public PlayerData curData = new PlayerData();
    public string path;
    public TextMeshProUGUI continueText;

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

        if(File.Exists(path + "GuardTheCastle"))
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
        File.WriteAllText(path + "GuardTheCastle", data);
    }

    public void DeleteData(string name)
    {
        File.Delete(path + name);
    }

    public void LoadData()
    {
        if(File.Exists(path + "GuardTheCastle"))
        {
            string data = File.ReadAllText(path + "GuardTheCastle");
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

        for(int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].card != null)
            {
                Inventory.instance.slots[i].card = null;
                Inventory.instance.slots[i].gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < UpGradeManager.instance.slots.Length; i++)
        {
            if (UpGradeManager.instance.slots[i].card != null)
            {
                UpGradeManager.instance.slots[i].card = null;
                UpGradeManager.instance.slots[i].gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < Inventory.instance.playerSlot.Length; i++)
        {
            if(Inventory.instance.playerSlot[i].card != null)
            {
                Inventory.instance.playerSlot[i].card = null;
            }
        }
    }
}
