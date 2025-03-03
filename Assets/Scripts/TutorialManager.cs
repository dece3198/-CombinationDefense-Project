using DG.Tweening;
using System.Collections;
using UnityEngine;
public enum TutorialState
{
    None, First, Two, Three
}

public class NoneState : BaseState<TutorialManager>
{
    public override void Enter(TutorialManager tutorial)
    {
        tutorial.isUpdate = false;
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
    private bool isFirst = true;
    private bool isTwo = true;
    private bool isThree = true;
    Vector3 temScale;
    public override void Enter(TutorialManager tutorial)
    {
        tutorial.isUpdate = true;
        isFirst = true;
        isTwo = true;
        isThree = true;
        if (GameManager.instance.isGame)
        {
            temScale = tutorial.tutorialA.transform.localScale;
            tutorial.obstacleImage.SetActive(true);
            tutorial.tutorialA.SetActive(true);
            tutorial.tutorialA.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.Linear).SetUpdate(true);
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
            if(isFirst)
            {
                tutorial.tutorialA.transform.localScale = temScale;
                isFirst = false;
                tutorial.obstacleImage.SetActive(false);
                tutorial.direction.SetActive(true);
                tutorial.tutorialA.SetActive(false);
                tutorial.handA.SetActive(false);
            }
        }

        if(GameManager.instance.mecrenary.Count > 0)
        {
            if(isTwo)
            {
                isTwo = false;
                Time.timeScale = 1;
                tutorial.direction.SetActive(false);
            }
        }

        if (MapManager.instance.map.gameObject.activeSelf)
        {
            if(isThree)
            {
                isThree = false;
                temScale = tutorial.tutorialB.transform.localScale;
                DataManager.instance.curData.isFirstTutorial = false;
                DataManager.instance.curData.tutorialState = tutorial.state.ToString();
                tutorial.obstacleImage.SetActive(true);
                tutorial.tutorialB.SetActive(true);
                tutorial.tutorialB.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.Linear).SetUpdate(true);
                tutorial.handB.SetActive(true);
                GameManager.instance.tutorial.gameObject.SetActive(false);
                GameManager.instance.SaveData();
            }
        }

        if(StageMenu.instance.menu.gameObject.activeSelf)
        {
            tutorial.tutorialB.transform.localScale = temScale;
            tutorial.obstacleImage.SetActive(false);
            tutorial.tutorialB.SetActive(false);
            tutorial.handB.SetActive(false);
            tutorial.ChangeState(TutorialState.None);
        }
    }

    private IEnumerator tutorialCo(TutorialManager tutorial)
    {
        yield return new WaitForSeconds(1f);
        if (SlotManager.instance.slots[0].card == null)
        {
            tutorial.handA.SetActive(true);
        }
        Time.timeScale = 0;
    }
}

public class TwoState : BaseState<TutorialManager>
{
    Vector3 tempScale;

    public override void Enter(TutorialManager tutorial)
    {
        tutorial.isUpdate = true;
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
                tempScale = tutorial.tutorialC.transform.localScale;
                DataManager.instance.curData.tutorialState = tutorial.state.ToString();
                GameManager.instance.SaveData();
                tutorial.obstacleImage.SetActive(true);
                tutorial.tutorialC.SetActive(true);
                tutorial.tutorialC.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.Linear).SetUpdate(true);
                tutorial.handC.SetActive(true);
            }
        }

        if (Inventory.instance.playerSlot[1].card != null)
        {
            tutorial.obstacleImage.SetActive(false);
            tutorial.handD.SetActive(false);
            tutorial.tutorialC.transform.localScale = tempScale;
            tutorial.ChangeState(TutorialState.None);
            DataManager.instance.curData.tutorialState = tutorial.state.ToString();
            GameManager.instance.SaveData();
        }
    }
}

public class ThreeState : BaseState<TutorialManager>
{
    public override void Enter(TutorialManager tutorial)
    {
        tutorial.isUpdate = true;
    }

    public override void Exit(TutorialManager tutorial)
    {

    }

    public override void Update(TutorialManager tutorial)
    {
        if(MapManager.instance.map.activeSelf)
        {
            tutorial.mixTutorial.SetActive(true);
            tutorial.ChangeState(TutorialState.None);
        }
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
    public GameObject mixTutorial;
    public TutorialState state;
    public bool isUpdate = true;

    private StateMachine<TutorialState, TutorialManager> stateMachine = new StateMachine<TutorialState, TutorialManager>();

    private void Awake()
    {
        instance = this;
        stateMachine.Reset(this);
        stateMachine.AddState(TutorialState.None, new NoneState());
        stateMachine.AddState(TutorialState.First, new FirstState());
        stateMachine.AddState(TutorialState.Two, new TwoState());
        stateMachine.AddState(TutorialState.Three, new ThreeState());
        stateMachine.ChangeState(TutorialState.None);
    }

    private void Update()
    {
        if(isUpdate)
        {
            stateMachine.Update();
        }
    }

    public void MixCheckButton()
    {
        mixTutorial.SetActive(false);
    }

    public void ChangeState(TutorialState _state)
    {
        stateMachine.ChangeState(_state);
        state = _state;
    }
}
