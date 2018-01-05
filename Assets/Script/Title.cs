using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        FadeManager.instance.FadeAndLoadScene("Stage");
    }
}
