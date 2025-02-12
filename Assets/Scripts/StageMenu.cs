using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public static StageMenu instance;
    [SerializeField] private GameObject menu;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI compensationText;
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite yStar;
    [SerializeField] private Sprite gStar;

    private void Awake()
    {
        instance = this; 
    }

    public void AddStage(StageSlot stageSlot)
    {
        menu.SetActive(true);
        characterImage.sprite = stageSlot.stage.stageImage;
        stageText.text = stageSlot.stage.stageText;
        goldText.text = stageSlot.stage.gold.ToString();
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
    }

    public void XButton()
    {
        menu.SetActive(false);
    }
}
