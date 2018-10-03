using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //아이템 파일명과 갯수
    public class InventoryItem
    {
        public InventoryItem(string i, int c)
        {
            item = i;
            count = c;
        }
        public string item;
        public int count;
    }

    //아이템명(한글명)과 설명
    public class Iteminformation
    {
        public Iteminformation(string n, string e)
        {
            name = n;
            explanation = e;
        }
        public Iteminformation(string n, string e, int a)
        {
            name = n;
            explanation = e;
            amountOfhunger = a;
        }
        public Iteminformation(string n, string e, string itype, string iName)
        {
            name = n;
            explanation = e;
            installType = itype;
            installName = iName;
        }

        public string name;
        public string explanation;
        public int amountOfhunger;
        public string installType;
        public string installName;
    }

    //public enum Item
    //{
    //    Food, //
    //    Oxygen,
    //    Battery,
    //    Stick,
    //    Board,
    //    Hose,
    //    Mass,
    //    Thorn,
    //    Facility01,
    //    Trap01,
    //    Heart,
    //    Bulb01,
    //    StickSeed,
    //    BoardSeed,
    //    ThornSeed,
    //    Tumor,
    //    TumorSeed,
    //    Grinder01,
    //    SuppliedBattery,
    //    SuppliedFood,
    //    Water,
    //    NyxCollector01,
    //    Nyx,
    //    Fruit,
    //    FruitSeed,
    //    Sawtooth
    //};

    List<InventoryItem> ItemList = new List<InventoryItem>(); //어떤 아이템이 몇개씩 있는지
    public Dictionary<string, Iteminformation> itemDictionary = new Dictionary<string, Iteminformation>();
    public List<string> discoveredItemList = new List<string>(); //발견된 아이템 목록

    GameObject[] itemSlot = new GameObject[15];
    GameObject Cursor;
    GameObject Arrow;
    GameObject[] GetEffect = new GameObject[15];

    GameObject player = null;
    Monologue monologue;
    HungerGauge hungerGauge;
    EnergyGauge energyGauge;
    InteractionMenu interactionMenu;
    GrinderWindow grinderWindow;
    ReportUI reportUI;

    //public Sprite FoodSp; //
    //public Sprite OxygenSp;
    //public Sprite BatterySp;
    //public Sprite StickSp;
    //public Sprite BoardSp;
    //public Sprite HoseSp;
    //public Sprite MassSp;
    //public Sprite ThornSp;
    //public Sprite Facility01Sp;
    //public Sprite Trap01Sp;
    //public Sprite HeartSp;
    //public Sprite Bulb01Sp;
    //public Sprite StickSeedSp;
    //public Sprite BoardSeedSp;
    //public Sprite ThornSeedSp;
    //public Sprite TumorSp;
    //public Sprite TumorSeedSp;
    //public Sprite EscapePodSp;
    //public Sprite Grinder01Sp;
    //public Sprite SuppliedBatterySp;
    //public Sprite SuppliedFoodSp;
    //public Sprite WaterSp;
    //public Sprite NyxCollector01Sp;
    //public Sprite NyxSp;
    //public Sprite FruitSp;
    //public Sprite FruitSeedSp;
    //public Sprite SawtoothSp;
    //public Sprite CaptureSp;
    //public Sprite Earth0Sp;
    //public Sprite Earth1Sp;
    //public Sprite Earth2Sp;
    //public Sprite HungerSp;
    //public Sprite SeedingPlantSp;

    public bool isInventoryActive = false;
    int selectedIndex = 0;

    Animator animaitor;
    float openTimer = 0;

    bool isOpenedByGrinder = false;


    public Sprite GetItemSprite(string fileName)
    {
        string path = "Sprite/Object/Item/";

        return Resources.Load<Sprite>(path + fileName);
    }

    public string GetItemName(string fileName)
    {
        if(itemDictionary[fileName] == null)
        {
            return "알 수 없는 아이템";
        }

        return itemDictionary[fileName].name;
    }

    public string GetItemExplanation(string fileName)
    {
        if (itemDictionary[fileName] == null)
        {
            return "정의되지 않은 아이템입니다.";
        }

        return itemDictionary[fileName].explanation;
    }

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        itemDictionary["Item_Food"] = new Iteminformation("식량", "유전자 조작으로 만든 종양을 가공해서 먹을 수 있게 만들었다.\n사용하면 허기 게이지가 50%만큼 회복된다.", 50);
        itemDictionary["Item_Oxygen"] = new Iteminformation("산소", "숨쉬는데 필요한 산소이다.\n사용하면 산소 게이지가 70%만큼 회복된다.");
        itemDictionary["Item_Battery"] = new Iteminformation("배터리", "괴물의 심장을 가공해서 만든 배터리이다.\n사용하면 에너지 게이지가 35%만큼 회복된다.");
        itemDictionary["Item_Stick"] = new Iteminformation("막대", "집게발 대나무에서 채집한 막대이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary["Item_Board"] = new Iteminformation("판자", "판자 식물에서 채집한 판자이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary["Item_Hose"] = new Iteminformation("호스", "막대와 판자를 합쳐서 만들어낸 아이템이다.");
        itemDictionary["Item_Mass"] = new Iteminformation("괴상한 덩어리", "괴물의 조직으로 추정되는 덩어리이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary["Item_Thorn"] = new Iteminformation("가시", "가시 덩굴에서 채집한 가시이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary["Item_Facility01"] = new Iteminformation("소형 워크벤치", "다양한 아이템을 만드는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.", "Facility", "TempFacility");
        itemDictionary["Item_Trap01"] = new Iteminformation("소형 덫", "작은 괴물을 잡기 위해 만든 덫이다.\n괴물의 둥지 근처에 설치하면 괴물의 심장을 얻을 수 있다.", "Plant", "Trap01");
        itemDictionary["Item_Heart"] = new Iteminformation("괴물의 심장", "작은 괴물의 심장이다.\n배터리나 시설을 만드는 데 사용할 수 있다.");
        itemDictionary["Item_Bulb"] = new Iteminformation("간이 전구", "빛을 내서 괴물의 접근을 막는 시설이다.\n탈출포드나 다른 전구 근처에는 설치할 수 없다.", "Bulb", "Bulb01");
        itemDictionary["Item_StickSeed"] = new Iteminformation("집게발 대나무 모종", "원하는 곳에 심으면 집게발 대나무가 자란다.\n게임 시간으로 하루에 한번 채집할 수 있다.", "Plant", "StickPlant");
        itemDictionary["Item_BoardSeed"] = new Iteminformation("판자 식물 모종", "원하는 곳에 심으면 판자 식물이 자란다.\n자라는 속도가 빨라서 연속으로 채집할 수 있다.", "Plant", "BoardPlant");
        itemDictionary["Item_ThornSeed"] = new Iteminformation("가시 덩굴 모종", "원하는 곳에 심으면 가시 덩굴이 자란다.\n현실 시간으로 1분에 한번 채집할 수 있다.", "Plant", "ThornPlant");
        itemDictionary["Item_Tumor"] = new Iteminformation("종양", "사용하면 허기 게이지가 10%만큼 회복된다.\n가공해서 식량으로 만들어 먹는 편이 효율적이다.", 10);
        itemDictionary["Item_TumorSeed"] = new Iteminformation("종양 씨앗", "나약해진 식물에 심으면 식용으로 쓸 수 있는 종양이 자란다.\n수확 후 나약해진 식물에 상호작용하여 심을 수 있다.");
        itemDictionary["Item_Grinder01"] = new Iteminformation("간이 분해기", "필요 없는 아이템을 분해해서 다른 아이템을 만드는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.", "Facility", "Grinder01");
        itemDictionary["Item_SuppliedBattery"] = new Iteminformation("보급용 배터리", "탈출포드에 들어있던 비상용 배터리이다.\n사용하면 에너지 게이지가 35%만큼 회복된다.");
        itemDictionary["Item_SuppliedFood"] = new Iteminformation("보급용 식량", "탈출포드에 들어있던 비상용 식량이다.\n사용하면 허기 게이지가 50%만큼 회복된다.", 50);
        itemDictionary["Item_Water"] = new Iteminformation("물", "아이템 분해를 통해 얻은 물이다.\n사용하면 허기 게이지가 5%만큼 회복된다.", 5);
        itemDictionary["Item_NyxCollector01"] = new Iteminformation("닉스입자 수집기", "닉스입자를 수집하는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.", "NyxCollector", "NyxCollector01");
        itemDictionary["Item_Nyx"] = new Iteminformation("닉스입자", "더미 아이템");
        itemDictionary["Item_Fruit"] = new Iteminformation("괴식물의 열매", "맛있어보인다. 먹을 수 있을 것 같다.\n사용하면 허기 게이지가 10%만큼 회복된다.", 10);
        itemDictionary["Item_FruitSeed"] = new Iteminformation("열매 나무 모종", "원하는 곳에 심으면 열매 나무가 자란다.\n게임 시간으로 하루에 한번 채집할 수 있다.", "Plant", "FruitPlant");
        itemDictionary["Item_Sawtooth"] = new Iteminformation("톱날", "가시와 판자를 합쳐서 만들어낸 아이템이다.");
    }

    public void OpenMenu() //
    {
        if (ItemList.Count <= selectedIndex)
        {
            return;
        }
        isInventoryActive = false;
        Arrow.SetActive(false);

        interactionMenu.ClearMenu();

        string m_name = GetItemName(ItemList[selectedIndex].item);
        string m_explanation = GetItemExplanation(ItemList[selectedIndex].item);
        interactionMenu.SetNameAndExp(m_name, m_explanation);

        switch (ItemList[selectedIndex].item)
        {
            case "Item_Food":
            case "Item_SuppliedFood":
            case "Item_Tumor":
            case "Item_Water":
            case "Item_Fruit":
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Food);
                break;
            case "Item_Facility01":
            case "Item_Trap01":
            case "Item_Bulb":
            case "Item_Grinder01":
            case "Item_NyxCollector01":
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Install);
                break;
            case "Item_StickSeed":
            case "Item_BoardSeed":
            case "Item_ThornSeed":
            case "Item_FruitSeed":
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Plant);
                break;
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Dump);
        
        interactionMenu.OpenMenu(this.gameObject, "Inventory", GetItemSprite(ItemList[selectedIndex].item));
    }

    bool IsInstallable(string type)
    {
        int sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
        {
            if(type == "Plant")
            {
                monologue.DisplayLog("여기는 모종을 심을 수 없을 것 같군.\n다른 곳에 심도록 하자.");
                return false;
            }
            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳을 찾아보자.");
            return false;
        }

        switch (type)
        {
            case "Facility":
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Bulb", "Bulb01") == false && SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Facility", "EscapePod") == false)
                {
                    monologue.DisplayLog("여기에 설치해두면 공격을 받을 것 같군.\n탈출포드나 전구가 있는 곳에 설치하자.");
                    return false;
                }
                break;
            case "Bulb":
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Bulb") == true || SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Facility", "EscapePod") == true)
                {
                    monologue.DisplayLog("근처에 이미 다른 광원이 있군.\n근처에 탈출포드나 전구가 없는 곳에 설치하자.");
                    return false;
                }
                break;
            case "Trap":
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Nest") == false)
                {
                    monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n괴물의 둥지 근처에 설치하자.");
                    return false;
                }
                break;
        }
        return true;
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
    {
        openTimer = 0;
        isInventoryActive = true;
        Arrow.SetActive(true);

        string selectedItem = ItemList[selectedIndex].item;
        int sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        switch (m)
        {
            case InteractionMenu.MenuItem.Food:
                hungerGauge.SetAmount(itemDictionary[selectedItem].amountOfhunger);
                DeleteItem(selectedItem);
                SoundManager.instance.PlaySE(40);
                break;
                
            case InteractionMenu.MenuItem.Install:
                if(energyGauge.GetAmount() < 5)
                {
                    monologue.DisplayLog("에너지가 부족해서 설치할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
                    break;
                }

                switch (ItemList[selectedIndex].item)
                {
                    case "Item_Facility01":
                    case "Item_Grinder01":
                    case "Item_NyxCollector01":
                        if(IsInstallable("Facility") == true)
                        {
                            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo(itemDictionary[selectedItem].installType, itemDictionary[selectedItem].installName));
                            energyGauge.SetAmount(-5);
                            DeleteItem(selectedItem);
                            SoundManager.instance.PlaySE(29);
                        }
                        break;
                    case "Item_Bulb":
                        if (IsInstallable("Bulb") == true)
                        {
                            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo(itemDictionary[selectedItem].installType, itemDictionary[selectedItem].installName));
                            energyGauge.SetAmount(-5);
                            DeleteItem(selectedItem);
                            SoundManager.instance.PlaySE(28);
                        }
                        break;
                    case "Item_Trap01":
                        if (IsInstallable("Trap") == true)
                        {
                            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo(itemDictionary[selectedItem].installType, itemDictionary[selectedItem].installName, 3));
                            energyGauge.SetAmount(-5);
                            DeleteItem(selectedItem);
                            SoundManager.instance.PlaySE(29);
                        }
                        break;
                    //case Item.Facility01:
                    //case Item.Grinder01:
                    //case Item.NyxCollector01:
                    //    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Bulb", "Bulb01") == false && SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Facility", "EscapePod") == false)
                    //    {
                    //        monologue.DisplayLog("여기에 설치해두면 공격을 받을 것 같군.\n탈출포드나 전구가 있는 곳에 설치하자.");
                    //    }
                    //    else if(SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                    //    {
                    //        monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                    //    }
                    //    else
                    //    {
                    //        if(Items[selectedIndex].name == Item.Facility01)
                    //        {
                    //            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Facility", "TempFacility"));
                    //        }
                    //        else if (Items[selectedIndex].name == Item.Grinder01)
                    //        {
                    //            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Facility", "Grinder01"));
                    //        }
                    //        else if (Items[selectedIndex].name == Item.NyxCollector01)
                    //        {
                    //            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("NyxCollector", "NyxCollector01"));
                    //        }
                    //        energyGauge.SetAmount(-5);
                    //        DeleteItem(Items[selectedIndex].name);
                    //        SoundManager.instance.PlaySE(29);
                    //    }
                    //    break;
                    //case Item.Trap01:
                    //    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Nest") == false)
                    //    {
                    //        monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n괴물의 둥지 근처에 설치하자.");
                    //    }
                    //    else if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                    //    {
                    //        monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                    //    }
                    //    else
                    //    {
                    //        if (Items[selectedIndex].name == Item.Trap01)
                    //        {
                    //            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "Trap01", 3));
                    //        }
                    //        energyGauge.SetAmount(-5);
                    //        DeleteItem(Items[selectedIndex].name);
                    //        SoundManager.instance.PlaySE(28);
                    //    }
                    //    break;
                    //case Item.Bulb01:
                    //    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Bulb") == true || SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Facility", "EscapePod") == true)
                    //    {
                    //        monologue.DisplayLog("근처에 이미 다른 광원이 있군.\n근처에 탈출포드나 전구가 없는 곳에 설치하자.");
                    //    }
                    //    else if (SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Bulb", "Bulb01")) == true)
                    //    {
                    //        energyGauge.SetAmount(-5);
                    //        DeleteItem(Items[selectedIndex].name);
                    //        SoundManager.instance.PlaySE(29);
                    //    }
                    //    else
                    //    {
                    //        monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                    //    }
                    //    break;
                }
                openTimer = 1;
                CloseInventory(false);
                break;

            case InteractionMenu.MenuItem.Plant:
                if (energyGauge.GetAmount() < 5)
                {
                    monologue.DisplayLog("에너지가 부족해서 심을 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
                    break;
                }

                if (IsInstallable("Plant") == true)
                {
                    SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo(itemDictionary[selectedItem].installType, itemDictionary[selectedItem].installName, 0));
                    energyGauge.SetAmount(-5);
                    DeleteItem(selectedItem);
                    SoundManager.instance.PlaySE(41);
                }

                //if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                //{
                //    monologue.DisplayLog("여기는 모종을 심을 수 없을 것 같군.\n다른 곳에 심도록 하자.");
                //}
                //else
                //{
                //    if (Items[selectedIndex].name == Item.StickSeed)
                //    {
                //        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "StickPlant", 0));
                //    }
                //    else if (Items[selectedIndex].name == Item.BoardSeed)
                //    {
                //        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "BoardPlant", 0));
                //    }
                //    else if (Items[selectedIndex].name == Item.ThornSeed)
                //    {
                //        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "ThornPlant", 0));
                //    }
                //    else if (Items[selectedIndex].name == Item.FruitSeed)
                //    {
                //        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "FruitPlant", 0));
                //    }
                //    energyGauge.SetAmount(-5);
                //    DeleteItem(Items[selectedIndex].name);
                //    SoundManager.instance.PlaySE(41);
                //}
                openTimer = 1;
                CloseInventory(false);
                break;

            case InteractionMenu.MenuItem.Dump:
                DeleteItem(selectedItem);
                break;
        }
    }

    public void CancleMenu()
    {
        isInventoryActive = true;
        Arrow.SetActive(true);
        openTimer = 0;
    }

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        Cursor = transform.Find("Cursor").gameObject;
        Arrow = transform.Find("Arrow").gameObject;

        for (int i=0; i<15; i++)
        {
            itemSlot[i] = transform.Find("Item" + (i + 1)).gameObject;
            GetEffect[i] = transform.Find("GetEffect" + (i + 1)).gameObject;
            itemSlot[i].SetActive(false);
            GetEffect[i].SetActive(false);
        }

        hungerGauge = GameObject.Find("HungerUI").GetComponent<HungerGauge>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        grinderWindow = GameObject.Find("GrinderWindow").GetComponent<GrinderWindow>();
        reportUI = GameObject.Find("ReportUI").GetComponent<ReportUI>();

        SetDictionary();
    }
	
	void Update ()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
            if(player != null)
            {
                monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
            }
            return;
        }

        if (isInventoryActive == true)
        {
            openTimer += Time.deltaTime;

            MoveCursor();
            if (Input.GetKeyUp(KeyCode.C))
            {
                if (isOpenedByGrinder == false)
                {
                    OpenMenu();
                }
                else
                {
                    if (ItemList.Count > selectedIndex && openTimer > 0.1f)
                    {
                        grinderWindow.SelectItem(false, ItemList[selectedIndex].item);
                        CloseInventory();
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Z))
            {
                if (openTimer > 0.1f)
                {
                    CloseInventory();
                    if (isOpenedByGrinder == true)
                    {
                        grinderWindow.CloseWindow();
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (player.GetComponent<PlayerInteraction>().GetInteractionPossible() == true)
                {
                    OpenInventory();
                }
            }
        }
    }

    public void OpenInventory(bool isGrinder = false)
    {
        if (player.GetComponent<PlayerMove>().GetMovePossible() == true || isGrinder == true)
        {
            isOpenedByGrinder = isGrinder;
            Cursor.SetActive(true);
            Arrow.SetActive(true);
            animaitor.SetBool("isOpen", true);
            //selectedIndex = 0;
            isInventoryActive = true;
            player.GetComponent<PlayerMove>().SetMovePossible(false);
            DisplayItem();
            openTimer = 0;
            SoundManager.instance.PlaySE(34);
        }
    }

    void CloseInventory(bool playSE = true)
    {
        if (openTimer <= 0.1f)
        {
            return;
        }
        animaitor.SetBool("isOpen", false);
        isInventoryActive = false;
        Cursor.SetActive(false);
        Arrow.SetActive(false);
        if (isOpenedByGrinder == false)
        {
            player.GetComponent<PlayerMove>().SetMovePossible(true);
        }
        if (playSE == true)
        {
            SoundManager.instance.PlaySE(35);
        }
    }

    void GetEffectOn(int index)
    {
        GetEffect[index].SetActive(true);
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
            }
            else
            {
                selectedIndex = 14;
            }
            SoundManager.instance.PlaySE(36);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (selectedIndex < 14)
            {
                selectedIndex++;
            }
            else
            {
                selectedIndex = 0;
            }
            SoundManager.instance.PlaySE(36);
        }
        
        Cursor.transform.position = itemSlot[selectedIndex].transform.position;
        Arrow.transform.position = itemSlot[selectedIndex].transform.position;

        if(isOpenedByGrinder == true)
        {
            if (ItemList.Count <= selectedIndex)
            {
                grinderWindow.InventoryMove(true);
            }
            else
            {
                grinderWindow.InventoryMove(false, ItemList[selectedIndex].item);
            }
        }
    }

    void DisplayItem()
    {
        if (ItemList.Count > 0)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                itemSlot[i].GetComponent<Image>().sprite = GetItemSprite(ItemList[i].item);
                itemSlot[i].SetActive(true);
                itemSlot[i].transform.Find("Text").GetComponent<Text>().text = "x" + ItemList[i].count;
            }
        }

        //빈칸 아이템 안보이게
        for(int i = 14; i >= ItemList.Count; i--)
        {
            itemSlot[i].SetActive(false);
        }
    }

    //아이템 획득에 실패하면 false를 반환 
    public bool GetItem(string itemName, int num = 1)
    {
        if (isFull(1, itemName) == false)
        {
            int? temp = isContains(itemName);
            if(temp.HasValue)
            {
                if(ItemList[(int)temp].count + num > 999)
                {
                    return false;
                }
                ItemList[(int)temp].count += num;
                GetEffectOn((int)temp);
            }
            else
            {
                ItemList.Add(new InventoryItem(itemName, num));
                GetEffectOn(ItemList.Count - 1);

                //if(discoveredItemList.Contains(itemName) == false)
                //{
                //    discoveredItemList.Add(itemName);
                //    researchWindow.DiscoverNewResearch(itemName);
                //}
            }
            DisplayItem();
            reportUI.RefreshUI();
            player.GetComponent<PlayerInteraction>().DisplayFT(GetItemName(itemName) + " +" + num, Color.white, true, GetItemSprite(itemName));
            SoundManager.instance.PlaySE(33);
            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리에 없는 아이템을 소모하려 하면 false를 반환
    public bool DeleteItem(string itemName, int num = 1, bool isTest = false)
    {
        int? temp = isContains(itemName);

        if(temp.HasValue)
        {
            if(ItemList[(int)temp].count - num >= 1)
            {
                ItemList[(int)temp].count -= num;
            }
            else if(ItemList[(int)temp].count - num == 0)
            {
                ItemList.RemoveAt((int)temp);
            }
            else if(ItemList[(int)temp].count - num < 0)
            {
                return false;
            }
            DisplayItem();
            reportUI.RefreshUI();
            if(isTest == false)
            {
                player.GetComponent<PlayerInteraction>().DisplayFT(GetItemName(itemName) + " -" + num, Color.white, true, GetItemSprite(itemName));
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리 초기화
    public void DeleteAllItems()
    {
        ItemList.Clear();
        DisplayItem();
        reportUI.RefreshUI();
    }

    //해당 아이템이 없으면 null을, 있으면 해당 아이템의 리스트번호를 리턴
    int? isContains(string itemName)
    {
        for(int i=0; i< ItemList.Count; i++)
        {
            if (ItemList[i].item == itemName)
            {
                return i;
            }
        }
        return null;
    }

    //갯수만큼 존재하면 true
    public bool HasItem(string itemName, int count = 1)
    {
        foreach(InventoryItem i in ItemList)
        {
            if(i.item == itemName && i.count >= count)
            {
                return true;
            }
        }

        return false;
    }

    public int CountOfItem(string itemName)
    {
        foreach (InventoryItem i in ItemList)
        {
            if (i.item == itemName)
            {
                return i.count;
            }
        }
        return 0;
    }

    // isFull 메소드에서 임시로 아이템을 추가해보는데 사용하는 메소드
    bool FullTestGetItem(string itemName, int num = 1)
    {
        if (ItemList.Count >= 15 && (HasItem(itemName) == false || HasItem(itemName, 1000 - num) == true))
        {
            return false;
        }
        else
        {
            int? temp = isContains(itemName);
            if (temp.HasValue)
            {
                if (ItemList[(int)temp].count + num > 999)
                {
                    return false;
                }
                ItemList[(int)temp].count += num;
            }
            else
            {
                ItemList.Add(new InventoryItem(itemName, num));
            }
            return true;
        }
    }

    // 가득 차있으면 true를 반환
    public bool isFull(int NumOfItemType, string itemName1, int num1 = 1, string itemName2 = "", int num2 = 1, string itemName3 = "", int num3 = 1, string itemName4 = "", int num4 = 1, string itemName5 = "", int num5 = 1)
    {
        bool result = false;
        bool[] temp = new bool[5];
        string[] iName = new string[5] { itemName1, itemName2, itemName3, itemName4, itemName5 };
        int[] num = new int[5] { num1, num2, num3, num4, num5 };
        
        for (int i = 0; i < NumOfItemType; i++)
        {
            temp[i] = FullTestGetItem(iName[i], num[i]);
        }

        for (int i = 0; i < NumOfItemType; i++)
        {
            if (temp[i] == false)
            {
                result = true;
            }
            else if(temp[i] == true)
            {
                DeleteItem(iName[i], num[i], true);
            }
        }

        return result;
    }

    public List<InventoryItem_Save> GetInventoryItemList()
    {
        List<InventoryItem_Save> temp = new List<InventoryItem_Save>();

        for (int i = 0; i < ItemList.Count; i++)
        {
            temp.Add(new InventoryItem_Save(ItemList[i].item, ItemList[i].count));
        }

        return temp;
    }

    public void LoadItemList(List<InventoryItem_Save> list)
    {
        DeleteAllItems();

        for(int i = 0; i < list.Count; i++)
        {
            ItemList.Add(new InventoryItem(list[i].item, list[i].count));
        }
        
        DisplayItem();
    }
}
