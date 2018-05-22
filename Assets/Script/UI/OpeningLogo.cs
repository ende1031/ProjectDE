using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningLogo : MonoBehaviour
{
    float timer = 0;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer >= 1.0f)
        {
            SceneChanger.instance.FadeAndLoadScene("OpeningMovie");
        }
	}
}
