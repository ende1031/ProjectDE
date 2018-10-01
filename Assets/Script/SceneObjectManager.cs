using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectManager : MonoBehaviour
{
    public static SceneObjectManager instance = null;

    public static int maxSceneNum = 4; //게임 플레이에 들어가는 씬의 수

    List<Dictionary<int, ObjectInfo>> SObjects = new List<Dictionary<int, ObjectInfo>>();
    public List<bool> isSceneInit = new List<bool>();
    public List<Vector2> StageWalls = new List<Vector2>();
    //Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    public bool isNewGame = false;

    GameObject UI;
    HungerGauge hungerGauge;
    EnergyGauge energyGauge;
    //Inventory inventory;
    NyxUI nyxUI;
    /*
    public GameObject TempFacility; //Prefab
    public GameObject EscapePod;
    public GameObject StickPlant;
    public GameObject BoardPlant;
    public GameObject MassPlant;
    public GameObject ThornPlant;
    public GameObject FruitPlant;
    public GameObject Portal;
    public GameObject Trap01;
    public GameObject Bulb01;
    public GameObject Nest01;
    public GameObject Grinder01;
    public GameObject NyxCollector01;
    
    void SetObjectDictionary()
    {
        objectDictionary["TempFacility"] = TempFacility;
        objectDictionary["EscapePod"] = EscapePod;
        objectDictionary["StickPlant"] = StickPlant;
        objectDictionary["BoardPlant"] = BoardPlant;
        objectDictionary["MassPlant"] = MassPlant;
        objectDictionary["ThornPlant"] = ThornPlant;
        objectDictionary["FruitPlant"] = FruitPlant;
        objectDictionary["Portal"] = Portal;
        objectDictionary["Trap01"] = Trap01;
        objectDictionary["Bulb01"] = Bulb01;
        objectDictionary["Nest01"] = Nest01;
        objectDictionary["Grinder01"] = Grinder01;
        objectDictionary["NyxCollector01"] = NyxCollector01;
    }
    */
    public class ObjectInfo
    {
        public ObjectInfo(string t, string n, int st = 1, float ti = 0)
        {
            type = t;
            name = n;
            state = st;
            timer = ti;
        }

        public GameObject inGameObject;
        public string type;
        public string name; //Portal의 경우 이동하려는 씬의 이름
        public float timer;
        public int state;

        public Inventory.Item facilityMakeItem;
        public Inventory.Item[] facilityGrinderItem = new Inventory.Item[2];
        public int[] facilityGrinderItemNum = new int[2];
        public float facilityTimeToMake;
    }

    void InstantiateObject(int grid, ObjectInfo ob)
    {
        Vector3 tempPos = Grid.instance.GridToPos(grid);
        tempPos.z = 0.1f;

        //=========================================================

        if (ob.type == "Portal")
        {
            ob.inGameObject = Instantiate(Resources.Load("Prefab/Portal") as GameObject, tempPos, Quaternion.identity);
        }
        else
        {
            ob.inGameObject = Instantiate(Resources.Load("Prefab/" + ob.name) as GameObject, tempPos, Quaternion.identity);
        }

        if(ob.name == "EscapePod")
        {
            tempPos.z = 0.2f; //크기가 커서 겹쳐보일 수 있음
        }
        ob.inGameObject.GetComponent<SceneObject>().TargetSceneName = ob.name;
        ob.inGameObject.GetComponent<SceneObject>().state = ob.state;
        ob.inGameObject.GetComponent<SceneObject>().objectTimer = ob.timer;

        if (ob.type == "Facility")
        {
            ob.inGameObject.GetComponent<FacilityBalloon>().makeItem = ob.facilityMakeItem;
            ob.inGameObject.GetComponent<FacilityBalloon>().timeToMake = ob.facilityTimeToMake;
            ob.inGameObject.GetComponent<FacilityBalloon>().grinderItem = ob.facilityGrinderItem;
            ob.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum = ob.facilityGrinderItemNum;
        }
        
        ob.inGameObject.GetComponent<SceneObject>().Init();

        //=========================================================
        
        //if (ob.type == "Plant") //Prefab 추가시 수정할 부분
        //{
        //    switch (ob.name)
        //    {
        //        case "StickPlant":
        //            ob.inGameObject = Instantiate(StickPlant, tempPos, Quaternion.identity);
        //            break;
        //        case "MassPlant":
        //            ob.inGameObject = Instantiate(MassPlant, tempPos, Quaternion.identity);
        //            break;
        //        case "BoardPlant":
        //            ob.inGameObject = Instantiate(BoardPlant, tempPos, Quaternion.identity);
        //            break;
        //        case "ThornPlant":
        //            ob.inGameObject = Instantiate(ThornPlant, tempPos, Quaternion.identity);
        //            break;
        //        case "Trap01":
        //            ob.inGameObject = Instantiate(Trap01, tempPos, Quaternion.identity);
        //            break;
        //        case "FruitPlant":
        //            ob.inGameObject = Instantiate(FruitPlant, tempPos, Quaternion.identity);
        //            break;
        //        default:
        //            ob.inGameObject = Instantiate(StickPlant, tempPos, Quaternion.identity);
        //            break;
        //    }
        //    ob.inGameObject.GetComponent<Plant>().state = ob.state;
        //    ob.inGameObject.GetComponent<Plant>().objectTimer = ob.timer;
        //    ob.inGameObject.GetComponent<Plant>().isLoadByManager = true;
        //}
        //else if (ob.type == "Facility")
        //{
        //    switch (ob.name)
        //    {
        //        case "TempFacility":
        //            ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
        //            break;
        //        case "Grinder01":
        //            ob.inGameObject = Instantiate(Grinder01, tempPos, Quaternion.identity);
        //            break;
        //        case "EscapePod":
        //            tempPos.z = 0.2f; //크기가 커서 겹쳐보일 수 있음
        //            ob.inGameObject = Instantiate(EscapePod, tempPos, Quaternion.identity);
        //            break;
        //        default:
        //            ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
        //            break;
        //    }
        //    ob.inGameObject.GetComponent<Facility>().state = ob.state;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().makeItem = ob.facilityMakeItem;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().progressTimer = ob.timer;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().timeToMake = ob.facilityTimeToMake;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().isMakeByGrinder = ob.isMakeByGrinder;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().grinderItem = ob.facilityGrinderItem;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum = ob.facilityGrinderItemNum;
        //    ob.inGameObject.GetComponent<Facility>().isLoadByManager = true;
        //    ob.inGameObject.GetComponent<FacilityBalloon>().isLoadByManager = true;
        //}
        //else if (ob.type == "Portal")
        //{
        //    ob.inGameObject = Instantiate(Portal, tempPos, Quaternion.identity);
        //    ob.inGameObject.GetComponent<Portal>().ObjectName = ob.name;
        //    ob.inGameObject.GetComponent<Portal>().state = ob.state;
        //}
        //else if (ob.type == "Bulb")
        //{
        //    ob.inGameObject = Instantiate(Bulb01, tempPos, Quaternion.identity);
        //}
        //else if (ob.type == "Nest")
        //{
        //    ob.inGameObject = Instantiate(Nest01, tempPos, Quaternion.identity);
        //}
        //else if (ob.type == "NyxCollector")
        //{
        //    ob.inGameObject = Instantiate(NyxCollector01, tempPos, Quaternion.identity);
        //    ob.inGameObject.GetComponent<NyxCollector>().state = ob.state;
        //    ob.inGameObject.GetComponent<NyxCollector>().objectTimer = ob.timer;
        //    ob.inGameObject.GetComponent<NyxCollector>().isLoadByManager = true;
        //}
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

        //DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        UI = GameObject.Find("UI");
        hungerGauge = GameObject.Find("HungerUI").GetComponent<HungerGauge>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();
        //inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        UI.SetActive(false);

        for (int i = 0; i < maxSceneNum; i++)
        {
            SObjects.Add(new Dictionary<int, ObjectInfo>());
            isSceneInit.Add(false);
            StageWalls.Add(Vector2.zero);
        }

        //SetObjectDictionary();
    }

    //게임 초기화
    public void ResetGame()
    {
        SceneManager.LoadScene("Persistent");
    }

    // 다른 맵에 있을때도 오브젝트의 타이머가 흘러가도록
    void Update()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[i])
            {
                if (pair.Value.type == "Plant")
                {
                    if (pair.Value.inGameObject == null)
                    {
                        pair.Value.timer += Time.deltaTime;

                        if (pair.Value.name == "Trap01")
                        {
                            if (RangeSearch(i, pair.Key, 2, "Bulb", "Bulb01") == false)
                            {
                                if (RangeSearch(i, pair.Key, 2, "Facility", "EscapePod") == false)
                                {
                                    if (RangeSearch(i, pair.Key, 2, "Nest", "Nest01") == true)
                                    {
                                        pair.Value.timer += Time.deltaTime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            pair.Value.timer += Time.deltaTime;
                        }
                    }
                }
                else if (pair.Value.type == "Facility")
                {
                    if (pair.Value.inGameObject == null && pair.Value.state == 2 && pair.Value.timer > 0)
                    {
                        pair.Value.timer -= Time.deltaTime;
                    }
                }
                else if (pair.Value.type == "NyxCollector")
                {
                    if (pair.Value.inGameObject == null && pair.Value.state == 1)
                    {
                        pair.Value.timer += Time.deltaTime;
                        if (pair.Value.timer >= 1.0f)
                        {
                            nyxUI.SetAmount(1);
                            pair.Value.timer = 0;
                        }
                    }
                }
            }
        }
    }

    //맵이동시 오브젝트의 상태를 저장함.
    public void SaveObject()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[i])
            {
                if (pair.Value.inGameObject != null)
                {
                    //============================================

                    pair.Value.state = pair.Value.inGameObject.GetComponent<SceneObject>().state;
                    pair.Value.timer = pair.Value.inGameObject.GetComponent<SceneObject>().objectTimer;

                    if (pair.Value.type == "Facility")
                    {
                        pair.Value.facilityTimeToMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().timeToMake;
                        pair.Value.facilityMakeItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().makeItem;
                        pair.Value.facilityGrinderItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItem;
                        pair.Value.facilityGrinderItemNum = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum;
                    }

                    //============================================

                    //if (pair.Value.type == "Plant")
                    //{
                    //    pair.Value.state = pair.Value.inGameObject.GetComponent<Plant>().state;
                    //    pair.Value.timer = pair.Value.inGameObject.GetComponent<Plant>().objectTimer;
                    //}
                    //else if (pair.Value.type == "Facility")
                    //{
                    //    pair.Value.state = pair.Value.inGameObject.GetComponent<Facility>().state;
                    //    pair.Value.timer = pair.Value.inGameObject.GetComponent<Facility>().objectTimer;
                    //    pair.Value.facilityTimeToMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().timeToMake;
                    //    pair.Value.facilityMakeItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().makeItem;
                    //    pair.Value.isMakeByGrinder = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMakeByGrinder;
                    //    pair.Value.facilityGrinderItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItem;
                    //    pair.Value.facilityGrinderItemNum = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum;
                    //}
                    //else if (pair.Value.type == "NyxCollector")
                    //{
                    //    pair.Value.state = pair.Value.inGameObject.GetComponent<NyxCollector>().state;
                    //    pair.Value.timer = pair.Value.inGameObject.GetComponent<NyxCollector>().objectTimer;
                    //}
                }
            }
        }
    }

    //잠자면 식물 최대성장, 건물파괴
    public void SleepAfter()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[i])
            {
                if (pair.Value.type == "Plant")
                {
                    if (pair.Value.state != 4 && pair.Value.state != 5)
                    {
                        pair.Value.state = 1;
                    }
                    else
                    {
                        pair.Value.state = 5;
                    }
                }
                else if (pair.Value.type == "Facility")
                {
                    if (pair.Value.name != "EscapePod")
                    {
                        if(pair.Value.state != 0)
                        {
                            pair.Value.state = 4;
                        }
                    }
                    else
                    {
                        pair.Value.state = 1;
                    }
                }
                else if (pair.Value.type == "NyxCollector")
                {
                    if (pair.Value.state != 0)
                    {
                        pair.Value.state = 4;
                    }
                }
            }
        }

        hungerGauge.SetAmount(-10, true);
        energyGauge.SetAmount(100);

        RandomSpawn(new ObjectInfo("Plant", "MassPlant", 1));
        RandomSpawn(new ObjectInfo("Plant", "MassPlant", 1));
        RandomSpawn(new ObjectInfo("Plant", "MassPlant", 1));
    }

    // 방문한 적 있는 맵 중에서 랜덤으로 오브젝트 생성. 씬이동시에만 사용할 것.
    void RandomSpawn(ObjectInfo ob)
    {
        //배열 초기화
        bool[] isSceneHasBlank = new bool[maxSceneNum];
        for (int i = 0; i < maxSceneNum; i++)
        {
            isSceneHasBlank[i] = true;
        }

        int blankCount = 0;

        //빈칸 수 확인
        for (int scene = 0; scene < maxSceneNum; scene++)
        {
            //안가본 씬은 스킵
            if (isSceneInit[scene] == false)
            {
                isSceneHasBlank[scene] = false;
                continue;
            }
            //벽 정보 받아옴
            int m_left = Grid.instance.PosToGrid(StageWalls[scene].x);
            int m_right = Grid.instance.PosToGrid(StageWalls[scene].y);

            int count = 0;
            //빈칸 수 카운트
            for (int i = m_left + 1; i < m_right; i++)
            {
                if (SObjects[scene].ContainsKey(i) == false)
                {
                    blankCount++;
                    count++;
                }
            }
            if (count == 0)
            {
                isSceneHasBlank[scene] = false;
            }
        }
        
        if(blankCount <= 3) //빈칸이 3칸보다 많을때만 덩어리 스폰 실행
        {
            return;
        }

        //랜덤으로 씬 선택
        int randomScene = Random.Range(0, maxSceneNum);
        int temp = 0;
        while (isSceneHasBlank[randomScene] == false)
        {
            randomScene = Random.Range(0, maxSceneNum);
            temp++;
            if(temp > 10)
            {
                return;
            }
        }

        //벽 정보 받아옴
        int left = Grid.instance.PosToGrid(StageWalls[randomScene].x);
        int right = Grid.instance.PosToGrid(StageWalls[randomScene].y);

        //랜덤으로 좌표 설정
        int randomGrid = Random.Range(left + 1, right - 1);
        temp = 0;
        while (SObjects[randomScene].ContainsKey(randomGrid) == true)
        {
            randomGrid = Random.Range(left + 1, right - 1);
            temp++;
            if (temp > 20)
            {
                return;
            }
        }

        //오브젝트 딕셔너리에 추가
        SObjects[randomScene].Add(randomGrid, ob);
        print("덩어리 랜덤 스폰 : " + randomScene + "씬, " + randomGrid + "블록");
    }

    //해당 좌표에 이미 다른 오브젝트가 있으면 false를 반환
    public bool AddObject(int sceneNum, int grid, ObjectInfo ob)
    {
        if (SObjects[sceneNum].ContainsKey(grid) == true)
        {
            return false;
        }
        
        SObjects[sceneNum].Add(grid, ob);

        int thisSceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        if(thisSceneNum == sceneNum)
        {
            InstantiateObject(grid, ob);
        }
        return true;
    }

    //해당 좌표에 오브젝트가 없으면 false를 반환
    public bool DeleteObject(int sceneNum, int grid)
    {
        if (SObjects[sceneNum].ContainsKey(grid) == false)
        {
            return false;
        }

        Destroy(SObjects[sceneNum][grid].inGameObject.gameObject);
        SObjects[sceneNum].Remove(grid);

        return true;
    }

    //해당 좌표에 오브젝트가 있으면 true
    public bool ContainObject(int sceneNum, int grid)
    {
        return SObjects[sceneNum].ContainsKey(grid);
    }

    //시설을 지우고 잔해를 넣는 등
    public void ChangeObject(int sceneNum, int grid, ObjectInfo ob)
    {
        DeleteObject(sceneNum, grid);
        AddObject(sceneNum, grid, ob);
    }

    //맵이동시 삭제되는 오브젝트를 다시 불러옴
    public void ReloadObject(int sceneNum)
    {
        foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[sceneNum])
        {
            if (pair.Value.inGameObject == null)
            {
                InstantiateObject(pair.Key, pair.Value);
            }
        }
    }

    //grid를 기준으로 양옆 range범위 안에 해당 type의 오브젝트가 있으면 true
    public bool RangeSearch(int sceneNum, int grid, int range, string type, string name = "NoName")
    {
        for (int i = (grid - range); i <= (grid + range); i++)
        {
            if (SObjects[sceneNum].ContainsKey(i) == true)
            {
                if (SObjects[sceneNum][i].type == type && name == "NoName")
                {
                    return true;
                }
                else if (SObjects[sceneNum][i].type == type && SObjects[sceneNum][i].name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveAllObject()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[i])
            {
                if (pair.Value.inGameObject != null)
                {
                    Destroy(pair.Value.inGameObject);
                }
            }
        }
        SObjects.Clear();

        for (int i = 0; i < maxSceneNum; i++)
        {
            SObjects.Add(new Dictionary<int, ObjectInfo>());
        }
    }

    public void SetUIActive(bool a)
    {
        if (UI != null)
        {
            UI.SetActive(a);
        }
    }

    public List<SceneObject_Save> GetSceneObjectList()
    {
        List<SceneObject_Save> temp = new List<SceneObject_Save>();

        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, ObjectInfo> pair in SObjects[i])
            {
                temp.Add(new SceneObject_Save(i, pair.Key, pair.Value.type, pair.Value.name, pair.Value.state, pair.Value.timer));
            }
        }

        return temp;
    }

    public void LoadSceneObject(List<SceneObject_Save> list)
    {
        RemoveAllObject();

        for (int i = 0; i < list.Count; i++)
        {
            AddObject(list[i].sceneNum, list[i].key, new ObjectInfo(list[i].type, list[i].name, list[i].state, list[i].timer));
        }

        ReloadObject(0);
    }
}
