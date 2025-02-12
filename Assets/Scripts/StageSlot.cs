using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public Stage stage;
    public StageSlot nextStage;
    public Image image;
    public bool isStage;
    public int star = 0;
    public GameObject clear;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * 2, rect.sizeDelta.y * 2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x/2, rect.sizeDelta.y/2);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(stage != null)
            {
                StageMenu.instance.AddStage(this);
            }
        }
    }
}
