using System.Collections;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public Mercenary mercenary;
    private ViewDetector viewDetector;

    private void Awake()
    {
        viewDetector = GetComponent<ViewDetector>();
    }

    private void OnEnable()
    {
        StartCoroutine(MagicCo());
    }

    private IEnumerator MagicCo()
    {
        viewDetector.FindAttackTarget();

        if(viewDetector.AtkTarget != null)
        {
            for (int i = 0; i < 5; i++)
            {
                viewDetector.FindRangeAttack(mercenary.atk);
                yield return new WaitForSeconds(0.6f);
            }
        }

        gameObject.SetActive(false);
    }
}
