using UnityEngine;
using UnityEngine.UI;

public class DragCard : MonoBehaviour
{
    public static DragCard instance;
    public Slot dragSlot;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image charImage;
    [SerializeField] private Image ratingImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetColor(0);
    }

    public void AddDragSlot()
    {
        charImage.sprite = dragSlot.cardImage.sprite;
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
        Color colorA = cardImage.color;
        Color colorB = charImage.color;
        Color colorC = ratingImage.color;
        colorA.a = alpha;
        colorB.a = alpha;
        colorC.a = alpha;
        cardImage.color = colorA;
        charImage.color = colorB;
        ratingImage.color = colorC;
    }
}
