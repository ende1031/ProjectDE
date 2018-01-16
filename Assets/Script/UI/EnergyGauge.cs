using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    GameObject gauge;
    GameObject percent;

    public int amountOfEnergy = 100;
    float realAmount = 100;
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
        realAmount -= reduceSpeed * Time.deltaTime;
        amountOfEnergy = (int)realAmount;
    }

    void DisplayText()
    {
        percent.GetComponent<Text>().text = amountOfEnergy + "%";
    }

    void DisplayGauge()
    {
        gauge.GetComponent<Image>().fillAmount = realAmount / 100;
    }

    void RangeLimit()
    {
        if(realAmount > 100)
        {
            realAmount = 100;
        }
        else if(realAmount < 0)
        {
            realAmount = 0;
        }
    }
}
