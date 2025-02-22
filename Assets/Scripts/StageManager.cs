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
    [SerializeField] private GameObject[] stars;
    [SerializeField] private Slot slot;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Mercenary castle;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(curStage != null)
        {
            if(curStage.stage.monsterCount == curCount)
            {
                Clear();
            }
        }
    }

    private void Clear()
    {
        clear.SetActive(true);
        Time.timeScale = 1f;
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

        if (curStage.stage.compensationCard != null)
        {
            if (curStage.star == 3)
            {
                if (curStage.isStage)
                {
                    slot.gameObject.SetActive(true);
                    slot.AddCard(curStage.stage.compensationCard);
                    Inventory.instance.AcquireCard(curStage.stage.compensationCard);
                    UpGradeManager.instance.AcquireCard(curStage.stage.compensationCard);
                    curStage.isStage = false;
                }
                else
                {
                    slot.gameObject.SetActive(false);
                }
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
        else
        {
            slot.gameObject.SetActive(false);
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
    }

    public void CheckButton()
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
        if(curStage.compensation != null)
        {
            curStage.compensation.gameObject.SetActive(false);
        }
        curStage = null;
        GameManager.instance.SaveData();
    }

    private IEnumerator ResetCo()
    {
        for(int i = 0; i < curStage.star; i++)
        {
            stars[i].SetActive(true);
            stars[i].GetComponent<Animator>().SetBool("Nothing", true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
