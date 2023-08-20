using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBin : MonoBehaviour

{
    
   


    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the tag "Player"
        if (collision.gameObject.CompareTag("CategoryC"))
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
