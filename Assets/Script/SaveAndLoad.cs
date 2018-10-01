using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SceneObject_Save
{
    public SceneObject_Save(int sn, int k, string t, string n, int st, float ti)
    {
        sceneNum = sn;
        key = k;
        type = t;
        name = n;
        state = st;
        timer = ti;
    }

    public int sceneNum;
    public int key;
    public string type;
    public string name;
    public float timer;
    public int state;
}

[System.Serializable]
public class InventoryItem_Save
{
    public InventoryItem_Save(Inventory.Item i, int c)
    {
        item = i;
        count = c;
    }

    public Inventory.Item item;
    public int count;
}

[System.Serializable]
public class ResearchItem_Save
{
    public ResearchItem_Save(int n, bool k)
    {
        putNum = n;
        isKnown = k;
    }

    public int putNum;
    public bool isKnown;
}

[System.Serializable]
public class SaveObject
{
    public float hunger;
    public int day;
    public int questNum;
    public int nyx;
    public List<SceneObject_Save> objList;
    public List<InventoryItem_Save> itemList;
    public List<ResearchItem_Save> researchList;
}

public class SaveAndLoad : MonoBehaviour
{
    HungerGauge hungerGauge;
    ReportUI reportUI;
    Inventory inventory;
    ResearchWindow researchWindow;
    NyxUI nyxUI;

    public static SaveAndLoad instance = null;

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
    }

    void LoadUI()
    {
        hungerGauge = GameObject.Find("HungerUI").GetComponent<HungerGauge>();
        reportUI = GameObject.Find("ReportUI").GetComponent<ReportUI>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();
    }

    public void SaveGame()
    {
        LoadUI();

        SaveObject sObj = new SaveObject();
        sObj.hunger = hungerGauge.amountOfHunger;
        sObj.day = reportUI.day;
        sObj.questNum = reportUI.questNum;
        sObj.nyx = nyxUI.GetAmount();
        sObj.itemList = inventory.GetInventoryItemList();
        sObj.objList = SceneObjectManager.instance.GetSceneObjectList();
        sObj.researchList = researchWindow.GetResearchList();

        string toJson = JsonUtility.ToJson(sObj);
        print(toJson);

        File.WriteAllText(Application.dataPath + "/Save/SaveData.json", toJson);
    }

    public void LoadGame(bool isNewGame = false)
    {
        LoadUI();

        string fromJson;
        if (isNewGame == true)
        {
            fromJson = File.ReadAllText(Application.dataPath + "/Save/NewGameData.json");
        }
        else
        {
            fromJson = File.ReadAllText(Application.dataPath + "/Save/SaveData.json");
        }
        var loadData = JsonUtility.FromJson<SaveObject>(fromJson);

        hungerGauge.amountOfHunger = loadData.hunger;
        nyxUI.amountOfNyx = loadData.nyx;

        reportUI.day = loadData.day;
        reportUI.questNum = loadData.questNum;
        reportUI.RefreshUI();

        inventory.LoadItemList(loadData.itemList);
        researchWindow.LoadResearch(loadData.researchList);
        SceneObjectManager.instance.LoadSceneObject(loadData.objList);
    }
}