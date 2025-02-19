using System.Collections;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public Mercenary mercenary;
    private ViewDetector viewDetector;
    [SerializeField] private int count = 0;

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
            for (int i = 0; i < count; i++)
            {
                viewDetector.FindRangeAttack(mercenary.atk);
                yield return new WaitForSeconds(0.6f);
            }
        }

        gameObject.SetActive(false);
    }
}
