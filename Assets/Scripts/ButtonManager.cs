using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    public MenuButton curButton;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void ButtonSound()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
