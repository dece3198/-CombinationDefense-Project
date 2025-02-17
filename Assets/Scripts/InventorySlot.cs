using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
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
    public GameObject checkImage;
    public bool isCheck = false;

    public void AddCard(Card _card)
    {
        card = _card;
        charImage.sprite = card.cardImage;
        typeImage.sprite = card.typeImage;
        nameText.text = card.cardName;
        ratingText.text = card.rating.ToString();
        atkText.text = card.atk.ToString();
        hpText.text = card.hp.ToString();
        defText.text = card.def.ToString();
        ratingImage.color = card.ratingColor;
        typeBackImage.color = card.ratingColor;
        cardBackImage.color = card.ratingColor;
        gameObject.SetActive(true);
    }

    public void ClickCard()
    {
        if(card != null)
        {
            isCheck = !isCheck;

            if (isCheck)
            {
                checkImage.SetActive(true);
                Inventory.instance.AddSlot(card);
                PlayerCard.instance.cardList.Add(card);
            }
            else
            {
                checkImage.SetActive(false);
                Inventory.instance.RemoveSlot(card);
                PlayerCard.instance.cardList.Remove(card);
            }
        }
    }

    public void UpGradeCardClick()
    {
        if (card != null)
        {
            isCheck = !isCheck;

            if (isCheck)
            {
                checkImage.SetActive(true);
                UpGradeManager.instance.ClickCard(card);
                UpGradeManager.instance.curSlot = this;
            }
            else
            {
                checkImage.SetActive(false);
                UpGradeManager.instance.ClearCard();
            }
        }
    }
}
