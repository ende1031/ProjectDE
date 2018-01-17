using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    GameObject gauge;
    GameObject percent;

    public float amountOfEnergy = 100;
    public float reduceSpeed = 1;

    void Start ()
    {
        gauge = transform.Find("Energy_Gauge").gameObject;
        percent = transform.Find("Energy_Percent").gameObject;
    }
	
	void Update ()
    {
        Reduce();
        RangeLimit();
        DisplayText();
        DisplayGauge();
    }

    void Reduce()
    {
        amountOfEnergy -= reduceSpeed * Time.deltaTime;
    }

    void DisplayText()
    {
        percent.GetComponent<Text>().text = (int)amountOfEnergy + "%";
    }

    void DisplayGauge()
    {
        gauge.GetComponent<Image>().fillAmount = amountOfEnergy / 100;
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
