using UnityEngine;

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
}
