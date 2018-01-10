using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetting : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;

    public float playerPosition;

    bool isSetPos = false;


    //테스트용 코드
    //public GameObject tempFacility;

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

        //테스트용 코드
        /*
        if (Input.GetKeyUp(KeyCode.A))
        {
            //print(Grid.instance.PlayerGrid());
            Vector3 tempPos = Grid.instance.GridToPos(Grid.instance.PlayerGrid());
            tempPos.z = 0.1f;
            Instantiate(tempFacility, tempPos, transform.rotation);
        }
        */
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
