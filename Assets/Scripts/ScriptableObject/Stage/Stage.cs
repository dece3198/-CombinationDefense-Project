using UnityEngine;

public enum StageType
{
    Normal, Boss, Tutorial
}


[CreateAssetMenu(fileName = "New Stage", menuName = "New Stage/Stage")]
public class Stage : ScriptableObject
{
    public Card[] monsters;
    public int monsterCount;
    public int startGold;
    public int money;
    public int crystal;
    public int stageNumber;
    public Card compensationCard;
    public Sprite stageImage;
    [SerializeField , TextArea(2,2)]
    public string stageText;
    public string compensation;
    public StageType stageType;
}
