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
    public bool isTimeCompensation = true;
    public bool isFirstStage = true;
    public bool isMix = false;

    public List<int> stageStarCount;
    public List<int> inventoryCard;
    public List<int> playerCard;
    public List<int> upGradeCard;
    public List<int> cardLevel;
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
}
