using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBin : MonoBehaviour

{
    
    public int Score = 0;

    public AudioClip correctClip;
    public AudioClip wrongClip;

    private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the tag "Player"
        if (collision.gameObject.CompareTag("CategoryA"))
        {
            Debug.Log("Collision with Player detected!");
            SpeedPoints.TheSpeedPoints++;
            Debug.Log(SpeedPoints.TheSpeedPoints);
            PlaySound(correctClip);
            Destroy(collision.gameObject);
            // You can add your own actions here, like scoring, health reduction, etc.
        }

        else
        {
            Destroy(collision.gameObject);
            PlaySound(wrongClip);

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
