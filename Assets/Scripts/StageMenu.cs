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

    public void AddStage(Stage stage)
    {
        menu.SetActive(true);
        characterImage.sprite = stage.stageImage;
        stageText.text = stage.stageText;
        goldText.text = stage.money.ToString();
        compensationText.text = stage.compensation;
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

        for(int k = 0; k < stage.monsters.Length; k++)
        {
            slots[k].AddCard(stage.monsters[k]);
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
        StageManager.instance.ClickSound(1);
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card != null)
            {
                slots[i].ClearSlot();
            }
        }
    }
}
