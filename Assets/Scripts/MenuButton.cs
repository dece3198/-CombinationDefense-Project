using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject menu;
    public Image image;
    public Sprite pressed;
    public Sprite button;

    private void Awake()
    {
        image = GetComponent<Image>();
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
            menu.SetActive(true);
            ButtonManager.instance.curButton = this;
            image.sprite = pressed;
        }
    }
}
