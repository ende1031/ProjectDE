using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    GameObject player;
    Monologue monologue;

    GameObject gauge;
    GameObject percent;
    GameObject gaugeLight;

    public float amountOfEnergy = 100;
    //public float reduceSpeed = 1;

    //bool monoMessage = false;

    void Start ()
    {
        gauge = transform.Find("Energy_Gauge").gameObject;
        percent = transform.Find("Energy_Percent").gameObject;
        gaugeLight = transform.Find("Energy_Light").gameObject;
    }
	
	void Update ()
    {
        if (monologue == null)
        {
            player = GameObject.Find("Player");
            if (player != null)
            {
                monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
            }
        }

        //Reduce();
        RangeLimit();
        DisplayText();
        DisplayGauge();
        GaugeLight();

        //if (monoMessage == false)
        //{
        //    if (amountOfEnergy < 20)
        //    {
        //        monologue.DisplayLog("남은 에너지가 얼마 없군.\n충전하지 않으면 위험할지도 모르겠어.");
        //        monoMessage = true;
        //    }
        //}
        //else
        //{
        //    if (amountOfEnergy > 20)
        //    {
        //        monoMessage = false;
        //    }
        //}
    }

    //void Reduce()
    //{
    //    amountOfEnergy -= reduceSpeed * Time.deltaTime;
    //}

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
        RangeLimit();
        if (amount > 0)
        {
            GaugeLightReset();
            if (amount != 100)
            {
                player.GetComponent<PlayerInteraction>().DisplayFT("에너지 +" + amount);
            }
        }
        else
        {
            player.GetComponent<PlayerInteraction>().DisplayFT("에너지 " + amount);
        }
    }

    public float GetAmount()
    {
        return amountOfEnergy;
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
