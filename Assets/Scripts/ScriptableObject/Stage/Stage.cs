using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "New Stage/Stage")]
public class Stage : ScriptableObject
{
    public Card[] monsters;
    public int monsterCount;
    public int startGold;
    public int gold;
    public Card compensationCard;
    public Sprite stageImage;
    [SerializeField , TextArea(2,2)]
    public string stageText;
    public string compensation;
}
