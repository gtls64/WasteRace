using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBin : MonoBehaviour

{
    
    public int Score = 0;

    


    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the tag "Player"
        if (collision.gameObject.CompareTag("CategoryB"))
        {
            Debug.Log("Collision with Player detected!");
            SpeedPoints.TheSpeedPoints++;
            Debug.Log(SpeedPoints.TheSpeedPoints);
            Destroy(collision.gameObject);
            // You can add your own actions here, like scoring, health reduction, etc.
        }

        else
        {
            Destroy(collision.gameObject);
        }
    }

}
