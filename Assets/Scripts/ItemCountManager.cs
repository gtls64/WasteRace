using UnityEngine;

public class ItemCountManager : MonoBehaviour
{
    public static ItemCountManager Instance { get; private set; }

    private int itemCount = 0;
    public int ItemCount => itemCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementItemCount()
    {
        itemCount++;
    }
}
