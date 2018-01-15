using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    GameObject gauge;
    GameObject percent;

    public int amountOfEnergy = 100;

    void Start ()
    {
        gauge = transform.Find("Gauge").gameObject;
        percent = transform.Find("PercentText").gameObject;
    }
	
	void Update ()
    {
        RangeLimit();

        DisplayText();
        DisplayGauge();
    }

    void DisplayText()
    {
        percent.GetComponent<Text>().text = amountOfEnergy + "%";
    }

    void DisplayGauge()
    {
        gauge.GetComponent<Image>().fillAmount = (float)amountOfEnergy / 100;
    }

    void RangeLimit()
    {
        if(amountOfEnergy > 100)
        {
            amountOfEnergy = 100;
        }
        else if(amountOfEnergy < 0)
        {
            amountOfEnergy = 0;
        }
    }
}
