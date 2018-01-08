using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepFade : MonoBehaviour
{
    bool isSleep;

    GameObject Player;
    GameObject Timer;
    bool fadeOutFinish;

    void Start ()
    {
        Player = GameObject.Find("Player");
        Timer = GameObject.Find("Timer");

        isSleep = false;
        fadeOutFinish = false;
    }
	
	void Update ()
    {
        if(isSleep)
        {
            SleepAndFade();
        }
    }

    public void Sleep()
    {
        //isSleep = true;
        //Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Timer.GetComponent<Timer>().ResetTimer();
    }

    void SleepAndFade()
    {
        //Timer.GetComponent<Timer>().ResetTimer();
    }
}
