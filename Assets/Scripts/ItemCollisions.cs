using UnityEngine;
using UnityEngine.UI;

public class ItemCollisions : MonoBehaviour
{
    public GameObject[] categoryColliders;
    public Text counterText;

    private int totalItemCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < categoryColliders.Length; i++)
        {
            if (other.gameObject == categoryColliders[i] && IsItemInCategory(other.gameObject, "Category" + (char)('A' + i)))
            {
                totalItemCount++;
                UpdateCounter();
                break;
            }
        }
    }

    private bool IsItemInCategory(GameObject item, string categoryTag)
    {
        return item.CompareTag(categoryTag);
    }

    private void UpdateCounter()
    {
        counterText.text = "Counter: " + totalItemCount.ToString();

        if (totalItemCount == 10)  // Assuming 10 is the total number of items
        {
            // All items have been collected correctly
            // You can perform any additional actions here
        }
    }
}
