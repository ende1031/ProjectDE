using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGauge : MonoBehaviour
{
    public int amountOfOxygen = 100;
    float angle = 55;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        RangeLimit();
        DisplayGauge();
    }

    void DisplayGauge()
    {
        angle = (float)amountOfOxygen * -0.84f + 53;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void RangeLimit()
    {
        if (amountOfOxygen > 100)
        {
            amountOfOxygen = 100;
        }
        else if (amountOfOxygen < 0)
        {
            amountOfOxygen = 0;
        }
    }
}
