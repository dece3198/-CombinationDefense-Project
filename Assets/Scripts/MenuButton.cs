using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject menu;
    public Image image;
    public Sprite pressed;
    public Sprite button;
    private Vector2 originScale;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        originScale = menu.transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(ButtonManager.instance.curButton != null)
            {
                ButtonManager.instance.curButton.image.sprite = ButtonManager.instance.curButton.button;
                ButtonManager.instance.curButton.menu.SetActive(false);
            }
            StartCoroutine(SizeCo());
            menu.SetActive(true);
            ButtonManager.instance.curButton = this;
            ButtonManager.instance.ButtonSound();
            image.sprite = pressed;
        }
    }

    public void tutorialClick()
    {
        if (ButtonManager.instance.curButton != null)
        {
            ButtonManager.instance.curButton.image.sprite = ButtonManager.instance.curButton.button;
            ButtonManager.instance.curButton.menu.SetActive(false);
        }
        StartCoroutine(SizeCo());
        menu.SetActive(true);
        ButtonManager.instance.curButton = this;
        ButtonManager.instance.ButtonSound();
        image.sprite = pressed;
        TutorialManager.instance.tutorialC.SetActive(false);
        TutorialManager.instance.handC.SetActive(false);
        TutorialManager.instance.handD.SetActive(true);
    }

    private IEnumerator SizeCo()
    {
        float time = 1;
        while(time < 1.1)
        {
            time += Time.deltaTime;
            menu.transform.localScale = originScale * time;
            yield return null;
        }
        menu.transform.localScale = originScale;
    }
}
