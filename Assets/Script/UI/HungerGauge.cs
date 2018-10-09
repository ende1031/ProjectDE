using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGauge : MonoBehaviour
{
    GameObject player;
    Monologue monologue;

    public float amountOfHunger = 100;
    public float reduceSpeed = 1;

    GameObject gauge;
    GameObject percent;
    GameObject gaugeLight;
    GameObject infoBG;
    GameObject Warning;

    float infoPos = 560.0f;
    float infoPos2 = 1.23f;

    float messageTimer = 0;

    void Start ()
    {
        gauge = transform.Find("Hunger_Guage").gameObject;
        gaugeLight = transform.Find("Hunger_Light").gameObject;
        infoBG = transform.Find("Hunger_Info").gameObject;
        percent = infoBG.transform.Find("Hunger_Percent").gameObject;
        Warning = transform.Find("Hunger_Warning").gameObject;
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

        if(amountOfHunger <= 0)
        {
            GameOver();
        }

        Reduce();
        RangeLimit();
        DisplayText();
        DisplayGauge();
        GaugeLight();

        HungerMessege();
    }

    void HungerMessege()
    {
        if (amountOfHunger <= 30)
        {
            messageTimer -= Time.deltaTime;

            if(messageTimer <= 0)
            {
                monologue.DisplayLog("배가 고프군. 뭔가 먹어야겠어.");
                messageTimer = 20.0f;
            }

            Warning.SetActive(true);
        }
        else
        {
            messageTimer = 0;
            Warning.SetActive(false);
        }
    }

    public void SetAmount(float amount, bool DoNotDie = false)
    {
        if (amount > 0)
        {
            GaugeLightReset();
            player.GetComponent<PlayerInteraction>().DisplayFT("허기 +" + amount, Color.yellow);
        }
        else
        {
            if(DoNotDie == true && amountOfHunger + amount <= 5)
            {
                amountOfHunger = 5;
                return;
            }
            player.GetComponent<PlayerInteraction>().DisplayFT("허기 " + amount, Color.yellow);
        }
        amountOfHunger += amount;
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

    void GameOver()
    {
        SceneObjectManager.instance.SetUIActive(false);
        SceneChanger.instance.FadeAndLoadScene("GameOver");
    }
}
