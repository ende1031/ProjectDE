using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    public static SceneObjectManager instance = null;
    GameObject UI;

    HungerGauge hungerGauge;
    OxygenGauge oxygenGauge;
    EnergyGauge energyGauge;
    Timer timer;

    public GameObject TempFacility; //Prefab 추가시 수정할 부분
    public GameObject EscapePod;
    public GameObject StickPlant;
    public GameObject BoardPlant;
    public GameObject MassPlant;
    public GameObject ThornPlant;
    public GameObject Portal;
    public GameObject Trap01;
    public GameObject Bulb01;
    public GameObject Nest01;
    public GameObject Grinder01;

    public class SceneObject
    {
        //public SceneObject(string t, string n) //기본
        //{
        //    type = t;
        //    name = n;
        //    //isOn = true;
        //    //isAlive = true;
        //    state = 1;
        //}

        public SceneObject(string t, string n, int temp = 1) //temp : 이동후좌표(포탈), 상태(식물, 시설)
        {
            type = t;
            name = n;
            portalAfterMoveGrid = temp;
            state = temp;
            //isOn = true;
            //isAlive = true;
        }

        public GameObject inGameObject;
        public string type; //"plant", "Facility" 등
        public string name; //"TempFacility", "EscapePod", "StickPlant" 등. Portal의 경우 이동하려는 씬의 이름

        //public bool isOn;
        //public bool isAlive;
        public float timer; //growthTimer(괴식물), progressTimer(시설), LifeTimer(전구)
        public int portalAfterMoveGrid;
        public int state;
        public Inventory.Item facilityMakeItem;
        public Inventory.Item[] facilityGrinderItem = new Inventory.Item[3];
        public int[] facilityGrinderItemNum = new int[3];
        public float facilityTimeToMake;
        //public bool facilityIsMake;
        //public bool facilityIsMakeFinish;
        public bool isMakeByGrinder;
        //public float bulbLifeTime;
    }

    static int maxSceneNum = 4; //씬 추가시 늘려줘야 됨

    List<Dictionary<int, SceneObject>> SObjects = new List<Dictionary<int, SceneObject>>();
    public List<bool> isSceneInit = new List<bool>();
    public List<Vector2> StageWalls = new List<Vector2>();

    void InstantiateObject(int grid, SceneObject ob)
    {
        Vector3 tempPos = Grid.instance.GridToPos(grid);
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
                case "ThornPlant":
                    ob.inGameObject = Instantiate(ThornPlant, tempPos, Quaternion.identity);
                    break;
                case "Trap01":
                    ob.inGameObject = Instantiate(Trap01, tempPos, Quaternion.identity);
                    break;
                default:
                    ob.inGameObject = Instantiate(StickPlant, tempPos, Quaternion.identity);
                    break;
            }
            ob.inGameObject.GetComponent<Plant>().state = ob.state;
            ob.inGameObject.GetComponent<Plant>().growthTimer = ob.timer;
            ob.inGameObject.GetComponent<Plant>().isLoadByManager = true;
        }
        else if (ob.type == "Facility")
        {
            switch (ob.name)
            {
                case "TempFacility":
                    ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
                    break;
                case "Grinder01":
                    ob.inGameObject = Instantiate(Grinder01, tempPos, Quaternion.identity);
                    break;
                case "EscapePod":
                    tempPos.z = 0.2f; //크기가 커서 겹쳐보일 수 있음
                    ob.inGameObject = Instantiate(EscapePod, tempPos, Quaternion.identity);
                    break;
                default:
                    ob.inGameObject = Instantiate(TempFacility, tempPos, Quaternion.identity);
                    break;
            }
            ob.inGameObject.GetComponent<Facility>().state = ob.state;
            //ob.inGameObject.GetComponent<Facility>().isOn = ob.isOn;
            ob.inGameObject.GetComponent<FacilityBalloon>().makeItem = ob.facilityMakeItem;
            ob.inGameObject.GetComponent<FacilityBalloon>().progressTimer = ob.timer;
            ob.inGameObject.GetComponent<FacilityBalloon>().timeToMake = ob.facilityTimeToMake;
            //ob.inGameObject.GetComponent<FacilityBalloon>().isMake = ob.facilityIsMake;
            //ob.inGameObject.GetComponent<FacilityBalloon>().isMakeFinish = ob.facilityIsMakeFinish;
            ob.inGameObject.GetComponent<FacilityBalloon>().isMakeByGrinder = ob.isMakeByGrinder;
            ob.inGameObject.GetComponent<FacilityBalloon>().grinderItem = ob.facilityGrinderItem;
            ob.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum = ob.facilityGrinderItemNum;
            //ob.inGameObject.GetComponent<Facility>().isAlive = ob.isAlive;
            ob.inGameObject.GetComponent<Facility>().isLoadByManager = true;
            ob.inGameObject.GetComponent<FacilityBalloon>().isLoadByManager = true;
        }
        else if (ob.type == "Portal")
        {
            ob.inGameObject = Instantiate(Portal, tempPos, Quaternion.identity);
            ob.inGameObject.GetComponent<Portal>().sceneName = ob.name;
            ob.inGameObject.GetComponent<Portal>().AfterMoveGrid = ob.portalAfterMoveGrid;
        }
        else if (ob.type == "Bulb")
        {
            ob.inGameObject = Instantiate(Bulb01, tempPos, Quaternion.identity);
            //ob.inGameObject.GetComponent<Bulb>().isOn = ob.isOn;
            //ob.inGameObject.GetComponent<Bulb>().LifeTimer = ob.timer;
            //ob.inGameObject.GetComponent<Bulb>().isAlive = ob.isAlive;
            //ob.inGameObject.GetComponent<Bulb>().isLoadByManager = true;
        }
        else if (ob.type == "Nest")
        {
            ob.inGameObject = Instantiate(Nest01, tempPos, Quaternion.identity);
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
        oxygenGauge = GameObject.Find("Oxygen_Needle").GetComponent<OxygenGauge>();
        hungerGauge = GameObject.Find("Hunger_Guage").GetComponent<HungerGauge>();
        energyGauge = GameObject.Find("LeftUI").GetComponent<EnergyGauge>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        UI.SetActive(false);

        for (int i = 0; i < maxSceneNum; i++)
        {
            SObjects.Add(new Dictionary<int, SceneObject>());
            isSceneInit.Add(false);
            StageWalls.Add(Vector2.zero);
        }
    }

    // 다른 맵에 있을때도 오브젝트의 타이머가 흘러가도록
    void Update()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, SceneObject> pair in SObjects[i])
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
                //else if (pair.Value.type == "Bulb")
                //{
                //    if (pair.Value.inGameObject == null && pair.Value.isOn == true)
                //    {
                //        pair.Value.timer += Time.deltaTime;
                //        if(pair.Value.timer > pair.Value.bulbLifeTime)
                //        {
                //            pair.Value.isAlive = false;
                //            pair.Value.isOn = false;
                //        }
                //    }
                //}
            }
        }
    }

    //맵이동시 오브젝트의 상태를 저장함.
    public void SaveObject()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, SceneObject> pair in SObjects[i])
            {
                if (pair.Value.type == "Plant")
                {
                    if (pair.Value.inGameObject != null)
                    {
                        pair.Value.state = pair.Value.inGameObject.GetComponent<Plant>().state;
                        pair.Value.timer = pair.Value.inGameObject.GetComponent<Plant>().growthTimer;
                    }
                }
                else if (pair.Value.type == "Facility")
                {
                    if (pair.Value.inGameObject != null)
                    {
                        pair.Value.state = pair.Value.inGameObject.GetComponent<Facility>().state;
                        //pair.Value.isOn = pair.Value.inGameObject.GetComponent<Facility>().isOn;
                        pair.Value.timer = pair.Value.inGameObject.GetComponent<FacilityBalloon>().progressTimer;
                        pair.Value.facilityTimeToMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().timeToMake;
                        pair.Value.facilityMakeItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().makeItem;
                        //pair.Value.facilityIsMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMake;
                        //pair.Value.facilityIsMakeFinish = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMakeFinish;
                        //pair.Value.isAlive = pair.Value.inGameObject.GetComponent<Facility>().isAlive;
                        pair.Value.isMakeByGrinder = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMakeByGrinder;
                        pair.Value.facilityGrinderItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItem;
                        pair.Value.facilityGrinderItemNum = pair.Value.inGameObject.GetComponent<FacilityBalloon>().grinderItemNum;
                    }
                }
                //else if (pair.Value.type == "Bulb")
                //{
                //    if (pair.Value.inGameObject != null)
                //    {
                //        pair.Value.isOn = pair.Value.inGameObject.GetComponent<Bulb>().isOn;
                //        pair.Value.timer = pair.Value.inGameObject.GetComponent<Bulb>().LifeTimer;
                //        pair.Value.isAlive = pair.Value.inGameObject.GetComponent<Bulb>().isAlive;
                //        pair.Value.bulbLifeTime = pair.Value.inGameObject.GetComponent<Bulb>().LifeTime;
                //    }
                //}
            }
        }
    }

    //잠자면 식물 최대성장, 건물파괴
    public void SleepAfter()
    {
        for (int i = 0; i < maxSceneNum; i++)
        {
            foreach (KeyValuePair<int, SceneObject> pair in SObjects[i])
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
                    //pair.Value.facilityIsMake = false;
                    //pair.Value.facilityIsMakeFinish = false;
                }
                //else if (pair.Value.type == "Bulb")
                //{
                //    if (pair.Value.isOn == true)
                //    {
                //        pair.Value.isAlive = false;
                //    }
                //}
            }
        }

        hungerGauge.SetAmount(-20);
        oxygenGauge.SetAmount(-10);
        energyGauge.SetAmount(-10);
        timer.ResetTimer();
        timer.PassDay();

        for (int i = 0; i < 3; i++)
        {
            RandomSpawn(new SceneObject("Plant", "MassPlant", 1));
        }
    }

    // 방문한 적 있는 맵 중에서 랜덤으로 오브젝트 생성. 씬이동시에만 사용할 것.
    void RandomSpawn(SceneObject ob)
    {
        bool[] isSceneHasBlank = new bool[maxSceneNum];
        for (int i = 0; i < maxSceneNum; i++)
        {
            isSceneHasBlank[i] = true;
        }

        while (true)
        {
            //모든 씬이 다 꽉차있는지 확인
            bool isAllSceneNoBlank = true;
            for(int i =0; i< maxSceneNum; i++)
            {
                if(isSceneHasBlank[i] == true && isSceneInit[i] == true)
                {
                    isAllSceneNoBlank = false;
                }
            }
            if(isAllSceneNoBlank == true)
            {
                print("오류코드 : 돌솥비빔밥"); // 방문한 적 있는 모든 스테이지에 빈칸 없음
                break;
            }

            //랜덤으로 씬 선택. 꽉차있거나 가본적이 없으면 continue
            int randomScene = Random.Range(0, maxSceneNum);
            if (isSceneInit[randomScene] == false || isSceneHasBlank[randomScene] == false)
            {
                continue;
            }

            //벽 정보 받아옴
            int left = Grid.instance.PosToGrid(StageWalls[randomScene].x);
            int right = Grid.instance.PosToGrid(StageWalls[randomScene].y);

            //현재 씬에 빈칸이 있는지 확인
            bool isblank = false;
            for (int i = left + 1; i < right; i++)
            {
                if (SObjects[randomScene].ContainsKey(i) == false)
                {
                    isblank = true;
                }
            }
            if (isblank == false)
            {
                isSceneHasBlank[randomScene] = false;
                continue;
            }

            //랜덤으로 좌표 설정
            int randomGrid = Random.Range(left + 1, right - 1);
            while (SObjects[randomScene].ContainsKey(randomGrid) == true)
            {
                randomGrid = Random.Range(left + 1, right - 1);
            }

            //오브젝트 딕셔너리에 추가
            SObjects[randomScene].Add(randomGrid, ob);
            print("덩어리 랜덤 스폰 : " + randomScene + "씬, " + randomGrid + "블록");
            break;
        }
    }

    //해당 좌표에 이미 다른 오브젝트가 있으면 false를 반환
    public bool AddObject(int sceneNum, int grid, SceneObject ob)
    {
        if (SObjects[sceneNum].ContainsKey(grid) == true)
        {
            return false;
        }
        
        SObjects[sceneNum].Add(grid, ob);
        InstantiateObject(grid, ob);
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
    public void ChangeObject(int sceneNum, int grid, SceneObject ob)
    {
        DeleteObject(sceneNum, grid);
        AddObject(sceneNum, grid, ob);
    }

    //맵이동시 삭제되는 오브젝트를 다시 불러옴
    public void ReloadObject(int sceneNum)
    {
        foreach (KeyValuePair<int, SceneObject> pair in SObjects[sceneNum])
        {
            if (pair.Value.inGameObject == null)
            {
                InstantiateObject(pair.Key, pair.Value);
            }
        }
    }

    //grid를 기준으로 양옆 range범위 안에 해당 type의 오브젝트가 있으면 true, isSearchLight는 전구가 아니라 빛(켜진 전구)을 탐색함
    public bool RangeSearch(int sceneNum, int grid, int range, string type, string name = "NoName")
    {
        for (int i = (grid - range); i <= (grid + range); i++)
        {
            if (SObjects[sceneNum].ContainsKey(i) == true)
            {
                if (SObjects[sceneNum][i].type == type && name == "NoName")
                {
                    //if (type == "Bulb" && isSearchLight == true)
                    //{
                    //    if (SObjects[sceneNum][i].isOn == true && SObjects[sceneNum][i].isAlive == true)
                    //    {
                    //        return true;
                    //    }
                    //}
                    //else
                    //{
                        return true;
                    //}
                }
                else if (SObjects[sceneNum][i].type == type && SObjects[sceneNum][i].name == name)
                {
                    //if(type == "Bulb" && isSearchLight == true)
                    //{
                    //    if(SObjects[sceneNum][i].isOn == true && SObjects[sceneNum][i].isAlive == true)
                    //    {
                    //        return true;
                    //    }
                    //}
                    //else
                    //{
                        return true;
                    //}
                }
            }
        }
        return false;
    }

    public void SetUIActive(bool a)
    {
        if (UI != null)
        {
            UI.SetActive(a);
        }
    }
}
