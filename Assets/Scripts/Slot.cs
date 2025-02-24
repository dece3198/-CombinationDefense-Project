using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Card card;
    public Image charImage;
    [SerializeField] private Image cardBackImage;
    [SerializeField] private Image typeImage;
    [SerializeField] private Image typeBackImage;
    [SerializeField] private Image ratingImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private Outline Outline;
    [SerializeField] private GameObject mix;
    [SerializeField] private float atk;
    [SerializeField] private float hp;
    [SerializeField] private float def;

    private void Awake()
    {
        Outline = GetComponent<Outline>();
        cardBackImage = GetComponent<Image>();
    }

    public void AddCard(Card _card)
    {
        card = _card;
        charImage.sprite = card.cardImage;
        typeImage.sprite = card.typeImage;
        nameText.text = card.cardName;
        ratingText.text = card.rating.ToString();
        atk = card.atk + (card.level * 0.1f);
        hp = card.hp + (card.level * 1f);
        def = card.def + (card.level * 0.1f);
        atkText.text = atk.ToString();
        hpText.text = hp.ToString();
        defText.text = def.ToString();
        ratingImage.color = card.ratingColor;
        typeBackImage.color = card.ratingColor;
        cardBackImage.color = card.ratingColor;
        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        card = null;
        charImage.sprite = null;
        Outline.effectColor = Color.green;
        Outline.enabled = false;
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (card != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
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
                if(GameManager.instance.isMix)
                {
                    SlotManager.instance.audioSource.PlayOneShot(SlotManager.instance.audioClips[0]);
                    AddCard(tempCard.nextCard);
                    mix.transform.position = transform.position;
                    mix.GetComponent<Animator>().Play("MixAni");
                    DragCard.instance.ClearDragSlot();
                }
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
        Outline.enabled = false;
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
