using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    GameObject UI;
	void Start ()
    {
        UI = GameObject.Find("UI");
        UI.SetActive(false);
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
        UI.SetActive(true);
        FadeManager.instance.FadeAndLoadScene("Stage01");
    }
}
