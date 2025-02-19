using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


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
    [SerializeField] private GameObject verso;
    private Animator animator;
    private bool isRotation = true;
   [SerializeField] private Image image;
    private int amount = Shader.PropertyToID("_DissolveAmount");

    public SpecialCard RandomCard()
    {
        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for(int i = 0; i < cardList.Count; i++)
        {
            weight += cardList[i].weigjt;
            if(selectNum <= weight)
            {
                SpecialCard temp = new SpecialCard(cardList[i]);
                return temp;
            }
        }
        return null;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        for(int i = 0; i < cardList.Count; i++)
        {
            total += cardList[i].weigjt;
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Verso");
        }
    }

    public void Verso()
    {
        
        isRotation = !isRotation;
        StartCoroutine(Vanish());

        if(isRotation)
        {
            verso.SetActive(true);
        }
        else
        {
            verso.SetActive(false);
        }
    }

    private IEnumerator Vanish()
    {
        image.material.SetFloat(amount, 0);
        yield return new WaitForSeconds(0.5f);
        float time = 2;

        while (time > 0)
        {
            time -= Time.deltaTime;
            float dissolveTime = Mathf.Lerp(1f, 0, (time / 2));
            image.material.SetFloat(amount, dissolveTime);
            yield return null;
        }
    }

}
