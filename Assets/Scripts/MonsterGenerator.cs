using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public static MonsterGenerator instance;
    [SerializeField] private Card[] monsters;
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < monsters.Length; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                GameObject monster = Instantiate(monsters[i].prefab, transform);
                monsterList.Add(monster);
            }
        }
    }

    public void ExitMonster(Card card)
    {
        int count = 0;

        for (int i = 0; i < monsterList.Count; i++)
        {
            if (card.type == monsterList[i].GetComponent<Mercenary>().weaponType)
            {
                if (card.rating == monsterList[i].GetComponent<Mercenary>().card.rating)
                {
                    count++;
                }
            }
        }

        if (count <= 0)
        {
            Refill(card, 5);
        }

        for (int i = 0; i < monsterList.Count; i++)
        {

            if (card.type == monsterList[i].GetComponent<Mercenary>().weaponType)
            {
                if (card.rating == monsterList[i].GetComponent<Mercenary>().card.rating)
                {
                    monsterList[i].SetActive(true);
                    GameManager.instance.monster.Add(monsterList[i]);
                    monsterList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void EnterMonster(GameObject monster)
    {
        GameManager.instance.monster.Remove(monster);
        monster.transform.position = transform.position;
        monsterList.Add(monster);
        monster.SetActive(false);
        StageManager.instance.curCount++;
    }

    public void StartGame()
    {
        StartCoroutine(MonsterCo());
    }

    private IEnumerator MonsterCo()
    {
        
        for(int i = 0; i < StageManager.instance.curStage.stage.monsterCount; i++)
        {
            int rand = Random.Range(3, 10);
            int randCard = Random.Range(0, StageManager.instance.curStage.stage.monsters.Length);
            ExitMonster(StageManager.instance.curStage.stage.monsters[randCard]);
            yield return new WaitForSeconds(rand);
        }
    }

    private void Refill(Card card, int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject _card = Instantiate(card.prefab, transform);
            monsterList.Add(_card);
        }
    }
}
