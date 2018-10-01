using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetting : MonoBehaviour
{
    //Inventory inventory;
    GameObject player;
    GameObject mainCamera;
    Monologue monologue;

    public int sceneNum;
    public float playerPosition;

    bool isSet = false;

    void Start ()
    {
        //inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
    }
	
	void Update ()
    {
		if(isSet == false)
        {
            SetPlayerPosition(Grid.instance.GridToPos(SceneChanger.instance.playerGrid));

            if (SceneObjectManager.instance.isSceneInit[sceneNum] == false)
            {
                SetScene(sceneNum);

                float left = GameObject.Find("LeftWall").transform.position.x;
                float right = GameObject.Find("RightWall").transform.position.x;
                SceneObjectManager.instance.StageWalls[sceneNum] = new Vector2(left, right);

                SceneObjectManager.instance.isSceneInit[sceneNum] = true;
            }
            SceneObjectManager.instance.ReloadObject(sceneNum);

            isSet = true;
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

    public bool GetIsSet()
    {
        return isSet;
    }

    void SetScene(int n)
    {
        switch(n)
        {
            case 0:
                SceneObjectManager.instance.AddObject(n, -1, new SceneObjectManager.SceneObject("Portal", "Stage01_B_1F", 13));
                SceneObjectManager.instance.AddObject(n, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 25));
                SceneObjectManager.instance.AddObject(n, 5, new SceneObjectManager.SceneObject("Facility", "EscapePod"));
                //SceneObjectManager.instance.AddObject(n, 6, new SceneObjectManager.SceneObject("Facility", "Grinder01"));
                //SceneObjectManager.instance.AddObject(n, 7, new SceneObjectManager.SceneObject("Facility", "TempFacility"));
                SceneObjectManager.instance.AddObject(n, 28, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                SceneObjectManager.instance.AddObject(n, -12, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, -13, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, -14, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, 16, new SceneObjectManager.SceneObject("Plant", "FruitPlant", 1));
                SceneObjectManager.instance.AddObject(n, 17, new SceneObjectManager.SceneObject("Plant", "FruitPlant", 1));
                SceneObjectManager.instance.AddObject(n, 18, new SceneObjectManager.SceneObject("Plant", "FruitPlant", 1));
                SceneObjectManager.instance.AddObject(n, 6, new SceneObjectManager.SceneObject("Plant", "MassPlant", 1));
                SceneObjectManager.instance.AddObject(n, 8, new SceneObjectManager.SceneObject("Plant", "MassPlant", 1));
                SceneObjectManager.instance.AddObject(n, 9, new SceneObjectManager.SceneObject("Plant", "MassPlant", 1));

                //inventory.GetItem(Inventory.Item.SuppliedFood, 5);
                //inventory.GetItem(Inventory.Item.SuppliedBattery, 7);
                //inventory.GetItem(Inventory.Item.Trap01, 2);
                monologue.DisplayLog("일단 주변에 쓸만한게 있는지 찾아보자.");
                break;

            case 1:
                SceneObjectManager.instance.AddObject(n, 23, new SceneObjectManager.SceneObject("Portal", "Stage01_A_2F", 4));
                SceneObjectManager.instance.AddObject(n, 25, new SceneObjectManager.SceneObject("Portal", "Stage01", 23));
                SceneObjectManager.instance.AddObject(n, 3, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                SceneObjectManager.instance.AddObject(n, 8, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                SceneObjectManager.instance.AddObject(n, 15, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, 17, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, 19, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                SceneObjectManager.instance.AddObject(n, 21, new SceneObjectManager.SceneObject("Plant", "StickPlant", 1));
                break;

            case 2:
                SceneObjectManager.instance.AddObject(n, 4, new SceneObjectManager.SceneObject("Portal", "Stage01_A_1F", 23));
                SceneObjectManager.instance.AddObject(n, 12, new SceneObjectManager.SceneObject("Nest", "Nest01"));
                SceneObjectManager.instance.AddObject(n, -1, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, 0, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                SceneObjectManager.instance.AddObject(n, 1, new SceneObjectManager.SceneObject("Plant", "ThornPlant", 1));
                break;

            case 3:
                SceneObjectManager.instance.AddObject(n, 13, new SceneObjectManager.SceneObject("Portal", "Stage01", -1));
                SceneObjectManager.instance.AddObject(n, 5, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
                SceneObjectManager.instance.AddObject(n, 7, new SceneObjectManager.SceneObject("Plant", "BoardPlant", 1));
                break;
        }
    }
}