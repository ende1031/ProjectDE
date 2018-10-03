using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

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
    public InventoryItem_Save(string i, int c)
    {
        item = i;
        count = c;
    }

    public string item;
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

    string key = "iwillsurviveonthisplanet56789012";

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
        File.WriteAllText(Application.dataPath + "/Save/SaveData.json", Encrypt(toJson));
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
        var loadData = JsonUtility.FromJson<SaveObject>(Decrypt(fromJson));

        hungerGauge.amountOfHunger = loadData.hunger;
        nyxUI.amountOfNyx = loadData.nyx;

        reportUI.day = loadData.day;
        reportUI.questNum = loadData.questNum;
        reportUI.RefreshUI();

        inventory.LoadItemList(loadData.itemList);
        researchWindow.LoadResearch(loadData.researchList);
        SceneObjectManager.instance.LoadSceneObject(loadData.objList);
    }

    string Encrypt(string text)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);

        RijndaelManaged aes = new RijndaelManaged();
        aes.Key = keyArray;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = aes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    string Decrypt(string text)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = Convert.FromBase64String(text);

        RijndaelManaged aes = new RijndaelManaged();
        aes.Key = keyArray;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = aes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    //public void StartDataEncrypt()
    //{
    //    string temp = File.ReadAllText(Application.dataPath + "/Save/NewGameData.json");

    //    string encryptString = Encrypt(temp);
    //    File.WriteAllText(Application.dataPath + "/Save/NewGameData_Encrypt.json", encryptString);

    //    string decryptString = Decrypt(encryptString);
    //    File.WriteAllText(Application.dataPath + "/Save/NewGameData_Decrypt.json", decryptString);
    //}
}