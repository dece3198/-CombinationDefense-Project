using System.Collections;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public StageSlot curStage;
    public int curCount = 0;
    public StageSlot[] stages;
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
        GameManager.instance.isTime = false;
        if(castle.Hp < 50)
        {
            curStage.star = 1;
        }
        else if(castle.Hp == castle.maxHp)
        {
            curStage.star = 3;
        }
        else
        {
            curStage.star = 2;
        }


        if(curStage.isFirst)
        {
            GameManager.instance.clearCount++;
            curStage.isFirst = false;
        }

        castle.Hp = castle.maxHp;
        goldText.text = curStage.stage.money.ToString() + "¿ø";
        StartCoroutine(ResetCo());
        curStage.nextStage.gameObject.SetActive(true);
        GameManager.instance.money += curStage.stage.money;
        curCount = 0;

        if (curStage.star == 3)
        {
            switch(curStage.stage.stageNumber)
            {
                case 14 : GameManager.instance.isMix = true; break;
            }


            if (curStage.stage.compensationCard != null)
            {
                if (curStage.isStage)
                {
                    slot.gameObject.SetActive(true);
                    slot.AddCard(curStage.stage.compensationCard);
                    Inventory.instance.AcquireCard(curStage.stage.compensationCard);
                    UpGradeManager.instance.AcquireCard(curStage.stage.compensationCard);
                    curStage.isStage = false;
                    return;
                }
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
        castle.Hp = castle.maxHp;
        castle.hpBar.value = castle.hp / castle.maxHp;
        StopCo();
        Time.timeScale = 1f;
        GameManager.instance.isTime = false;
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
