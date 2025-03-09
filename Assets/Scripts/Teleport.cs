using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public DungeonMap nextDungeon;
    [SerializeField] private GameObject fade;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            StartCoroutine(TeleportCo(other));
        }
    }

    private IEnumerator TeleportCo(Collider other)
    {
        Vector3 tempScale = fade.transform.localScale;
        fade.transform.DOScale(Vector3.one, 1f);
        yield return new WaitForSeconds(1.25f);
        if(DungeonManager.instance.curDungeonMap != null)
        {
            DungeonManager.instance.curDungeonMap.gameObject.SetActive(false);
            DungeonManager.instance.curDungeonMap = nextDungeon;
        }
        nextDungeon.gameObject.SetActive(true);

        if(DungeonManager.instance.curDungeonMap.monsterPos.Length > 0)
        {
            DungeonManager.instance.curDungeonMap.AddPos();
        }

        for(int i = 0; i < DungeonManager.instance.curDungeon.stage[DungeonManager.instance.stageCount].monsterCount; i++)
        {
            DungeonManager.instance.ExitMonster();
        }
        other.transform.position = DungeonManager.instance.curDungeonMap.tpPos.position;
        fade.transform.DOScale(tempScale, 1f);
        gameObject.SetActive(false);
    }
}
