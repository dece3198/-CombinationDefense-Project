using System.Collections;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public Mercenary mercenary;
    private ViewDetector viewDetector;
    [SerializeField] private int count = 0;
    [SerializeField] private float skillTime = 0;

    private void Awake()
    {
        viewDetector = GetComponent<ViewDetector>();
    }

    private void OnEnable()
    {
        StartCoroutine(MagicCo());
    }

    private void Update()
    {
        //viewDetector.FindAttackTarget();
    }

    private IEnumerator MagicCo()
    {
        viewDetector.FindAttackTarget();
        if (viewDetector.AtkTarget != null)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(skillTime);
                viewDetector.FindRangeAttack(mercenary.atk);
            }
        }

        gameObject.SetActive(false);
    }
}
