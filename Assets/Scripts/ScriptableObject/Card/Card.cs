using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Mercenary, Monster
}

[CreateAssetMenu(fileName = "New Card", menuName = "New Card/Card")]
public class Card : ScriptableObject
{
    public Sprite cardImage;
    public string cardName;
    public int rating;
    public float atk;
    public float hp;
    public float def;
    public GameObject prefab;
    public Card nextCard;
    public Color ratingColor;
    public Color classColor;
    public WeaponType type;
    public CardType cardType;
    public int gold;
}
