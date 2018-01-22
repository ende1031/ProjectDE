using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    public static SceneObjectManager instance = null;
    GameObject UI;

    public GameObject TempFacility; //Prefab 추가시 수정할 부분
    public GameObject EscapePod;
    public GameObject StickPlant;
    public GameObject BoardPlant;
    public GameObject MassPlant;
    public GameObject Portal;

    public class SceneObject
    {
        public SceneObject(string t, string n, int g) //기본
        {
            type = t;
            name = n;
            grid = g;
        }

        public SceneObject(string t, string n, int g, int temp) //포탈용
        {
            type = t;
            name = n;
            grid = g;
            portalAfterMoveGrid = temp;
            plantState = temp;
        }

        public GameObject inGameObject;
        public string type; //"plant", "Facility" 등
        public string name; //"TempFacility", "EscapePod", "StickPlant" 등. Portal의 경우 이동하려는 씬의 이름
        public int grid;

        public int portalAfterMoveGrid;
        public int plantState;
        public float plantGrowthTimer;
    }

    static int maxSceneCount = 2; //씬 추가시 늘려줘야 됨

    List<List<SceneObject>> SObjects = new List<List<SceneObject>>();

    void InstantiateObject(SceneObject ob)
    {
        Vector3 tempPos = Grid.instance.GridToPos(ob.grid);
        tempPos.z = 0.1f;

        if (ob.type == "Plant") //Prefab 추가시 수정할 부분
        {
            switch (ob.name)
            {
                case "StickPlant":
                    ob.inGameObject = Instantiate(StickPlant, tempPos, Quaternion.identity);
                    break;
                case "MassPlant":
                    ob.inGameObject = Instantiate(MassPlant, tempPos, Quaternion.identity);
                    break;
                case "BoardPlant":
                    ob.inGameObject = Instantiate(BoardPlant, tempPos, Quaternion.identity);
                    break;
                default:
                    ob.inGameObject = Instantiate(StickPlant, tempPos, Quaternion.identity);
                    break;
            }
            ob.inGameObject.GetComponent<Plant>().state = ob.plantState;
            ob.inGameObject.GetComponent<Plant>().growthTimer = ob.plantGrowthTimer;
        }
        else if (ob.type == "Facility")
        {
            switch (ob.name)
            {
                case "TempFacility":
                    ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
                    break;
                case "EscapePod":
                    tempPos.z = 0.2f; //크기가 커서 겹쳐보일 수 있음
                    ob.inGameObject = Instantiate(EscapePod, tempPos, Quaternion.identity);
                    break;
                default:
                    ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
                    break;
            }
        }
        else if (ob.type == "Portal")
        {
            ob.inGameObject = Instantiate(Portal, tempPos, Quaternion.identity);
            ob.inGameObject.GetComponent<Portal>().sceneName = ob.name;
            ob.inGameObject.GetComponent<Portal>().AfterMoveGrid = ob.portalAfterMoveGrid;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        UI = GameObject.Find("UI");
        UI.SetActive(false);

        for (int i = 0; i < maxSceneCount; i++)
        {
            SObjects.Add(new List<SceneObject>());
        }
    }

    void Update()
    {
        for (int i = 0; i < maxSceneCount; i++)
        {
            foreach (SceneObject ob in SObjects[i])
            {
                if (ob.type == "Plant")
                {
                    if (ob.inGameObject == null)
                    {
                        ob.plantGrowthTimer += Time.deltaTime;
                    }
                }
            }
        }
    }

    //맵이동시 오브젝트의 상태를 저장함.
    public void SaveObject()
    {
        for (int i = 0; i < maxSceneCount; i++)
        {
            foreach (SceneObject ob in SObjects[i])
            {
                if (ob.type == "Plant")
                {
                    if (ob.inGameObject != null)
                    {
                        ob.plantState = ob.inGameObject.GetComponent<Plant>().state;
                        ob.plantGrowthTimer = ob.inGameObject.GetComponent<Plant>().growthTimer;
                    }
                }
            }
        }
    }

    //잠자면 식물 최대성장, 건물파괴
    public void SleepAfter()
    {
        for (int i = 0; i < maxSceneCount; i++)
        {
            foreach (SceneObject ob in SObjects[i])
            {
                if (ob.type == "Plant")
                {
                    ob.plantState = 1;
                }
            }
        }
    }

    //해당 좌표에 이미 다른 오브젝트가 있으면 false를 반환
    public bool AddObject(int sceneNum, SceneObject ob)
    {
        if(isContain(sceneNum, ob.grid).HasValue == true)
        {
            return false;
        }

        SObjects[sceneNum].Add(ob);
        InstantiateObject(ob);

        return true;
    }

    //해당 좌표에 오브젝트가 없으면 false를 반환
    public bool DeleteObject(int sceneNum, int grid)
    {
        int? temp = isContain(sceneNum, grid);
        if (temp.HasValue == false)
        {
            return false;
        }

        Destroy(SObjects[sceneNum][(int)temp].inGameObject.gameObject);
        SObjects[sceneNum].RemoveAt((int)temp);

        return true;
    }

    //해당 좌표에 오브젝트가 있으면 리스트의 몇번째 오브젝트인지 반환, 없으면 null반환
    int? isContain(int sceneNum, int grid)
    {
        if(SObjects[sceneNum].Count <= 0)
        {
            return null;
        }

        for (int i =0; i< SObjects[sceneNum].Count; i++)
        {
            if (SObjects[sceneNum][i].grid == grid)
            {
                return i;
            }
        }

        return null;
    }

    //맵이동시 삭제되는 오브젝트를 다시 불러옴
    public void ReloadObject(int sceneNum)
    {
        foreach (SceneObject ob in SObjects[sceneNum])
        {
            if (ob.inGameObject == null)
            {
                InstantiateObject(ob);
            }
        }
    }

    public void SetUIActive(bool a)
    {
        if (UI != null)
        {
            UI.SetActive(a);
        }
    }
}
