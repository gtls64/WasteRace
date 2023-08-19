using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownAndCollision : MonoBehaviour
{
    public float countdownDuration = 40f; // Countdown duration in seconds
    private float currentTime;
    private bool countdownStarted = true; // Countdown starts immediately

    public Text countdownText; // Reference to the UI Text element

    void Start()
    {
        currentTime = countdownDuration;
        UpdateCountdownText();
    }

    void Update()
    {
        if (countdownStarted)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                // Countdown ends, transition to Scene4
                SceneManager.LoadScene("Scene4");
            }

            UpdateCountdownText();
        }
    }

    

    void UpdateCountdownText()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        countdownText.text = "Countdown: " + seconds + "s";
    }
}
