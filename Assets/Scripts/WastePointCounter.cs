using UnityEngine;
using UnityEngine.UI;

public class WastePointCounter : MonoBehaviour
{
    public Text counterText;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    public GameObject catACollider;
    public GameObject catBCollider;
    public GameObject catCCollider;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8;
    public GameObject item9;
    public GameObject item10;

    private int itemCount = 0;
    private const int totalItems = 10;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedItem = other.gameObject;

        if (collidedItem == item1 || collidedItem == item2 || collidedItem == item3)
        {
            if (catACollider != null && transform.gameObject == catACollider)
            {
                itemCount++;
                UpdateCounterText();
                PlaySound(correctClip);
                Destroy(collidedItem);
            }
            else
            {
                PlaySound(wrongClip);
            }
        }
        else if (collidedItem == item4 || collidedItem == item5 || collidedItem == item6)
        {
            if (catBCollider != null && transform.gameObject == catBCollider)
            {
                itemCount++;
                UpdateCounterText();
                PlaySound(correctClip);
                Destroy(collidedItem);
            }
            else
            {
                PlaySound(wrongClip);
            }
        }
        else if (collidedItem == item7 || collidedItem == item8 || collidedItem == item9 || collidedItem == item10)
        {
            if (catCCollider != null && transform.gameObject == catCCollider)
            {
                itemCount++;
                UpdateCounterText();
                PlaySound(correctClip);
                Destroy(collidedItem);
            }
            else
            {
                PlaySound(wrongClip);
            }
        }
    }

    private void UpdateCounterText()
    {
        counterText.text = "Count: " + itemCount + " / " + totalItems;

        if (itemCount == totalItems)
        {
            // Handle case when all items are collected
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
