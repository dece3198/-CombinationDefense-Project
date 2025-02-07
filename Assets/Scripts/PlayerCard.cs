using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    public static PlayerCard instance;
    public List<Card> cardList = new List<Card>();

    private void Awake()
    {
        instance = this;
    }
}
