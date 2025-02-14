using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int count = 0;
    public int cash = 0;
    public int gold = 0;
    public List<GameObject> monster = new List<GameObject>();
    public List<GameObject> mecrenary = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject mainMenu;
    private bool isTime = false;

    public bool isMix = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        goldText.text = gold.ToString();
    }

    public void StartButton()
    {
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(1));
        MapManager.instance.map.SetActive(false);
        MapManager.instance.animator.SetBool("Close", true);
        MapManager.instance.animator.Play("Close");
        StageMenu.instance.menu.SetActive(false);
        gold = StageManager.instance.curStage.stage.startGold;
    }

    public void NewGameButton()
    {
        Fade.instance.FadeInOut();
        StartCoroutine(FadeCo(0));
    }

    public void GameReset()
    {
        for(int i = 0; i < monster.Count; i++)
        {
            MonsterGenerator.instance.EnterMonster(monster[i]);
        }

        for(int j = 0; j < mecrenary.Count; j++)
        {
            Generator.instance.EnterCard(mecrenary[j]);
        }

        for(int k = 0; k < SlotManager.instance.slots.Length; k++)
        {
            if(SlotManager.instance.slots[k].card != null)
            {
                SlotManager.instance.slots[k].ClearSlot();
            }
        }
    }

    public void SpeedUp()
    {
        isTime = !isTime;
        if (isTime)
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }

    }


    private IEnumerator FadeCo(int number)
    {
        while (Fade.instance.isFade)
        {
            yield return null;
        }
        
        if(number == 0)
        {
            mainMenu.SetActive(false);
            MapManager.instance.mapParent.SetActive(true);
            MapManager.instance.animator.Play("Open");
            yield return new WaitForSeconds(0.83f);
            MapManager.instance.map.SetActive(true);
        }
        else
        {
            MonsterGenerator.instance.StartGame();
            MapManager.instance.animator.SetBool("Close", true);
            MapManager.instance.mapParent.SetActive(false);
        }
    }
}
