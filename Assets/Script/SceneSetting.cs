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
                case 2:
                    SettingScene03();
                    break;
                case 3:
                    SettingScene04();
                    break;
            }
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

    void SettingScene01()
    {
        //맵에 기본적으로 설치되어있는 오브젝트.
        //테스트 후엔 EscapePod, Portal등 파괴되지 않고 이동하지도 않는 오브젝트만 남기고 지울 것.
        SceneObjectManager.instance.AddObject(0, -9, new SceneObjectManager.SceneObject("Portal", "Stage01_B_1F", 25));
        SceneObjectManager.instance.AddObject(0, 5, new SceneObjectManager.SceneObject("Facility", "EscapePod"));
        SceneObjectManager.instance.AddObject(0, 7, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
        SceneObjectManager.instance.AddObject(0, 0, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(0, -12, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
        SceneObjectManager.instance.AddObject(0, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 25));
        SceneObjectManager.instance.AddObject(0, -5, new SceneObjectManager.SceneObject("Nest", "Nest01"));

        //맵이동시 삭제된 오브젝트를 다시 불러옴.
        SceneObjectManager.instance.ReloadObject(0);
    }

    void SettingScene02()
    {
        SceneObjectManager.instance.AddObject(1, 15, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(1, 7, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
        SceneObjectManager.instance.AddObject(1, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_2F", 25));
        SceneObjectManager.instance.AddObject(1, 25, new SceneObjectManager.SceneObject("Portal", "Stage01", 23));

        SceneObjectManager.instance.ReloadObject(1);
    }

    void SettingScene03()
    {
        SceneObjectManager.instance.AddObject(2, 20, new SceneObjectManager.SceneObject("Nest", "Nest01"));
        SceneObjectManager.instance.AddObject(2, 13, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(2, 25, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 23));

        SceneObjectManager.instance.ReloadObject(2);
    }

    void SettingScene04()
    {
        SceneObjectManager.instance.AddObject(3, 19, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
        SceneObjectManager.instance.AddObject(3, 11, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
        SceneObjectManager.instance.AddObject(3, 25, new SceneObjectManager.SceneObject("Portal", "Stage01", -9));

        SceneObjectManager.instance.ReloadObject(3);
    }
}