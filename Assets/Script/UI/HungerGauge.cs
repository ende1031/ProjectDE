using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGauge : MonoBehaviour
{
    public int amountOfHunger = 100;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        DisplayGauge();
        RangeLimit();
    }

    void DisplayGauge()
    {
        GetComponent<Image>().fillAmount = (float)amountOfHunger / 100;
    }

    void RangeLimit()
    {
        if (amountOfHunger > 100)
        {
            amountOfHunger = 100;
        }
        else if (amountOfHunger < 0)
        {
            amountOfHunger = 0;
        }
    }
}
