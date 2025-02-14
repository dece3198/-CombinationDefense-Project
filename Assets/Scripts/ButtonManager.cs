using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    public MenuButton curButton;

    private void Awake()
    {
        instance = this;
    }
}
