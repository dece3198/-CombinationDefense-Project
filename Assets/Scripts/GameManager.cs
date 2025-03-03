using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int count = 0;
    public int gold = 0;
    public int money = 0;
    public int crystal = 0;
    public int curStageCount = 0;
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

        InvokeRepeating("CheckTime", 0, 60f);
    }

    private void Update()
    {
        goldText.text = gold.ToString();
        moneyText.text = money.ToString();
        crystalText.text = crystal.ToString();
    }

    public void SaveData()
    {
        DataManager.instance.curData.gold = gold;
        DataManager.instance.curData.money = money;
        DataManager.instance.curData.crystal = crystal;
        DataManager.instance.curData.BGVolume = SoundManager.instance.bgValue;
        DataManager.instance.curData.SFXVolume = SoundManager.instance.sfxValue;

        DataManager.instance.curData.inventoryCard.Clear();
        DataManager.instance.curData.playerCard.Clear();
        DataManager.instance.curData.upGradeCard.Clear();
        DataManager.instance.curData.cardLevel.Clear();
        DataManager.instance.curData.stageStarCount.Clear();
        DataManager.instance.curData.bossStarCount.Clear();


        //인벤토리 카드 저장
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if(Inventory.instance.slots[i].card != null)
            {
                DataManager.instance.curData.inventoryCard.Add(PlayerCard.instance.cardDic[Inventory.instance.slots[i].card]);
            }
        }

        //현재 장착중인 카드 저장
        for(int i = 0; i < PlayerCard.instance.cardList.Count; i++)
        {
            DataManager.instance.curData.playerCard.Add(PlayerCard.instance.cardDic[PlayerCard.instance.cardList[i]]);
        }

        for(int i = 0; i < PlayerCard.instance.specialCardList.Count; i++)
        {
            DataManager.instance.curData.playerSpecialCard.Add(PlayerCard.instance.cardDic[PlayerCard.instance.specialCardList[i]]);
        }

        //업그레이드 카드 저장
        for(int i = 0; i < UpGradeManager.instance.slots.Length; i++)
        {
            if (UpGradeManager.instance.slots[i].card != null)
            {
                DataManager.instance.curData.upGradeCard.Add(PlayerCard.instance.cardDic[UpGradeManager.instance.slots[i].card]);
                DataManager.instance.curData.cardLevel.Add(UpGradeManager.instance.slots[i].card.level);
            }
        }

        //현재 클리어한 스테이지의 별개수를 저장
        for(int i = 0; i < StageManager.instance.stages.Length; i++)
        {
            if (StageManager.instance.stages[i].star > 0)
            {
                DataManager.instance.curData.stageStarCount.Add(StageManager.instance.stages[i].star);
            }
        }

        //현재 클리어한 보스스테이지의 별개수를 저장
        for(int i = 0; i <StageManager.instance.bossStages.Length;i++)
        {
            if (StageManager.instance.bossStages[i].star > 0)
            {
                DataManager.instance.curData.bossStarCount.Add(StageManager.instance.bossStages[i].star);
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
        SoundManager.instance.bgValue = DataManager.instance.curData.BGVolume;
        SoundManager.instance.sfxValue = DataManager.instance.curData.SFXVolume;
        PlayerCard.instance.cardList.Clear();
        PlayerCard.instance.specialCardList.Clear();
        SoundManager.instance.ResetSlider();

        if(DataManager.instance.curData.isFirstTutorial)
        {
            tutorial.gameObject.SetActive(false);
        }

        //튜토리얼 맵으로 깼을 때 로드
        if (DataManager.instance.curData.isFirstStageClear)
        {
            TutorialManager.instance.ChangeState(TutorialState.First);
            StageManager.instance.stages[0].gameObject.SetActive(true);
        }

        //튜토리얼 중간에 게임을 끄면 하던 곳에서 로드
        if (DataManager.instance.curData.tutorialState == "Two")
        {
            TutorialManager.instance.ChangeState(TutorialState.Two);
        }

        //클러이한 스테이지 로드
        for (int i = 0; i < DataManager.instance.curData.clearCount; i++)
        {
            StageManager.instance.stages[i].gameObject.SetActive(true);
            StageManager.instance.stages[i + 1].gameObject.SetActive(true);
            StageManager.instance.stages[i].clear.SetActive(true);
        }

        //활성화 된 보스 스테이지 개수들을 확인하고 보스스테이지 로드
        for(int i = 0; i < DataManager.instance.curData.bossCount; i++)
        {
            StageManager.instance.bossStages[i].gameObject.SetActive(true);
        }

        //저장한 카드 인벤토리에 로드
        for(int i = 0; i < DataManager.instance.curData.inventoryCard.Count; i++)
        {
            Inventory.instance.AcquireCard(PlayerCard.instance.cardNumberDic[DataManager.instance.curData.inventoryCard[i]]);
        }

        //저장한 카드 업그레이드에 로드
        for(int i = 0; i < DataManager.instance.curData.upGradeCard.Count; i++)
        {
            UpGradeManager.instance.AcquireCard(PlayerCard.instance.cardNumberDic[DataManager.instance.curData.upGradeCard[i]]);
            UpGradeManager.instance.slots[i].card.level = DataManager.instance.curData.cardLevel[i];
            UpGradeManager.instance.slots[i].LevelUp();
        }

        //장착했던 카드들 로드
        for (int i = 0; i < DataManager.instance.curData.playerCard.Count; i++)
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

        for (int i = 0; i < DataManager.instance.curData.playerSpecialCard.Count; i++)
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
        for (int i = 0; i < DataManager.instance.curData.stageStarCount.Count; i++)
        {
            StageManager.instance.stages[i].star = DataManager.instance.curData.stageStarCount[i];
            
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
                DataManager.instance.curData.isMix = true;
            }
        }


        //클리어한 보스스테이지의 별개수에 따라 로드
        for (int i = 0; i < DataManager.instance.curData.bossStarCount.Count; i++)
        {
            StageManager.instance.bossStages[i].star = DataManager.instance.curData.bossStarCount[i];

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
    }
    



    //12시마다 초기화
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
        if (!StageManager.instance.stages[0].gameObject.activeSelf)
        {
            newGameButton.SetActive(false);
            StageManager.instance.curStage = tutorial;
            DataManager.instance.SaveData();
            StartButton();
            return;
        }

        loadButton.SetActive(false);
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
        StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[0]);
    }

    public void GameReset()
    {
        isGame = false;

        for (int i = 0; i < monster.Count; i++)
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
}
