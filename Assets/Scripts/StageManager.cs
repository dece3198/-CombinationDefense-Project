using System.Collections;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public StageSlot curStage;
    public int curCount = 0;
    public StageSlot[] stages;
    public StageSlot[] bossStages;
    [SerializeField] private GameObject clear;
    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private Slot slot;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Mercenary castle;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public IEnumerator monsterCo;
    private bool isClear = true;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(curStage != null)
        {
            if(curStage.stage.monsterCount == curCount)
            {
                Clear();
            }

            if(castle.Hp <= 0)
            {
                fail.SetActive(true);
            }
        }
    }

    private void Clear()
    {
        audioSource.PlayOneShot(audioClips[2]);
        clear.SetActive(true);
        Time.timeScale = 1f;
        SlotManager.instance.isTime = false;

        //성의 체력에 따라 별 개수가 달라짐
        if(castle.Hp == castle.maxHp)
        {
            curStage.star = 3;
        }
        else if(castle.Hp >= 25 || castle.hp < castle.maxHp)
        {
            curStage.star = 2;
        }
        else if(castle.Hp < 25)
        {
            curStage.star = 1;
        }

        //처음으로 스테이지를 클리어했을 때 데이터 저장
        if(curStage.isFirst)
        {

            switch(curStage.stage.stageType)
            {
                case StageType.Normal: DataManager.instance.curData.clearCount++; break;
                case StageType.Boss: DataManager.instance.curData.bossCount++; break;
                case StageType.Tutorial: break;
            }
            curStage.isFirst = false;
        }

        castle.Hp = castle.maxHp;
        if(curStage.stage.crystal > 0)
        {
            goldText.text = curStage.stage.crystal.ToString() + "크리스탈";
        }
        else
        {
            goldText.text = curStage.stage.money.ToString() + "원";
        }

        //별 성공 애니메이션 불러오는 코루틴
        StartCoroutine(ResetCo());
        curStage.nextStage.gameObject.SetActive(true);
        if(curStage.bossStage != null)
        {
            curStage.bossStage.gameObject.SetActive(true);
        }
        GameManager.instance.money += curStage.stage.money;
        curCount = 0;

        if (curStage.star == 3)
        {
            switch(curStage.stage.stageNumber)
            {
                case 11 : 
                    TutorialManager.instance.ChangeState(TutorialState.Two);
                    DataManager.instance.curData.isFirstStageClear = false;
                    break;
                case 13 : GameManager.instance.isMix = true; break;
            }

            if (curStage.isStage)
            {
                slot.gameObject.SetActive(false);
                if (curStage.stage.compensationCard != null)
                {
                    slot.gameObject.SetActive(true);
                    slot.AddCard(curStage.stage.compensationCard);
                    Inventory.instance.AcquireCard(curStage.stage.compensationCard);
                    UpGradeManager.instance.AcquireCard(curStage.stage.compensationCard);
                }
                GameManager.instance.crystal += curStage.stage.crystal;
                curStage.isStage = false;
                return;
            }
        }

        slot.gameObject.SetActive(false);
    }

    public void CheckButton()
    {
        if(isClear)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].GetComponent<Animator>().SetBool("Nothing", false);
                stars[i].SetActive(false);
            }

            GameManager.instance.ClearButton();
            GameManager.instance.GameReset();
            SlotManager.instance.isTime = true;
            SlotManager.instance.SpeedUp();
            if(curStage.clear != null)
            {
                curStage.clear.SetActive(true);
            }
            clear.SetActive(false);
            if (curStage.compensation != null)
            {
                curStage.compensation.gameObject.SetActive(false);
            }
            curStage = null;
            GameManager.instance.SaveData();
        }
    }

    public void FailCheckButton()
    {
        GameManager.instance.GameReset();
        GameManager.instance.ClearButton();
        SlotManager.instance.isTime = true;
        SlotManager.instance.SpeedUp();
        castle.Hp = castle.maxHp;
        castle.hpBar.value = castle.hp / castle.maxHp;
        StopCo();
        fail.SetActive(false);
        curCount = 0;
        curStage = null;
    }

    public void ClickSound(int number)
    {
        audioSource.PlayOneShot(audioClips[number]);
    }

    public void StartCo()
    {
        monsterCo = MonsterGenerator.instance.MonsterCo();
        StartCoroutine(monsterCo);
    }

    public void StopCo()
    {
        StopCoroutine(monsterCo);
    }


    private IEnumerator ResetCo()
    {
        isClear = false;
        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < curStage.star; i++)
        {
            audioSource.PlayOneShot(audioClips[3]);
            stars[i].SetActive(true);
            stars[i].GetComponent<Animator>().SetBool("Nothing", true);
            yield return new WaitForSeconds(0.5f);
        }
        isClear = true;
    }
}
