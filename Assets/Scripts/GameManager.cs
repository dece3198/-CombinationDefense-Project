using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int count = 0;
    public int gold = 0;
    public int money = 0;
    public int crystal = 0;
    public int curStageCount = 0;
    public int clearCount = 0;
    public List<GameObject> monster = new List<GameObject>();
    public List<GameObject> mecrenary = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private GameObject mainMenu;
    public bool isTime = false;
    public bool isMix = false;
    public bool isGame = false;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InvokeRepeating("CheckTime", 0, 60f);
    }

    private void Update()
    {
        goldText.text = gold.ToString();
        moneyText.text = money.ToString();
    }

    public void SaveData()
    {
        DataManager.instance.curData.gold = gold;
        DataManager.instance.curData.money = money;
        DataManager.instance.curData.crystal = crystal;
        DataManager.instance.curData.clearCount = clearCount;

        DataManager.instance.curData.inventoryCard.Clear();
        DataManager.instance.curData.playerCard.Clear();
        DataManager.instance.curData.upGradeCard.Clear();
        DataManager.instance.curData.cardLevel.Clear();
        DataManager.instance.curData.stageStarCount.Clear();

        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if(Inventory.instance.slots[i].card != null)
            {
                DataManager.instance.curData.inventoryCard.Add(PlayerCard.instance.cardDic[Inventory.instance.slots[i].card]);
            }
        }

        for(int i = 0; i < PlayerCard.instance.cardList.Count; i++)
        {
            DataManager.instance.curData.playerCard.Add(PlayerCard.instance.cardDic[PlayerCard.instance.cardList[i]]);
        }

        for(int i = 0; i < UpGradeManager.instance.slots.Length; i++)
        {
            if (UpGradeManager.instance.slots[i].card != null)
            {
                DataManager.instance.curData.upGradeCard.Add(PlayerCard.instance.cardDic[UpGradeManager.instance.slots[i].card]);
                DataManager.instance.curData.cardLevel.Add(UpGradeManager.instance.slots[i].card.level);
            }
        }

        for(int i = 0; i < StageManager.instance.stages.Length; i++)
        {
            if (StageManager.instance.stages[i].star > 0)
            {
                DataManager.instance.curData.stageStarCount.Add(StageManager.instance.stages[i].star);
            }
        }

        DataManager.instance.SaveData();
    }

    private void LoadData()
    {
        DataManager.instance.LoadData();
        gold = DataManager.instance.curData.gold;
        money = DataManager.instance.curData.money;
        crystal = DataManager.instance.curData.crystal;
        for(int i = 0; i < DataManager.instance.curData.clearCount; i++)
        {
            StageManager.instance.stages[i].gameObject.SetActive(true);
            StageManager.instance.stages[i + 1].gameObject.SetActive(true);
            StageManager.instance.stages[i].clear.SetActive(true);
        }

        for(int i = 0; i < DataManager.instance.curData.inventoryCard.Count; i++)
        {
            Inventory.instance.AcquireCard(PlayerCard.instance.cardNumberDic[DataManager.instance.curData.inventoryCard[i]]);
        }

        for(int i = 0; i < DataManager.instance.curData.upGradeCard.Count; i++)
        {
            UpGradeManager.instance.AcquireCard(PlayerCard.instance.cardNumberDic[DataManager.instance.curData.upGradeCard[i]]);
            UpGradeManager.instance.slots[i].card.level = DataManager.instance.curData.cardLevel[i];
            UpGradeManager.instance.slots[i].LevelUp();
        }
        for(int i = 0; i < DataManager.instance.curData.playerCard.Count; i++)
        {
            PlayerCard.instance.cardList.Add(PlayerCard.instance.cardNumberDic[DataManager.instance.curData.playerCard[i]]);
            for(int j = 0; j < Inventory.instance.slots.Length; j++)
            {
                if (Inventory.instance.slots[j].card == PlayerCard.instance.cardNumberDic[DataManager.instance.curData.playerCard[i]])
                {
                    Inventory.instance.AddSlot(Inventory.instance.slots[j].card);
                    Inventory.instance.slots[j].LevelUp();
                    Inventory.instance.slots[j].checkImage.SetActive(true);
                    Inventory.instance.slots[j].isCheck = true;
                }
            }
        }


        for (int i = 0; i < DataManager.instance.curData.stageStarCount.Count; i++)
        {
            StageManager.instance.stages[i].star = DataManager.instance.curData.stageStarCount[i];
            if (DataManager.instance.curData.stageStarCount[i] >= 3)
            {
                StageManager.instance.stages[i].isFirst = false;
                StageManager.instance.stages[i].isStage = false;
            }
            else if (DataManager.instance.curData.stageStarCount[i] >= 1)
            {
                StageManager.instance.stages[i].isFirst = false;
            }
        }
    }
    
    private void CheckTime()
    {
        DateTime curTime = DateTime.Now;

        if(DataManager.instance.curData.isTimeCompensation)
        {
            DataManager.instance.curData.isTimeCompensation = false;
        }
        else
        {
            if (curTime.Day == curTime.AddDays(+1).Day)
            {
                DataManager.instance.curData.isTimeCompensation = true;
                CheckTime();
            }
        }
    }

    public void StartButton()
    {
        isGame = true;
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(1));
        MapManager.instance.map.SetActive(false);
        MapManager.instance.animator.SetBool("Close", true);
        MapManager.instance.animator.Play("Close");
        StageMenu.instance.menu.SetActive(false);
        gold = StageManager.instance.curStage.stage.startGold;
        audioSource.PlayOneShot(audioClips[0]);
        audioSource.loop = true;
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void NewGameButton()
    {
        DataManager.instance.SaveData();
        LoadData();
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void ClearButton()
    {
        audioSource.Stop();
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
    }

    public void LoadButton()
    {
        if (!File.Exists(DataManager.instance.path + "GuardTheCastle"))
        {
            return;
        }
        LoadData();
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void GameReset()
    {
        isGame = false;
        for(int i = 0; i < monster.Count; i++)
        {
            MonsterGenerator.instance.EnterMonster(monster[i]);
        }

        for(int j = 0; j < mecrenary.Count; j++)
        {
            Generator.instance.EnterCard(mecrenary[j]);
        }

        for(int k = 0; k < SlotManager.instance.slots.Length; k++)
        {
            if(SlotManager.instance.slots[k].card != null)
            {
                SlotManager.instance.slots[k].ClearSlot();
            }
        }
    }

    public void SpeedUp()
    {
        isTime = !isTime;
        if (isTime)
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }

    }


    private IEnumerator FadeCo(int number)
    {
        while (Fade.instance.isFade)
        {
            yield return null;
        }
        
        if(number == 0)
        {
            mainMenu.SetActive(false);
            MapManager.instance.mapParent.SetActive(true);
            MapManager.instance.animator.Play("Open");
            yield return new WaitForSeconds(0.83f);
            MapManager.instance.map.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            MapManager.instance.map.GetComponent<AudioSource>().Play();
        }
        else
        {
            MonsterGenerator.instance.StartGame();
            MapManager.instance.animator.SetBool("Close", true);
            MapManager.instance.mapParent.SetActive(false);
        }
    }
}
