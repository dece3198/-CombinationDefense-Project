using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager instance;
    [SerializeField] private GameObject curobj;
    public InventorySlot curSlot;
    public InventorySlot[] slots;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject[] card;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Transform cardPos;
    private bool isCard = true;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
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

    }

    public void AcquireCard(Card card)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == null)
            {
                slots[i].AddCard(card);
                return;
            }
        }
    }


    public void ClickCard(InventorySlot slot)
    {
        if(curSlot != null)
        {
            ClearCard();
        }


        for(int i = 0; i < card.Length; i++)
        {
            if (card[i].GetComponent<Mercenary>().card == slot.card)
            {
                if (isCard)
                {
                    card[i].gameObject.SetActive(true);
                    card[i].transform.position = cardPos.position;
                    curobj = card[i];
                    curSlot = slot;
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
        if (curSlot != null || curSlot.card.level >= 100) return;

        int upgrade = curSlot.card.level + 1;

        if (GameManager.instance.money >= (curSlot.card.level + 1)) return;

        Card curCard = curSlot.card;
        while(curSlot != null)
        {
            curCard.level += 1;
            curCard = curCard.nextCard;
        }

        curSlot.LevelUp();

        foreach(var slot in Inventory.instance.slots.Where(slot => slot.card == curSlot.card))
        {
            slot.LevelUp();
        }

        audioSource.PlayOneShot(audioClips[0]);
        TextReset();
        GameManager.instance.SaveData();
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
        curSlot = null;
        curobj = null;
        isCard = true;
    }

    private void TextReset()
    {
        if (curSlot.card.level >= 100)
        {
            levelText.text = "Max레벨";
        }
        else
        {
            levelText.text = curSlot.card.level.ToString() + "레벨\n" + "비용 : " + (curSlot.card.level + 1).ToString() + "$";
        }

        atkText.text = curSlot.atk.ToString("N1") + " -> " + (curSlot.atk + 0.1).ToString("N1");
        hpText.text = curSlot.hp.ToString("N1") + " -> " + (curSlot.hp + 1).ToString("N1");
        defText.text = curSlot.def.ToString("N1") + " -> " + (curSlot.def + 0.1).ToString("N1");
    }
}
