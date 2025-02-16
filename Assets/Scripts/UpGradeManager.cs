using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager instance;
    [SerializeField] private Card curCard;
    [SerializeField] private GameObject curobj;
    public InventorySlot[] slots;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject[] card;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI levelText;
    public InventorySlot curSlot;
    private bool isCard = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<InventorySlot>();

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == null)
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == null)
            {
                slots[i].AddCard(PlayerCard.instance.cardList[0]);
                return;
            }
        }
    }

    public void ClickCard(Card _card)
    {
        if(curCard != null)
        {
            ClearCard();
        }


        for(int i = 0; i < card.Length; i++)
        {
            if (card[i].GetComponent<Mercenary>().card == _card)
            {
                if (isCard)
                {
                    card[i].gameObject.SetActive(true);
                    curobj = card[i];
                    curCard = _card;
                    TextReset();
                    return;
                }
                else
                {
                    ClearCard();
                    return;
                }
            }
        }
    }

    public void UpButton()
    {
        if(curCard.level >= 25)
        {
            return;
        }
        
        if(GameManager.instance.money >= (curCard.level + 1))
        {
            curCard.atk += 1;
            curCard.hp += 25;
            curCard.def += 1;
        }

        TextReset();
    }

    public void ClearCard()
    {
        curSlot.checkImage.SetActive(false);
        curSlot.isCheck = false;
        curobj.SetActive(false);
        levelText.text = "";
        atkText.text = "";
        hpText.text = "";
        defText.text = "";
        curCard = null;
        curobj = null;
        isCard = true;
    }

    private void TextReset()
    {
        if (curCard.level >= 24)
        {
            levelText.text = "Max레벨";
        }
        else
        {
            levelText.text = curCard.level.ToString() + "레벨\n" + "비용 : " + (curCard.level + 1).ToString() + "$";
        }

        atkText.text = curCard.atk.ToString() + " -> " + (curCard.atk + 1).ToString();
        hpText.text = curCard.hp.ToString() + " -> " + (curCard.hp + 25).ToString();
        defText.text = curCard.def.ToString() + " -> " + (curCard.def + 1).ToString();
    }
}
