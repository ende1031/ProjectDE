using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetting : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;

    public int sceneNum;
    public float playerPosition;

    bool isSet = false;

    void Start ()
    {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
    }
	
	void Update ()
    {
		if(isSet == false)
        {
            isSet = true;
            SetPlayerPosition(Grid.instance.GridToPos(SceneChanger.instance.playerGrid));

            if (SceneObjectManager.instance.isSceneInit[sceneNum] == false)
            {
                SetScene(sceneNum);
                SceneObjectManager.instance.isSceneInit[sceneNum] = true;
            }

            SceneObjectManager.instance.ReloadObject(sceneNum);
        }

        //테스트용 코드
        /*
        if (Input.GetKeyUp(KeyCode.D))
        {
            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "MassPlant", 1));
        }*/
    }

    void SetPlayerPosition(Vector3 pos)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 cameraPos = mainCamera.transform.position;

        playerPos.x = pos.x;
        cameraPos.x = pos.x;

        player.transform.position = playerPos;
        mainCamera.transform.position = cameraPos;
    }

    void SetScene(int n)
    {
        switch(n)
        {
            case 0:
                SceneObjectManager.instance.AddObject(n, -9, new SceneObjectManager.SceneObject("Portal", "Stage01_B_1F", 25));
                SceneObjectManager.instance.AddObject(n, 5, new SceneObjectManager.SceneObject("Facility", "EscapePod"));
                SceneObjectManager.instance.AddObject(n, 7, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
                SceneObjectManager.instance.AddObject(n, 10, new SceneObjectManager.SceneObject("Bulb", "Bulb01"));
                SceneObjectManager.instance.AddObject(n, 0, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, -12, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 25));
                SceneObjectManager.instance.AddObject(n, -5, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                break;

            case 1:
                SceneObjectManager.instance.AddObject(n, 20, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
                SceneObjectManager.instance.AddObject(n, 7, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_2F", 25));
                SceneObjectManager.instance.AddObject(n, 25, new SceneObjectManager.SceneObject("Portal", "Stage01", 23));
                break;

            case 2:
                SceneObjectManager.instance.AddObject(n, 20, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                SceneObjectManager.instance.AddObject(n, 13, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, 25, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 23));
                break;

            case 3:
                SceneObjectManager.instance.AddObject(n, 19, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
                SceneObjectManager.instance.AddObject(n, 11, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, 25, new SceneObjectManager.SceneObject("Portal", "Stage01", -9));
                break;
        }
    }
}