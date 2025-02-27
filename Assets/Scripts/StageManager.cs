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


        if(curStage.isFirst)
        {
            if(curStage.stage.stageType == StageType.Normal)
            {
                DataManager.instance.curData.clearCount++;
            }
            else
            {
                DataManager.instance.curData.bossCount++;
            }
            curStage.isFirst = false;
        }

        castle.Hp = castle.maxHp;
        if(curStage.stage.money == 0)
        {
            goldText.text = curStage.stage.crystal.ToString() + "Å©¸®½ºÅ»";
        }
        else
        {
            goldText.text = curStage.stage.money.ToString() + "¿ø";
        }
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
            curStage.clear.SetActive(true);
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
            yield return new WaitForSeconds(1f);
        }
        isClear = true;
    }
}
