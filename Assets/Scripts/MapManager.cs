using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public Animator animator;
    public GameObject map;
    public GameObject mapParent;

    private void Awake()
    {
        instance = this;
    }

    public void MapOpen()
    {
        map.SetActive(true);
    }
}
