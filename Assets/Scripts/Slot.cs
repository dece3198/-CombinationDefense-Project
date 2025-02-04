using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Card card;
    public Image cardImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingTextA;
    [SerializeField] private TextMeshProUGUI ratingTextB;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;

    public void AddCard(Card _card)
    {
        card = _card;
        cardImage.sprite = card.cardImage;
        nameText.text = card.cardName;
        ratingTextA.text = card.rating.ToString();
        ratingTextB.text = card.rating.ToString();
        atkText.text = card.atk.ToString();
        hpText.text = card.hp.ToString();
        defText.text = card.def.ToString();
        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        card = null;
        cardImage.sprite = null;
        gameObject.SetActive(false);
    }
}
