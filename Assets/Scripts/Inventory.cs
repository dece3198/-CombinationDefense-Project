using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventorySlot[] slots;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private InventorySlot[] playerSlot;
    [SerializeField] private Transform[] slotPos;
    [SerializeField] private GameObject[] mercenary;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

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
        for(int i = 0; i < playerSlot.Length; i++)
        {
            if (playerSlot[i].card == null)
            {
                playerSlot[i].card = card;
                playerSlot[i].gameObject.SetActive(false);
                for(int j = 0; j < mercenary.Length; j++)
                {
                    if (mercenary[j].GetComponent<Mercenary>().card == card)
                    {
                        mercenary[j].SetActive(true);
                        mercenary[j].transform.position = slotPos[i].position;
                        return;
                    }
                }
            }
        }
    }

    public void RemoveSlot(Card card)
    {
        for(int i = 0; i < playerSlot.Length; i++)
        {
            if (playerSlot[i].card == card)
            {
                playerSlot[i].card = null;
                playerSlot[i].gameObject.SetActive(true);
                for (int j = 0; j < mercenary.Length; j++)
                {
                    if (mercenary[j].GetComponent<Mercenary>().card == card)
                    {
                        mercenary[j].SetActive(false);
                        return;
                    }
                }
            }
        }
    }

    public void clickSound()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
