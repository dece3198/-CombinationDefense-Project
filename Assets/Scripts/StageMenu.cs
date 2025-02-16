using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public static StageMenu instance;
    public GameObject menu;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI compensationText;
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite yStar;
    [SerializeField] private Sprite gStar;
    [SerializeField] private Slot[] slots;

    private void Awake()
    {
        instance = this; 
    }

    public void AddStage(StageSlot stageSlot)
    {
        menu.SetActive(true);
        characterImage.sprite = stageSlot.stage.stageImage;
        stageText.text = stageSlot.stage.stageText;
        goldText.text = stageSlot.stage.money.ToString();
        compensationText.text = stageSlot.stage.compensation;
        StageManager.instance.curStage = stageSlot;
        for (int i = 0; i < stars.Length; i++)
        {
            if ((StageManager.instance.curStage.star - 1) >= i)
            {
                stars[i].sprite = yStar;
            }
            else
            {
                stars[i].sprite = gStar;
            }
        }

        for(int k = 0; k < stageSlot.stage.monsters.Length; k++)
        {
            slots[k].AddCard(stageSlot.stage.monsters[k]);
        }

        for (int j = 0; j < slots.Length; j++)
        {
            if (slots[j].card == null)
            {
                slots[j].gameObject.SetActive(false);
            }
        }
    }

    public void XButton()
    {
        menu.SetActive(false);
    }
}
