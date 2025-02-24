using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class SpecialCard
{
    public Card card;
    public int weigjt;

    public SpecialCard(SpecialCard specialCard)
    {
        this.card = specialCard.card;
        this.weigjt = specialCard.weigjt;
    }

}

public class RandomSelect : MonoBehaviour
{
    public List<SpecialCard> cardList = new List<SpecialCard>();
    public int total = 0;
    public SpecialCard curCard;
    [SerializeField] private GameObject randCard;
    [SerializeField] private TextMeshProUGUI lockText;
    [SerializeField] float magnitude;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    [SerializeField] private int price = 0;
    [SerializeField] private string lockstr;

    public SpecialCard RandomCardSelect()
    {
        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < cardList.Count; i++)
        {
            weight += cardList[i].weigjt;
            if (selectNum <= weight)
            {
                SpecialCard temp = new SpecialCard(cardList[i]);
                return temp;
            }
        }

        return null;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            total += cardList[i].weigjt;
        }
    }

    public void SelectCard()
    {
        if(GameManager.instance.crystal > price)
        {
            GameManager.instance.crystal -= price;
            curCard = RandomCardSelect();
            randCard.SetActive(true);
            RandomCard.instance.AddCard(curCard.card);
            if (curCard.card.type != WeaponType.Money)
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    if (cardList[i].card == curCard.card)
                    {

                        cardList.RemoveAt(i);
                    }
                }
            }
        }
        else
        {
            lockText.text = "크리스탈이 부족합니다.";
            StartCoroutine(LockCo());
            audioSource.PlayOneShot(audioClips[0]);
        }
    }

    public void lockButton()
    {
        lockText.text = lockstr;
        StartCoroutine(LockCo());
        audioSource.PlayOneShot(audioClips[0]);
    }

    private IEnumerator LockCo()
    {
        lockText.gameObject.SetActive(true);
        float time = 0.5f;
        Vector3 origingPos = lockText.transform.position;
        while(time > 0)
        {
            time -= Time.deltaTime;
            lockText.transform.position = Random.insideUnitSphere * magnitude + origingPos;
            yield return null;
        }
        lockText.transform.position = origingPos;
        lockText.gameObject.SetActive(false);
    }
}
