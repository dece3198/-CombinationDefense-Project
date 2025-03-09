using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public Dungeon curDungeon;
    public DungeonMap curDungeonMap;
    public DungeonMap waitingRoom;
    public GameObject player;
    public Teleport dungeonTeleport;
    [SerializeField] private DungeonMap[] dungeonsA;
    [SerializeField] private DungeonMap[] dungeonsB;
    [SerializeField] private DungeonMap[] dungeonsC;
    [SerializeField] private Dictionary<int, DungeonMap[]> dungeonDic = new Dictionary<int, DungeonMap[]>();
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>();
    [SerializeField] private GameObject dungeonUi;
    [SerializeField] private Dungeon[] dungeons;
    [SerializeField] private int dungeonCount = 0;
    [SerializeField] private int count = 0;
    public int stageCount = 0;
    private bool isDungeon = false;

    private void Awake()
    {
        instance = this;
        dungeonDic.Add(0, dungeonsA);
        dungeonDic.Add(1, dungeonsB);
        dungeonDic.Add(2, dungeonsC);

    }

    private void Start()
    {
        for(int i = 0; i < monsters.Length; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                GameObject monster = Instantiate(monsters[i], transform);
                monsterList.Add(monster);
            }
        }
    }

    private void Update()
    {
        if(curDungeon != null)
        {
            if (count >= curDungeon.stage[stageCount].monsterCount)
            {
                count = 0;
                stageCount++;
                GameStart();
            }
        }
    }

    //현재 스테이지에 따라 랜덤맵을 부여
    public void GameStart()
    {
        dungeonTeleport.gameObject.SetActive(true);
        dungeonTeleport.transform.position = curDungeonMap.teleportPos.position;
        if (curDungeon != null)
        {
            if (curDungeon.stage.Length == stageCount)
            {
                //보스 스테이지를 넣는다
            }
            else
            {
                int rand = Random.Range(0, dungeonDic[stageCount].Length);
                dungeonTeleport.nextDungeon = dungeonDic[stageCount][rand];
            }
        }
    }

    
    public void ExitMonster()
    {
        //몬스터를 내보내기전 개수가 있는지 없는지 확인 없다면 5마리 추가 생성
        foreach (var monster in curDungeon.stage[stageCount].monster)
        {
            bool exists = monsterList.Any(m => m.GetComponent<Monster>().card == monster.GetComponent<Monster>().card);
            if (!exists)
            {
                Refill(5, monster);
            }
        }


        //현재스테이지에 알맞는 몬스터를 소환하고 랜덤한 위치에 생성
        for (int i = 0; i < monsterList.Count; i++)
        {
            int rand = Random.Range(0, curDungeon.stage[stageCount].monster.Length);
            int randPos = Random.Range(0, curDungeonMap.posList.Count);

            if (curDungeon.stage[stageCount].monster[rand].GetComponent<Monster>().card == monsterList[i].GetComponent<Monster>().card)
            {
                monsterList[i].SetActive(true);
                monsterList[i].transform.position = curDungeonMap.posList[randPos].position;
                curDungeonMap.posList.RemoveAt(randPos);
                monsterList.RemoveAt(i);
                return;
            }
        }
    }

    public void EnterMonster(GameObject monster)
    {
        monster.transform.position = transform.position;
        monsterList.Add(monster);
        monster.SetActive(false);
        count++;
    }

    private void Refill(int count, GameObject monster)
    {
        for (int j = 0; j < count; j++)
        {
            GameObject _monster = Instantiate(monster, transform);
            monsterList.Add(_monster);
        }
    }

    public void DungeonButtonClick()
    {
        isDungeon = !isDungeon;
        curDungeon = dungeons[dungeonCount];
        if (isDungeon)
        {
            dungeonUi.SetActive(true);
        }
        else
        {
            dungeonUi.SetActive(false);
        }
    }

    public void NextDungeon(int _count)
    {
        if(dungeonCount + _count < 0 || dungeonCount + _count > dungeons.Length)
        {
            return;
        }
        dungeonCount += _count;
        curDungeon = dungeons[dungeonCount];
    }
}
