using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
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

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedItem = other.gameObject;

        if (collidedItem == item1 || collidedItem == item2 || collidedItem == item3 ||
            collidedItem == item4 || collidedItem == item5 || collidedItem == item6 ||
            collidedItem == item7 || collidedItem == item8 || collidedItem == item9 || collidedItem == item10)
        {
            if ((collidedItem.CompareTag("CatA") && catACollider != null && transform.gameObject == catACollider) ||
                (collidedItem.CompareTag("CatB") && catBCollider != null && transform.gameObject == catBCollider) ||
                (collidedItem.CompareTag("CatC") && catCCollider != null && transform.gameObject == catCCollider))
            {
                ItemCountManager.Instance.IncrementItemCount();
                PlaySound(correctClip);
                Destroy(collidedItem);
            }
            else
            {
                PlaySound(wrongClip);
            }
        }
    }

    private void Update()
    {
        counterText.text = "Count: " + ItemCountManager.Instance.ItemCount + " / 10";
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
