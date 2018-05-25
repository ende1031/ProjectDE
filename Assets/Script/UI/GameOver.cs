using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    float timer = 0;
	
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer < 0.5f)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneObjectManager.instance.ResetGame();
        }
    }
}
