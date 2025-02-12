using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string saveName;
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

        if(File.Exists(path + curData.saveName))
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
        File.WriteAllText(path + curData.saveName, data);
    }

    public void DeleteData(string name)
    {
        File.Delete(path + name);
    }

    public void LoadData(string name)
    {
        if(File.Exists(path + name))
        {
            string data = File.ReadAllText(path + name);
            curData = JsonUtility.FromJson<PlayerData>(data);
        }
    }
}
