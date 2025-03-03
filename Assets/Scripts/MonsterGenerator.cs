using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        if(monsterList == null || !monsterList.Any(m => m.GetComponent<Mercenary>().card == card))
        {
            Refill(card, 5);
        }

        for (int i = 0; i < monsterList.Count; i++)
        {
            if (card == monsterList[i].GetComponent<Mercenary>().card)
            {
                monsterList[i].SetActive(true);
                GameManager.instance.monster.Add(monsterList[i]);
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
        StageManager.instance.curCount++;
    }

    public IEnumerator MonsterCo()
    {
        for(int i = 0; i < StageManager.instance.curStage.stage.monsterCount; i++)
        {
            int rand = Random.Range(10, 20);
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
