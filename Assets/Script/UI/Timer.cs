using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int TimeOfDay = 6;
    public int time;
    public float minute = 0;
    public int day = 1;

    Text timerText;
    Text dayText;

    void Start ()
    {
        time = TimeOfDay;
        timerText = GetComponent<Text>();
        dayText = GameObject.Find("DayTimer").gameObject.GetComponent<Text>();
    }
	
	void Update ()
    {
        minute -= Time.deltaTime;

        if(minute <= 0)
        {
            minute = 60;
            time--;
        }

        string t;
        string m;
        if(time >= 10)
        {
            t = time.ToString();
        }
        else
        {
            t = "0" + time;
        }

        if (minute >= 10)
        {
            m = ((int)minute).ToString();
        }
        else
        {
            m = "0" + (int)minute;
        }

        timerText.text = t + " : " + m;
        dayText.text = "Day " + day;
    }

    public void ResetTimer()
    {
        time = TimeOfDay;
        minute = 0.5f;
    }

    public void SetTimer(int t, int m)
    {
        time = t;
        minute = (float)m;
    }

    public float PercentOfTime()
    {
        float m = TimeOfDay * 60;
        float t = time * 60 + minute;

        return 100 - (t / m * 100);
    }

    public void PassDay()
    {
        day++;
    }
}