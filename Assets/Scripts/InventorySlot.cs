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
    public float atk = 0;
    public float hp = 0;
    public float def = 0;
    public GameObject checkImage;
    public bool isCheck = false;

    public void AddCard(Card _card)
    {
        card = _card;
        charImage.sprite = card.cardImage;
        typeImage.sprite = card.typeImage;
        nameText.text = card.cardName;
        ratingText.text = card.rating.ToString();
        ratingImage.color = card.ratingColor;
        typeBackImage.color = card.ratingColor;
        cardBackImage.color = card.ratingColor;
        atk = card.atk + (card.level * 0.1f);
        hp = card.hp + (card.level * 1f);
        def = card.def + (card.level * 0.1f);
        atkText.text = atk.ToString();
        hpText.text = hp.ToString();
        defText.text = def.ToString();
        gameObject.SetActive(true);
    }

    public void LevelUp()
    {
        atk = card.atk + (card.level * 0.1f);
        hp = card.hp + (card.level * 1f);
        def = card.def + (card.level * 0.1f);
        atkText.text = atk.ToString();
        hpText.text = hp.ToString();
        defText.text = def.ToString();
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
                UpGradeManager.instance.ClickCard(this);
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
