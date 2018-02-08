﻿using System.Collections;
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
    public GameObject Wreckage01;

    public class SceneObject
    {
        public SceneObject(string t, string n) //기본
        {
            type = t;
            name = n;
            isOn = true;
            isAlive = true;
        }

        public SceneObject(string t, string n, int temp) //temp : 이동후좌표(포탈), 상태(식물)
        {
            type = t;
            name = n;
            portalAfterMoveGrid = temp;
            plantState = temp;
            isOn = true;
            isAlive = true;
        }

        public GameObject inGameObject;
        public string type; //"plant", "Facility" 등
        public string name; //"TempFacility", "EscapePod", "StickPlant" 등. Portal의 경우 이동하려는 씬의 이름

        public bool isOn;
        public bool isAlive;
        public float timer; //growthTimer(괴식물), progressTimer(시설), LifeTimer(전구)
        public int portalAfterMoveGrid;
        public int plantState;
        public Inventory.Item facilityMakeItem;
        public float facilityTimeToMake;
        public bool facilityIsMake;
        public bool facilityIsMakeFinish;
        public float bulbLifeTime;
    }

    static int maxSceneNum = 4; //씬 추가시 늘려줘야 됨

    List<Dictionary<int, SceneObject>> SObjects = new List<Dictionary<int, SceneObject>>();
    public List<bool> isSceneInit = new List<bool>();

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
            ob.inGameObject.GetComponent<Plant>().state = ob.plantState;
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
            ob.inGameObject.GetComponent<Facility>().isOn = ob.isOn;
            ob.inGameObject.GetComponent<FacilityBalloon>().makeItem = ob.facilityMakeItem;
            ob.inGameObject.GetComponent<FacilityBalloon>().progressTimer = ob.timer;
            ob.inGameObject.GetComponent<FacilityBalloon>().timeToMake = ob.facilityTimeToMake;
            ob.inGameObject.GetComponent<FacilityBalloon>().isMake = ob.facilityIsMake;
            ob.inGameObject.GetComponent<FacilityBalloon>().isMakeFinish = ob.facilityIsMakeFinish;
            ob.inGameObject.GetComponent<FacilityBalloon>().isLoadByManager = true;
            ob.inGameObject.GetComponent<Facility>().isAlive = ob.isAlive;
            ob.inGameObject.GetComponent<Facility>().isLoadByManager = true;
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
            ob.inGameObject.GetComponent<Bulb>().isOn = ob.isOn;
            ob.inGameObject.GetComponent<Bulb>().LifeTimer = ob.timer;
            ob.inGameObject.GetComponent<Bulb>().isAlive = ob.isAlive;
            ob.inGameObject.GetComponent<Bulb>().isLoadByManager = true;
        }
        else if (ob.type == "Nest")
        {
            ob.inGameObject = Instantiate(Nest01, tempPos, Quaternion.identity);
        }
        else if (ob.type == "Wreckage")
        {
            ob.inGameObject = Instantiate(Wreckage01, tempPos, Quaternion.identity);
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
                            if (RangeSearch(i, pair.Key, 2, "Bulb", "Bulb01", true) == false)
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
                    if (pair.Value.inGameObject == null && pair.Value.facilityIsMake == true && pair.Value.timer > 0)
                    {
                        pair.Value.timer -= Time.deltaTime;
                    }
                }
                else if (pair.Value.type == "Bulb")
                {
                    if (pair.Value.inGameObject == null && pair.Value.isOn == true)
                    {
                        pair.Value.timer += Time.deltaTime;
                        if(pair.Value.timer > pair.Value.bulbLifeTime)
                        {
                            pair.Value.isAlive = false;
                            pair.Value.isOn = false;
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
            foreach (KeyValuePair<int, SceneObject> pair in SObjects[i])
            {
                if (pair.Value.type == "Plant")
                {
                    if (pair.Value.inGameObject != null)
                    {
                        pair.Value.plantState = pair.Value.inGameObject.GetComponent<Plant>().state;
                        pair.Value.timer = pair.Value.inGameObject.GetComponent<Plant>().growthTimer;
                    }
                }
                else if (pair.Value.type == "Facility")
                {
                    if (pair.Value.inGameObject != null)
                    {
                        pair.Value.isOn = pair.Value.inGameObject.GetComponent<Facility>().isOn;
                        pair.Value.timer = pair.Value.inGameObject.GetComponent<FacilityBalloon>().progressTimer;
                        pair.Value.facilityTimeToMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().timeToMake;
                        pair.Value.facilityMakeItem = pair.Value.inGameObject.GetComponent<FacilityBalloon>().makeItem;
                        pair.Value.facilityIsMake = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMake;
                        pair.Value.facilityIsMakeFinish = pair.Value.inGameObject.GetComponent<FacilityBalloon>().isMakeFinish;
                        pair.Value.isAlive = pair.Value.inGameObject.GetComponent<Facility>().isAlive;
                    }
                }
                else if (pair.Value.type == "Bulb")
                {
                    if (pair.Value.inGameObject != null)
                    {
                        pair.Value.isOn = pair.Value.inGameObject.GetComponent<Bulb>().isOn;
                        pair.Value.timer = pair.Value.inGameObject.GetComponent<Bulb>().LifeTimer;
                        pair.Value.isAlive = pair.Value.inGameObject.GetComponent<Bulb>().isAlive;
                        pair.Value.bulbLifeTime = pair.Value.inGameObject.GetComponent<Bulb>().LifeTime;
                    }
                }
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
                    if (pair.Value.plantState != 4 && pair.Value.plantState != 5)
                    {
                        pair.Value.plantState = 1;
                    }
                    else
                    {
                        pair.Value.plantState = 5;
                    }
                }
                else if (pair.Value.type == "Facility")
                {
                    if (pair.Value.name != "EscapePod")
                    {
                        if(pair.Value.isOn == true)
                        {
                            pair.Value.isAlive = false;
                        }
                    }
                    pair.Value.facilityIsMake = false;
                    pair.Value.facilityIsMakeFinish = false;
                }
                else if (pair.Value.type == "Bulb")
                {
                    if (pair.Value.isOn == true)
                    {
                        pair.Value.isAlive = false;
                    }
                }
            }
        }

        hungerGauge.SetAmount(-20);
        oxygenGauge.SetAmount(-10);
        energyGauge.SetAmount(-10);
        timer.ResetTimer();
        timer.PassDay();
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
    public bool RangeSearch(int sceneNum, int grid, int range, string type, string name = "NoName", bool isSearchLight = false)
    {
        for (int i = (grid - range); i <= (grid + range); i++)
        {
            if (SObjects[sceneNum].ContainsKey(i) == true)
            {
                if (SObjects[sceneNum][i].type == type && name == "NoName")
                {
                    if (type == "Bulb" && isSearchLight == true)
                    {
                        if (SObjects[sceneNum][i].isOn == true && SObjects[sceneNum][i].isAlive == true)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (SObjects[sceneNum][i].type == type && SObjects[sceneNum][i].name == name)
                {
                    if(type == "Bulb" && isSearchLight == true)
                    {
                        if(SObjects[sceneNum][i].isOn == true && SObjects[sceneNum][i].isAlive == true)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
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
