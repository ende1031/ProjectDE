using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGauge : MonoBehaviour
{
    Monologue monologue;

    public float amountOfOxygen = 100;
    float displayedAmount = 100;
    float angle = 55;
    public float reduceSpeed = 1;
    public float speed = 50;

    bool monoMessage = false;

    void Start ()
    {
		
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
        DisplayGauge();

        //나중에 이지인 이지아웃도 만들기
        if (displayedAmount < amountOfOxygen)
        {
            displayedAmount += speed * Time.deltaTime;

            if(amountOfOxygen - displayedAmount < 0.05f)
            {
                displayedAmount = amountOfOxygen;
            }
        }
        else if(displayedAmount > amountOfOxygen)
        {
            displayedAmount -= speed * Time.deltaTime;

            if (displayedAmount - amountOfOxygen < 0.05f)
            {
                displayedAmount = amountOfOxygen;
            }
        }

        if (monoMessage == false)
        {
            if (amountOfOxygen < 20)
            {
                monologue.DisplayLog("젠장.. 산소가 부족해.");
                monoMessage = true;
            }
        }
        else
        {
            if (amountOfOxygen > 20)
            {
                monoMessage = false;
            }
        }
    }

    public void SetAmount(float amount)
    {
        amountOfOxygen += amount;
    }

    void Reduce()
    {
        amountOfOxygen -= reduceSpeed * Time.deltaTime;
    }

    void DisplayGauge()
    {
        angle = displayedAmount * -0.84f + 53;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void RangeLimit()
    {
        if (amountOfOxygen > 100)
        {
            amountOfOxygen = 100;
        }
        else if (amountOfOxygen < 0)
        {
            amountOfOxygen = 0;
        }
    }
}
