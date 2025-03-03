using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RandomCard : MonoBehaviour
{
    public static RandomCard instance;
    private Animator animator;
    public Card card;
    public Image charImage;
    [SerializeField] private Image cardBackImage;
    [SerializeField] private Image typeImage;
    [SerializeField] private Image typeBackImage;
    [SerializeField] private Image ratingImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private Image dissolveImage;
    [SerializeField] private GameObject verso;
    [SerializeField] private GameObject clickText;
    [SerializeField] private GameObject okButton;
    private int amount = Shader.PropertyToID("_DissolveAmount");
    public int gold = 0;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        cardBackImage = GetComponent<Image>();
    }

    public void AddCard(Card _card)
    {
        clickText.SetActive(true);
        okButton.SetActive(false);
        card = _card;
        gold = Random.Range(1, card.gold);
        charImage.sprite = card.cardImage;
        typeImage.sprite = card.typeImage;
        if (card.type == WeaponType.Money)
        {
            nameText.text = gold.ToString() + "¸Ó´Ï";
        }
        else
        {
            nameText.text = card.cardName;
        }
        ratingText.text = card.rating.ToString();
        atkText.text = card.atk.ToString();
        hpText.text = card.hp.ToString();
        defText.text = card.def.ToString();
        ratingImage.color = card.ratingColor;
        typeBackImage.color = card.ratingColor;
        cardBackImage.color = card.ratingColor;
    }

    public void CardClick()
    {
        animator.Play("Verso");
        clickText.SetActive(false);
    }

    public void Verso()
    {
        StartCoroutine(Vanish());
        verso.SetActive(false);
    }

    public void OkButton()
    {
        if(card != null)
        {
            if(card.type == WeaponType.Money)
            {
                GameManager.instance.money += gold;
            }
            else
            {
                Inventory.instance.AcquireCard(card);
                UpGradeManager.instance.AcquireCard(card);
                
            }
            GameManager.instance.SaveData();
            verso.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private IEnumerator Vanish()
    {
        dissolveImage.material.SetFloat(amount, 0);
        yield return new WaitForSeconds(0.5f);
        float time = 2;

        while (time > 0)
        {
            time -= Time.deltaTime;
            float dissolveTime = Mathf.Lerp(1f, 0, (time / 2));
            dissolveImage.material.SetFloat(amount, dissolveTime);
            yield return null;
        }
        okButton.SetActive(true);
    }
}
