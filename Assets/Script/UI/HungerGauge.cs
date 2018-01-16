using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGauge : MonoBehaviour
{
    public float amountOfHunger = 100;
    public float reduceSpeed = 1;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        Reduce();
        RangeLimit();
        DisplayGauge();
    }

    void Reduce()
    {
        amountOfHunger -= reduceSpeed * Time.deltaTime;
    }

    void DisplayGauge()
    {
        GetComponent<Image>().fillAmount = amountOfHunger / 100;
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
