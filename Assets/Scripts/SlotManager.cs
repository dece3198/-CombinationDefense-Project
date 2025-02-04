using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public Slot[] slots;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private Card[] cards;


    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void LuckyDip()
    {
        int rand = Random.Range(0, cards.Length);

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == null)
            {
                slots[i].AddCard(cards[rand]);
                return;
            }
        }
    }
}
