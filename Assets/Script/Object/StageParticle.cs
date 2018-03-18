using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageParticle : MonoBehaviour
{
    //Timer Timer;
    ParticleSystem.EmissionModule emission;

    void Start ()
    {
        //Timer = GameObject.Find("Timer").GetComponent<Timer>();
        emission = GetComponent<ParticleSystem>().emission;
    }

    void Update ()
    {
        //emission.rateOverTime = new ParticleSystem.MinMaxCurve(Timer.PercentOfTime() / 2);
    }
}