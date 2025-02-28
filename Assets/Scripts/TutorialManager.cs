using System.Collections;
using UnityEngine;
public enum TutorialState
{
    None, First, Two
}

public class NoneState : BaseState<TutorialManager>
{
    public override void Enter(TutorialManager tutorial)
    {

    }

    public override void Exit(TutorialManager tutorial)
    {

    }

    public override void Update(TutorialManager tutorial)
    {

    }
}


public class FirstState : BaseState<TutorialManager>
{
    public override void Enter(TutorialManager tutorial)
    {
        if(GameManager.instance.isGame)
        {
            tutorial.obstacleImage.SetActive(true);
            tutorial.tutorialA.SetActive(true);
            tutorial.StartCoroutine(tutorialCo(tutorial));
        }
    }

    public override void Exit(TutorialManager tutorial)
    {

    }

    public override void Update(TutorialManager tutorial)
    {
        if (SlotManager.instance.slots[0].card != null)
        {
            tutorial.obstacleImage.SetActive(false);
            tutorial.direction.SetActive(true);
            tutorial.tutorialA.SetActive(false);
            tutorial.handA.SetActive(false);
        }

        if(GameManager.instance.mecrenary.Count > 0)
        {
            Time.timeScale = 1;
            tutorial.direction.SetActive(false);
        }

        if (MapManager.instance.map.gameObject.activeSelf)
        {
            DataManager.instance.curData.isFirstTutorial = true;
            DataManager.instance.curData.isFirstStageClear = true;
            tutorial.obstacleImage.SetActive(true);
            tutorial.tutorialB.SetActive(true);
            tutorial.handB.SetActive(true);
            GameManager.instance.tutorial.gameObject.SetActive(false);
            DataManager.instance.SaveData();
        }

        if(StageMenu.instance.menu.gameObject.activeSelf)
        {
            tutorial.obstacleImage.SetActive(false);
            tutorial.tutorialB.SetActive(false);
            tutorial.handB.SetActive(false);
            tutorial.ChangeState(TutorialState.None);
        }
    }

    private IEnumerator tutorialCo(TutorialManager tutorial)
    {
        yield return new WaitForSeconds(1f);
        tutorial.handA.SetActive(true);
        Time.timeScale = 0;
    }
}

public class TwoState : BaseState<TutorialManager>
{
    public override void Enter(TutorialManager tutorial)
    {
    }

    public override void Exit(TutorialManager tutorial)
    {

    }

    public override void Update(TutorialManager tutorial)
    {
        if(MapManager.instance.map.gameObject.activeSelf)
        {
            if(!Inventory.instance.inventory.activeSelf)
            {
                DataManager.instance.curData.tutorialState = tutorial.state.ToString();
                DataManager.instance.SaveData();
                tutorial.obstacleImage.SetActive(true);
                tutorial.tutorialC.SetActive(true);
                tutorial.handC.SetActive(true);
            }
        }

        if (Inventory.instance.playerSlot[1].card != null)
        {
            tutorial.obstacleImage.SetActive(false);
            tutorial.handD.SetActive(false);
            tutorial.ChangeState(TutorialState.None);
            DataManager.instance.curData.tutorialState = tutorial.state.ToString();
            DataManager.instance.SaveData();
        }
    }

    private IEnumerator tutorialCo(TutorialManager tutorial)
    {
        yield return new WaitForSeconds(1f);
        tutorial.handA.SetActive(true);
    }
}


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public GameObject tutorialA;
    public GameObject tutorialB;
    public GameObject tutorialC;
    public GameObject handA;
    public GameObject handB;
    public GameObject handC;
    public GameObject handD;
    public GameObject direction;
    public GameObject obstacleImage;
    public TutorialState state;

    private StateMachine<TutorialState, TutorialManager> stateMachine = new StateMachine<TutorialState, TutorialManager>();

    private void Awake()
    {
        instance = this;
        stateMachine.Reset(this);
        stateMachine.AddState(TutorialState.None, new NoneState());
        stateMachine.AddState(TutorialState.First, new FirstState());
        stateMachine.AddState(TutorialState.Two, new TwoState());
        stateMachine.ChangeState(TutorialState.None);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(TutorialState _state)
    {
        stateMachine.ChangeState(_state);
        state = _state;
    }
}
