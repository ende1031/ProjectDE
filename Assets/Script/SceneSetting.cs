using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetting : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;

    public float playerPosition;

    bool isSetPos = false;

    void Start ()
    {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
    }
	
	void Update ()
    {
		if(isSetPos == false)
        {
            SetPlayerPosition(playerPosition);
            isSetPos = true;
        }
	}

    void SetPlayerPosition(float pos)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 cameraPos = mainCamera.transform.position;

        playerPos.x = pos;
        cameraPos.x = pos;

        player.transform.position = playerPos;
        mainCamera.transform.position = cameraPos;
    }
}
