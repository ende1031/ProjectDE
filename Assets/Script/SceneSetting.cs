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
            SceneObjectManager.instance.AddObject(sceneNum, new SceneObjectManager.SceneObject("Plant", "Trap01", Grid.instance.PlayerGrid(), 3));
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SceneObjectManager.instance.AddObject(sceneNum, new SceneObjectManager.SceneObject("Plant", "MassPlant", Grid.instance.PlayerGrid(), 1));
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
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Facility", "EscapePod", 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Facility", "TempFacility", 4));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Facility", "TempFacility", 6));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Facility", "TempFacility", 7));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "StickPlant", 9, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "StickPlant", 10, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 11, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 15, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 18, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 19, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 20, 1));
        SceneObjectManager.instance.AddObject(0, new SceneObjectManager.SceneObject("Portal", "Stage02", 13, 13));

        //맵이동시 삭제된 오브젝트를 다시 불러옴.
        SceneObjectManager.instance.ReloadObject(0);
    }

    void SettingScene02()
    {
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Facility", "TempFacility", 12));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Facility", "TempFacility", 14));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "StickPlant", 16, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "StickPlant", 17, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "StickPlant", 18, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 10, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 9, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 8, 1));
        SceneObjectManager.instance.AddObject(1, new SceneObjectManager.SceneObject("Portal", "Stage01", 13, 13));

        SceneObjectManager.instance.ReloadObject(1);
    }
}
