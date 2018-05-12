using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGauge : MonoBehaviour
{
    Monologue monologue;

    public float amountOfHunger = 100;
    public float reduceSpeed = 1;

    GameObject gauge;
    GameObject percent;
    GameObject gaugeLight;
    GameObject infoBG;

    bool monoMessage = false;

    float infoPos = 560.0f;
    float infoPos2 = 1.23f;

    void Start ()
    {
        gauge = transform.Find("Hunger_Guage").gameObject;
        gaugeLight = transform.Find("Hunger_Light").gameObject;
        infoBG = transform.Find("Hunger_Info").gameObject;
        percent = infoBG.transform.Find("Hunger_Percent").gameObject;
    }
	
	void Update ()
    {
        if (monologue == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
            }
        }

        Reduce();
        RangeLimit();
        DisplayText();
        DisplayGauge();
        GaugeLight();

        if (monoMessage == false)
        {
            if (amountOfHunger < 20)
            {
                monologue.DisplayLog("슬슬 배가 고프군.\n뭔가 섭취할 필요가 있겠어.");
                monoMessage = true;
            }
        }
        else
        {
            if (amountOfHunger > 20)
            {
                monoMessage = false;
            }
        }
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

    void DisplayText()
    {
        float temp = Mathf.Round(amountOfHunger);
        percent.GetComponent<Text>().text = (int)temp + "";
    }

    void DisplayGauge()
    {
        gauge.GetComponent<Image>().fillAmount = amountOfHunger / 100;
        gaugeLight.GetComponent<Image>().fillAmount = amountOfHunger / 100;

        Vector3 temp = infoBG.transform.position;
        temp.y = infoPos + amountOfHunger * infoPos2;
        infoBG.transform.position = temp;
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
