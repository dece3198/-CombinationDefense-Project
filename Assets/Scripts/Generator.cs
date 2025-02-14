using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator instance;
    public Card[] cards;
    [SerializeField] private List<GameObject> mercenaryList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject card = Instantiate(cards[i].prefab, transform);
                mercenaryList.Add(card);
            }
        }
    }

    public void ExitCard(Card card)
    {
        int count = 0;

        for (int i = 0; i < mercenaryList.Count; i++)
        {
            if (card.type == mercenaryList[i].GetComponent<Mercenary>().weaponType)
            {
                if(card.rating == mercenaryList[i].GetComponent<Mercenary>().card.rating)
                {
                    count++;
                }
            }
        }

        if(count <= 0)
        {
            Refill(card,5);
        }

        for (int i = 0; i < mercenaryList.Count; i++)
        {

            if (card.type == mercenaryList[i].GetComponent<Mercenary>().weaponType)
            {
                if (card.rating == mercenaryList[i].GetComponent<Mercenary>().card.rating)
                {
                    mercenaryList[i].SetActive(true);
                    GameManager.instance.mecrenary.Add(mercenaryList[i]);
                    mercenaryList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void EnterCard(GameObject card)
    {
        card.transform.position = transform.position;
        mercenaryList.Add(card);
        card.SetActive(false);
    }

    private void Refill(Card card, int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject _card = Instantiate(card.prefab, transform);
            mercenaryList.Add(_card);
        }
    }
}
