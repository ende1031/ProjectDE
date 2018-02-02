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
            switch(sceneNum)
            {
                case 0:
                    SettingScene01();
                    break;
                case 1:
                    SettingScene02();
                    break;
            }
        }

        //테스트용 코드
        if (Input.GetKeyUp(KeyCode.S))
        {
            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "StickPlant", 0));
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "MassPlant", 1));
        }
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

    void SettingScene01()
    {
        //맵에 기본적으로 설치되어있는 오브젝트.
        //테스트 후엔 EscapePod, Portal등 파괴되지 않고 이동하지도 않는 오브젝트만 남기고 지울 것.
        SceneObjectManager.instance.AddObject(0, 1, new SceneObjectManager.SceneObject("Facility", "EscapePod"));
        SceneObjectManager.instance.AddObject(0, 4, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(0, 6, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(0, 7, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(0, 9, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(0, 10, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(0, 11, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(0, 15, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(0, 18, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
        SceneObjectManager.instance.AddObject(0, 19, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
        SceneObjectManager.instance.AddObject(0, 20, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
        SceneObjectManager.instance.AddObject(0, 13, new SceneObjectManager.SceneObject("Portal", "Stage02", 13));
        SceneObjectManager.instance.AddObject(0, 21, new SceneObjectManager.SceneObject("Bulb", "Bulb"));
        SceneObjectManager.instance.AddObject(0, 24, new SceneObjectManager.SceneObject("Nest", "Nest01"));

        //맵이동시 삭제된 오브젝트를 다시 불러옴.
        SceneObjectManager.instance.ReloadObject(0);
    }

    void SettingScene02()
    {
        SceneObjectManager.instance.AddObject(1, 12, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(1, 14, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(1, 16, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(1, 17, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(1, 18, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(1, 10, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(1, 9, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(1, 8, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(1, 13, new SceneObjectManager.SceneObject("Portal", "Stage01", 13));
        SceneObjectManager.instance.AddObject(1, 20, new SceneObjectManager.SceneObject("Nest", "Nest01"));
        SceneObjectManager.instance.AddObject(1, 24, new SceneObjectManager.SceneObject("Nest", "Nest01"));
        SceneObjectManager.instance.AddObject(1, 28, new SceneObjectManager.SceneObject("Nest", "Nest01"));

        SceneObjectManager.instance.ReloadObject(1);
    }
}