using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int time = 12;
    public float minute = 0;

    Text timerText;

    string tempZero = string.Empty;

    void Start ()
    {
        timerText = GetComponent<Text>();
    }
	
	void Update ()
    {
        minute -= Time.deltaTime;

        if(minute <= 0)
        {
            minute = 60;
            time--;
        }

        if(minute < 10)
        {
            tempZero = "0";
        }
        else
        {
            tempZero = string.Empty;
        }

        timerText.text = (int)time + " : " + tempZero + (int)minute;
    }

    public void ResetTimer()
    {
        time = 12;
        minute = 0.5f;
    }

    public void SetTimer(int t, int m)
    {
        time = t;
        minute = (float)m;
    }
}