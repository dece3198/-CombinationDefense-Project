using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    [SerializeField] private Image panel;
    float time = 0;
    float F_time = 2;
    public bool isFade = false;

    private void Awake()
    {
        instance = this;
    }

    public void FadeInOut()
    {
        StartCoroutine(FadeCo());
    }

    private IEnumerator FadeCo()
    {
        isFade = false;
        Color alpha = panel.color;
        while(alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            panel.color = alpha;
            yield return null;
        }

        time = 0;

        yield return new WaitForSeconds(1f);

        while (alpha.a > 0)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            panel.color = alpha;
            yield return null;
        }

        isFade = true;
    }
}
