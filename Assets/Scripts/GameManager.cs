using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private int prevGold, prevMoney, prevCrystal;

    public List<GameObject> monster = new List<GameObject>();
    public List<GameObject> mecrenary = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private GameObject mainMenu;
    public bool isGame = false;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private GameObject basicButton;
    [SerializeField] private GameObject specialButton;
    [SerializeField] private AdmobManager[] admobs;
    public StageSlot tutorial;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (File.Exists(DataManager.instance.path + "Guarding the Castle With Luck"))
        {
            newGameButton.SetActive(false);
        }
    }

    private void Update()
    {
        if(gold != prevGold)
        {
            goldText.text = gold.ToString();
            prevGold = gold;
        }

        if(money != prevMoney)
        {
            moneyText.text = money.ToString();
            prevMoney = money;
        }

        if(crystal != prevCrystal)
        {
            crystalText.text = crystal.ToString();
            prevCrystal = crystal;
        }
    }

    public void SaveData()
    {
        var data = DataManager.instance.curData;
        data.gold = gold;
        data.money = money;
        data.crystal = crystal;
        data.BGVolume = SoundManager.instance.bgValue;
        data.SFXVolume = SoundManager.instance.sfxValue;
        data.lastCheckTIme = DateTime.Now;

        //인벤토리 카드 저장
        data.inventoryCard.Clear();
        foreach(var slot in Inventory.instance.slots)
        {
            if(slot.card != null && PlayerCard.instance.cardDic.TryGetValue(slot.card, out int cardID))
            {
                data.inventoryCard.Add(cardID);
            }
        }

        //현재 장착중인 카드 저장
        data.playerCard.Clear();
        foreach(var card in PlayerCard.instance.cardList)
        {
            if(PlayerCard.instance.cardDic.TryGetValue(card, out int cardID))
            {
                data.playerCard.Add(cardID);
            }
        }

        data.playerSpecialCard.Clear();
        foreach(var card in PlayerCard.instance.specialCardList)
        {
            if(PlayerCard.instance.cardDic.TryGetValue(card, out int cardID))
            {
                data.playerSpecialCard.Add(cardID);
            }
        }

        //업그레이드 카드 저장
        data.upGradeCard.Clear();
        data.cardLevel.Clear();
        foreach(var slot in UpGradeManager.instance.slots)
        {
            if(slot.card != null && PlayerCard.instance.cardDic.TryGetValue(slot.card, out int cardID))
            {
                data.upGradeCard.Add(cardID);
                data.cardLevel.Add(slot.card.level);
            }
        }

        //현재 클리어한 스테이지의 별개수를 저장
        data.stageStarCount.Clear();
        foreach(var stage in StageManager.instance.stages)
        {
            if(stage.star > 0)
            {
                data.stageStarCount.Add(stage.star);
            }
        }

        //현재 클리어한 보스스테이지의 별개수를 저장
        data.bossStarCount.Clear();
        foreach(var stage in StageManager.instance.bossStages)
        {
            if(stage.star > 0)
            {
                data.bossStarCount.Add(stage.star);
            }
        }

        //광고 개수 저장
        data.admobCount.Clear();
        foreach(var ad in admobs)
        {
            data.admobCount.Add(ad.count);
        }

        DataManager.instance.SaveData();
    }

    private void LoadData()
    {
        DataManager.instance.LoadData();
        var data = DataManager.instance.curData;
        gold = data.gold;
        money = data.money;
        crystal = data.crystal;
        SoundManager.instance.bgValue = data.BGVolume;
        SoundManager.instance.sfxValue = data.SFXVolume;
        PlayerCard.instance.cardList.Clear();
        PlayerCard.instance.specialCardList.Clear();
        SoundManager.instance.ResetSlider();


        //클러이한 스테이지 로드
        for (int i = 0; i < data.clearCount; i++)
        {
            StageManager.instance.stages[i].gameObject.SetActive(true);
            StageManager.instance.stages[i].clear.SetActive(true);
        }
        StageManager.instance.stages[data.clearCount].gameObject.SetActive(true);

        //클리어한 보스스테이지 로드(보스 스테이지는 3성 달성시 클리어)
        for (int i = 0; i < data.bossCount; i++)
        {
            StageManager.instance.bossStages[i].gameObject.SetActive(true);
        }

        //저장한 카드 인벤토리에 로드
        for(int i = 0; i < data.inventoryCard.Count; i++)
        {
            Inventory.instance.AcquireCard(PlayerCard.instance.cardNumberDic[data.inventoryCard[i]]);
        }

        //저장한 카드 업그레이드에 로드
        for(int i = 0; i < data.upGradeCard.Count; i++)
        {
            UpGradeManager.instance.AcquireCard(PlayerCard.instance.cardNumberDic[data.upGradeCard[i]]);
            UpGradeManager.instance.slots[i].card.level = data.cardLevel[i];
            UpGradeManager.instance.slots[i].LevelUp();
        }

        //장착했던 카드들 로드
        for (int i = 0; i < data.playerCard.Count; i++)
        {
            var emptySlot = Inventory.instance.slots.FirstOrDefault(slot => slot.card == PlayerCard.instance.cardNumberDic[DataManager.instance.curData.playerCard[i]]);
            if (emptySlot != null)
            {
                Inventory.instance.AddSlot(emptySlot.card);
                emptySlot.LevelUp();
                emptySlot.checkImage.SetActive(true);
                emptySlot.isCheck = true;
            }
        }

        for (int i = 0; i < data.playerSpecialCard.Count; i++)
        {
            var emptySlot = Inventory.instance.slots.FirstOrDefault(slot => slot.card == PlayerCard.instance.cardNumberDic[DataManager.instance.curData.playerSpecialCard[i]]);
            if(emptySlot != null)
            {
                Inventory.instance.AddSlot(emptySlot.card);
                emptySlot.LevelUp();
                emptySlot.checkImage.SetActive(true);
                emptySlot.isCheck = true;
            }
        }

        //클리어한 스테이지의 별개수 로드
        for (int i = 0; i < data.stageStarCount.Count; i++)
        {
            StageManager.instance.stages[i].star = data.stageStarCount[i];
            
            switch (StageManager.instance.stages[i].star)
            {
                case >= 3:
                    StageManager.instance.stages[i].isFirst = false;
                    StageManager.instance.stages[i].isStage = false;
                    break;
                case >= 1:
                    StageManager.instance.stages[i].isFirst = false;
                    break;
            }


            if (StageManager.instance.stages[3].star == 3)
            {
                data.isMix = true;
            }
        }


        //클리어한 보스스테이지의 별개수에 따라 로드
        for (int i = 0; i < data.bossStarCount.Count; i++)
        {
            StageManager.instance.bossStages[i].star = data.bossStarCount[i];

            if (StageManager.instance.bossStages[i].star >= 3)
            {
                StageManager.instance.bossStages[i].isFirst = false;
                StageManager.instance.bossStages[i].isStage = false;
                StageManager.instance.bossStages[i].clear.SetActive(true);
            }
        }

        //상점 lock버튼 로드
        for(int i = 0; i < StageManager.instance.stages.Length; i++)
        {
            if (!StageManager.instance.stages[i].isFirst)
            {
                switch (StageManager.instance.stages[i].stage.stageNumber)
                {
                    case 14: StageManager.instance.stages[i].compensation.SetActive(false); break;
                    case 15: StageManager.instance.stages[i].compensation.SetActive(false); break;
                    case 16: StageManager.instance.stages[i].compensation.SetActive(false); break;
                }
            }
        }

        //광고 개수 로드
        if(data.admobCount.Count != 0)
        {
            for (int i = 0; i < admobs.Length; i++)
            {
                admobs[i].ResetText(data.admobCount[i]);
            }
        }
    }

    //스테이지 시작시 버튼
    public void StartButton()
    {
        if (PlayerCard.instance.specialCardList.Count == 0)
        {
            basicButton.SetActive(true);
            specialButton.SetActive(false);
        }
        else
        {
            basicButton.SetActive(false);
            specialButton.SetActive(true);
        }

        isGame = true;
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(1));
        MapManager.instance.map.SetActive(false);
        MapManager.instance.animator.Play("Close");
        StageMenu.instance.XButton();
        gold = StageManager.instance.curStage.stage.startGold;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[0]);
    }

    //게임시작 버튼
    public void NewGameButton()
    {
        newGameButton.SetActive(false);
        StageManager.instance.curStage = tutorial;
        DataManager.instance.SaveData();
        LoadData();
        StartCoroutine(CheckTimeCo());
        StartButton();
    }

    
    public void ClearButton()
    {
        audioSource.Stop();
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
    }

    public void LoadButton()
    {
        if (!File.Exists(DataManager.instance.path + "Guarding the Castle With Luck"))
        {
            return;
        }
        LoadData();
        if(DataManager.instance.curData.isFirstTutorial)
        {
            newGameButton.SetActive(false);
            StageManager.instance.curStage = tutorial;
            DataManager.instance.SaveData();
            StartButton();
            return;
        }
        else
        {
            switch (DataManager.instance.curData.tutorialState)
            {
                case "First" : TutorialManager.instance.ChangeState(TutorialState.First); break;
                case "Two" : TutorialManager.instance.ChangeState(TutorialState.Two);break;
            }
        }
        tutorial.gameObject.SetActive(false);
        loadButton.SetActive(false);
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
        StartCoroutine(CheckTimeCo());
        StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[0]);
    }

    public void GameReset()
    {
        isGame = false;

        monster.RemoveAll(m => { MonsterGenerator.instance.EnterMonster(m); return true;});
        mecrenary.RemoveAll(m => { Generator.instance.EnterCard(m); return true; });

        foreach(var slot in SlotManager.instance.slots.Where(slot => slot.card != null))
        {
            slot.ClearSlot();
        }

        monster.Clear();
        mecrenary.Clear();
    }

    public void DeleteData()
    {
        SoundManager.instance.SoundSetting();
        SoundManager.instance.DeleteButton();
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(2));
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
            audioSource.PlayOneShot(audioClips[1]);
        }
        else if(number == 1)
        {
            if(StageManager.instance.curStage.stage.stageType == StageType.Tutorial)
            {
                mainMenu.SetActive(false);
                TutorialManager.instance.ChangeState(TutorialState.First);
            }
            StageManager.instance.StartCo();
            MapManager.instance.mapParent.SetActive(false);
        }
        else
        {
            MapManager.instance.animator.Play("Close");
            newGameButton.SetActive(true);
            loadButton.SetActive(true);
            mainMenu.SetActive(true);
            DataManager.instance.curData.isMix = false;
            MapManager.instance.map.SetActive(false);
            audioSource.Stop();
            GameReset();
            File.Delete(DataManager.instance.path + "Guarding the Castle With Luck");
            DataManager.instance.ClearData();
        }
    }

    //InvokeRepeating으로 1분마다 확인중으로 12시가 넘어가면 보상후 초기화
    private void CheckTime()
    {
        if(!DataManager.instance.curData.isTimeCompensation)
        {
            for(int i = 0; i < admobs.Length; i++)
            {
                admobs[i].ResetText(5);
                DataManager.instance.curData.isTimeCompensation = true;
                SaveData();
            }
        }
    }

    private IEnumerator CheckTimeCo()
    {
        while(true)
        {
            DateTime now = DateTime.Now;
            DateTime lastCheckTime = DataManager.instance.curData.lastCheckTIme;
            DateTime todayReseTIme = now.Date;
            DateTime nextDay = now.AddDays(1);

            if(lastCheckTime < todayReseTIme)
            {
                DataManager.instance.curData.isTimeCompensation = false;
                SaveData();
            }

            if(!DataManager.instance.curData.isTimeCompensation)
            {
                CheckTime();
            }

            TimeSpan timeUntilReset = nextDay - now;
            yield return new WaitForSeconds((float)timeUntilReset.TotalSeconds);

            CheckTime();
        }
    }
}
