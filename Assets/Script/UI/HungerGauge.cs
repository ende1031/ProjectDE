using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGauge : MonoBehaviour
{
    public float amountOfHunger = 100;
    public float reduceSpeed = 1;

    GameObject gaugeLight;

    void Start ()
    {
        gaugeLight = GameObject.Find("Hunger_Light").gameObject;
    }
	
	void Update ()
    {
        Reduce();
        RangeLimit();
        DisplayGauge();
        GaugeLight();
    }

    public void SetAmount(float amount)
    {
        amountOfHunger += amount;
        if (amount > 0)
        {
            GaugeLightReset();
        }
    }

    void Reduce()
    {
        amountOfHunger -= reduceSpeed * Time.deltaTime;
    }

    void DisplayGauge()
    {
        GetComponent<Image>().fillAmount = amountOfHunger / 100;
        gaugeLight.GetComponent<Image>().fillAmount = amountOfHunger / 100;
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

    void GaugeLight()
    {
        Color tempColor = gaugeLight.GetComponent<Image>().color;

        if (tempColor.a > 0)
        {
            tempColor.a -= 5.0f * Time.deltaTime;
        }
        if (tempColor.a <= 0)
        {
            tempColor.a = 0;
        }

        gaugeLight.GetComponent<Image>().color = tempColor;
    }

    void GaugeLightReset()
    {
        Color tempColor = gaugeLight.GetComponent<Image>().color;
        tempColor.a = 1.0f;
        gaugeLight.GetComponent<Image>().color = tempColor;
    }
}
