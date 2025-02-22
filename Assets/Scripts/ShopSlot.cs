using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GetCoin[] coins;
    [SerializeField] private int count = 0;

    public void SlotClick()
    {
        for(int i = 0; i < count; i++)
        {
            coins[i].target = target;
            coins[i].startPos = transform;
            coins[i].gameObject.SetActive(true);
        }
    }
}
