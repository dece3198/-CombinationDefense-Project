using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator instance;
    [SerializeField] private List<GameObject> mercenaryList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < PlayerCard.instance.cardList.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject card = Instantiate(PlayerCard.instance.cardList[i].prefab, transform);
                mercenaryList.Add(card);
            }
        }
    }

    public void ExitCard(Card card)
    {
        int count = 0;

        for (int i = 0; i < mercenaryList.Count; i++)
        {
            if (card.type == mercenaryList[i].GetComponent<Mercenary>().type)
            {
                if(card.rating == mercenaryList[i].GetComponent<Mercenary>().rating)
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

            if (card.type == mercenaryList[i].GetComponent<Mercenary>().type)
            {
                if (card.rating == mercenaryList[i].GetComponent<Mercenary>().rating)
                {
                    mercenaryList[i].SetActive(true);
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
