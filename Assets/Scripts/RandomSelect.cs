using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public class SpecialCard
{
    public Card card;
    public int weigjt;

    public SpecialCard(SpecialCard specialCard)
    {
        this.card = specialCard.card;
        this.weigjt = specialCard.weigjt;
    }

}

public class RandomSelect : MonoBehaviour
{
    public List<SpecialCard> cardList = new List<SpecialCard>();
    public int total = 0;
    public SpecialCard curCard;
    [SerializeField] private GameObject randCard;

    public SpecialCard RandomCardSelect()
    {
        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < cardList.Count; i++)
        {
            weight += cardList[i].weigjt;
            if (selectNum <= weight)
            {
                SpecialCard temp = new SpecialCard(cardList[i]);
                return temp;
            }
        }

        return null;
    }

    private void Start()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            total += cardList[i].weigjt;
        }
    }

    public void SelectCard()
    {
        curCard = RandomCardSelect();
        randCard.SetActive(true);
        RandomCard.instance.AddCard(curCard.card);
        if(curCard.card.type == WeaponType.Money || curCard.card.type == WeaponType.Nothing)
        {

        }
        else
        {
            for(int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].card == curCard.card)
                {
                    
                    cardList.RemoveAt(i);
                }
            }
        }
    }
}
