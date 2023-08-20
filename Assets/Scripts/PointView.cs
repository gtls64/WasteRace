using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointView : MonoBehaviour
{
    public Text PointsOnUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PointsOnUi.text = "Points : " + SpeedPoints.TheSpeedPoints.ToString();
    }
}
