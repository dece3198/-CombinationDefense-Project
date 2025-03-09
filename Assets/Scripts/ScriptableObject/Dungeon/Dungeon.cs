using System;
using UnityEngine;

[System.Serializable]
public class DungeonStage
{
    public GameObject[] monster;
    public int monsterCount;
}


[CreateAssetMenu(fileName = "New Dungeon", menuName = "New Dungeon/Dungeon")]
public class Dungeon : ScriptableObject
{
    public string dungeonName;
    public DungeonStage[] stage;
}
