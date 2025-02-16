using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Stage stage;
    public StageSlot nextStage;
    public Image image;
    public bool isFirst = true;
    public bool isStage = false;
    public int star = 0;
    public GameObject clear;
    private RectTransform rect;
    private Vector2 tempSize;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        tempSize = rect.sizeDelta;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * 0.9f, rect.sizeDelta.y * 0.9f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect.sizeDelta = tempSize;
    }
}
