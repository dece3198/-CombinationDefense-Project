using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;
    public Slot[] slots;
    [SerializeField] private GameObject slotParent;
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

    public void LuckyDip()
    {
        if(GameManager.instance.gold >= 2)
        {
            GameManager.instance.gold -= 2;
        }
        else
        {
            return;
        }
        int rand = Random.Range(0, PlayerCard.instance.cardList.Count);

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == null)
            {
                slots[i].AddCard(PlayerCard.instance.cardList[rand]);
                return;
            }
        }
    }
}
