using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int time = 12;
    public float minute = 0;

    Text timerText;

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