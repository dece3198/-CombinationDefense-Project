using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;
    public Slot[] slots;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject surrender;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public bool isTime = false;
    private bool isSurrender = false;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SpeedUp()
    {
        isTime = !isTime;

        if(isTime)
        {
            audioSource.PlayOneShot(audioClips[1]);
            SetColor(1);
            animator.SetBool("X2", true);
            Time.timeScale = 2;
        }
        else
        {
            audioSource.PlayOneShot(audioClips[2]);
            SetColor(0);
            animator.SetBool("X2", false);
            Time.timeScale = 1;
        }
    }

    public void CheckButton()
    {
        isSurrender = false;
        surrender.SetActive(false);
        StageManager.instance.FailCheckButton();
    }

    private void SetColor(float alpha)
    {
        Color color = animator.gameObject.GetComponent<Image>().color;
        color.a = alpha;
        animator.gameObject.GetComponent<Image>().color = color;
    }

    public void SurrenderButton()
    {
        isSurrender = !isSurrender;

        if(isSurrender)
        {
            if(GameManager.instance.isGame)
            {
                audioSource.PlayOneShot(audioClips[1]);
            }
            surrender.SetActive(true);
        }
        else
        {
            if (GameManager.instance.isGame)
            {
                audioSource.PlayOneShot(audioClips[2]);
            }
            surrender.SetActive(false);
        }
    }

    public void LuckyDipA()
    {
        audioSource.PlayOneShot(audioClips[5]);
        if (PlayerCard.instance.cardList.Count == 0 || GameManager.instance.Gold >= 2)
        {
            audioSource.PlayOneShot(audioClips[3]);
            GameManager.instance.Gold -= 2;
            int rand = Random.Range(0, PlayerCard.instance.cardList.Count);
            var emptySlot = slots.FirstOrDefault(slot => slot.card == null);
            if(emptySlot != null)
            {
                emptySlot.AddCard(PlayerCard.instance.cardList[rand]);
            }
        }
    }

    public void LuckyDipB()
    {
        audioSource.PlayOneShot(audioClips[5]);
        if (PlayerCard.instance.specialCardList.Count == 0 || GameManager.instance.Gold >= 5)
        {
            audioSource.PlayOneShot(audioClips[3]);
            GameManager.instance.Gold -= 5;
            int rand = Random.Range(0, PlayerCard.instance.specialCardList.Count);
            var emptySlot = slots.FirstOrDefault(slot => slot.card == null);
            if (emptySlot != null)
            {
                emptySlot.AddCard(PlayerCard.instance.specialCardList[rand]);
            }
        }
    }
}
