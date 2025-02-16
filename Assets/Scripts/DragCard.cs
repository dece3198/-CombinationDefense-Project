using UnityEngine;
using UnityEngine.UI;

public class DragCard : MonoBehaviour
{
    public static DragCard instance;
    public Slot dragSlot;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image charImage;
    [SerializeField] private Image ratingImage;
    [SerializeField] private Image[] childs;

    private void Awake()
    {
        instance = this;
        cardImage = GetComponent<Image>();
    }

    private void Start()
    {
        childs = gameObject.GetComponentsInChildren<Image>();
        SetColor(0);
    }

    public void AddDragSlot()
    {
        charImage.sprite = dragSlot.charImage.sprite;
        ratingImage.color = dragSlot.card.ratingColor;
        SetColor(1);
    }

    public void ClearDragSlot()
    {
        if(dragSlot != null)
        {
            dragSlot.ClearSlot();
        }
        dragSlot = null;
        SetColor(0);
    }

    public void SetColor(float alpha)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            Color color = childs[i].color;
            color.a = alpha;
            childs[i].color = color;
        }
        Color colorA = cardImage.color;
        colorA.a = alpha;
        cardImage.color = colorA;

    }
}
