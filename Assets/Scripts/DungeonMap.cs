using System.Collections.Generic;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    public Transform tpPos;
    public Transform teleportPos;
    public Transform[] monsterPos;
    public List<Transform> posList = new List<Transform>();

    public void AddPos()
    {
        posList.Clear();
        for(int i = 0; i < monsterPos.Length; i++)
        {
            posList.Add(monsterPos[i]);
        }
    }
}
