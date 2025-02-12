using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Card card;
    public Image cardImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Image ratingImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingTextA;
    [SerializeField] private TextMeshProUGUI ratingTextB;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private Outline Outline;
    [SerializeField] private GameObject mix;

    private void Awake()
    {
        backImage = GetComponent<Image>();
    }

    public void AddCard(Card _card)
    {
        card = _card;
        cardImage.sprite = card.cardImage;
        nameText.text = card.cardName;
        ratingTextA.text = card.rating.ToString();
        ratingTextB.text = card.rating.ToString();
        ratingTextA.color = card.classColor;
        ratingTextB.color = card.classColor;
        atkText.text = card.atk.ToString();
        hpText.text = card.hp.ToString();
        defText.text = card.def.ToString();
        ratingImage.color = card.ratingColor;
        backImage.color = card.classColor;
        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        card = null;
        cardImage.sprite = null;
        Outline.effectColor = Color.green;
        Outline.enabled = false;
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(card != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                DragCard.instance.dragSlot = this;
                DragCard.instance.AddDragSlot();
                DragCard.instance.transform.position = eventData.position;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (card != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                DragCard.instance.transform.position = eventData.position;
                Outline.effectColor = Color.red;
                Outline.enabled = true;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragCard.instance.dragSlot != null)
        {
            if (transform.CompareTag("Recall"))
            {
                Generator.instance.ExitCard(DragCard.instance.dragSlot.card);
                DragCard.instance.ClearDragSlot();
            }
        }

        if (DragCard.instance.dragSlot == this)
        {
            return;
        }

        if(DragCard.instance.dragSlot != null)
        {
            if(card != null)
            {
                Change();
            }
        }
    }

    private void Change()
    {
        Card tempCard = card;

        if(tempCard == DragCard.instance.dragSlot.card)
        {
            if(tempCard.nextCard != null)
            {
                AddCard(tempCard.nextCard);
                mix.transform.position = transform.position;
                mix.GetComponent<Animator>().Play("MixAni");
                DragCard.instance.ClearDragSlot();
            }
            else
            {
                return;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Outline.effectColor = Color.green;
        DragCard.instance.SetColor(0);
        DragCard.instance.dragSlot = null;  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (card != null)
            Outline.effectColor = Color.green;
        Outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (card != null)
            Outline.effectColor = Color.green;
        Outline.enabled = false;
    }
}
