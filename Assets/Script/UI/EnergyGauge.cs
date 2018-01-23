using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    GameObject gauge;
    GameObject percent;
    GameObject gaugeLight;

    public float amountOfEnergy = 100;
    public float reduceSpeed = 1;

    void Start ()
    {
        gauge = transform.Find("Energy_Gauge").gameObject;
        percent = transform.Find("Energy_Percent").gameObject;
        gaugeLight = transform.Find("Energy_Light").gameObject;
    }
	
	void Update ()
    {
        Reduce();
        RangeLimit();
        DisplayText();
        DisplayGauge();
        GaugeLight();
    }

    void Reduce()
    {
        amountOfEnergy -= reduceSpeed * Time.deltaTime;
    }

    void DisplayText()
    {
        float temp = Mathf.Round(amountOfEnergy);
        percent.GetComponent<Text>().text = (int)temp + "";
    }

    void DisplayGauge()
    {
        gauge.GetComponent<Image>().fillAmount = amountOfEnergy / 100;
        gaugeLight.GetComponent<Image>().fillAmount = amountOfEnergy / 100;
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

    public void SetAmount(float amount)
    {
        amountOfEnergy += amount;
        if(amount > 0)
        {
            GaugeLightReset();
        }
    }

    void GaugeLight()
    {
        Color tempColor = gaugeLight.GetComponent<Image>().color;

        if(tempColor.a > 0)
        {
            tempColor.a -= 5.0f * Time.deltaTime;
        }
        if(tempColor.a <= 0)
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
