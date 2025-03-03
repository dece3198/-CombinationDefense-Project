using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventorySlot[] slots;
    [SerializeField] private GameObject slotParent;
    public InventorySlot[] playerSlot;
    public InventorySlot[] specialSlot;
    [SerializeField] private Transform[] slotPos;
    [SerializeField] private Transform[] specialSlotPos;
    [SerializeField] private GameObject[] mercenary;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    public GameObject inventory;

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
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].card == null)
            {
                slots[i].AddCard(card);
                return;
            }
        }
    }

    public void AddSlot(Card card)
    {
        bool isMercenary = card.cardType == CardType.Mercenary;
        var slotArray = isMercenary ? playerSlot : specialSlot;
        var slotPosArray = isMercenary ? slotPos : specialSlotPos;
        var playerCardList = isMercenary ? PlayerCard.instance.cardList : PlayerCard.instance.specialCardList;

        playerCardList.Add(card);

        var emptySlot = slotArray.FirstOrDefault(slot => slot.card == null);
        if(emptySlot != null)
        {
            emptySlot.card = card;
            emptySlot.gameObject.SetActive(false);

            var targetMercenary = mercenary.FirstOrDefault(m => m.GetComponent<Mercenary>().card == card);

            if(targetMercenary != null)
            {
                targetMercenary.SetActive(true);
                targetMercenary.transform.position = slotPosArray[System.Array.IndexOf(slotArray, emptySlot)].position;
            }
        }
    }

    public void RemoveSlot(Card card)
    {
        bool isMercenary = card.cardType == CardType.Mercenary;
        var slotArray = isMercenary ? playerSlot : specialSlot;
        var playerCardList = isMercenary ? PlayerCard.instance.cardList : PlayerCard.instance.specialCardList;

        playerCardList.Remove(card);

        var emptySlot = slotArray.FirstOrDefault(slot => slot.card == card);

        if(emptySlot != null)
        {
            emptySlot.card = null;
            emptySlot.gameObject.SetActive(true);

            var targetMercenary = mercenary.FirstOrDefault(m => m.GetComponent<Mercenary>().card == card);

            if(targetMercenary != null)
            {
                targetMercenary.SetActive(false);
            }
        }
    }

    public void clickSound()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
