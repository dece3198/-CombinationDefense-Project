using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    public static PlayerCard instance;
    public List<Card> cardList = new List<Card>();
    public List<Card> specialCardList = new List<Card>();
    public Card[] cards;
    public Dictionary<Card, int> cardDic = new Dictionary<Card, int>();
    public Dictionary<int, Card> cardNumberDic = new Dictionary<int, Card>();

    private void Awake()
    {
        instance = this;

        for(int i = 0; i < cards.Length; i++)
        {
            cardDic.Add(cards[i], i);
            cardNumberDic.Add(i, cards[i]);
        }
    }
}
