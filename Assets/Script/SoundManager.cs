using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioClip[] seClip = new AudioClip[41];
    AudioSource Sound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Sound = this.gameObject.AddComponent<AudioSource>();
        Sound.loop = false;
    }

    public void PlaySE(int SE_Num)
    {
        Sound.clip = seClip[SE_Num];
        Sound.Play();
    }
}
