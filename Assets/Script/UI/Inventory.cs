﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public class ItemInfo
    {
        public ItemInfo(Item n, int c)
        {
            name = n;
            count = c;
        }
        public Item name;
        public int count;
    }

    public class ItemName
    {
        public ItemName(Sprite sp, string n, string e)
        {
            sprite = sp;
            ObjectName = n;
            ObjectExplanation = e;
        }

        public Sprite sprite;
        public string ObjectName;
        public string ObjectExplanation;
    }

    public enum Item
    {
        Food, //
        Oxygen,
        Battery,
        Stick,
        Board,
        Hose,
        Mass,
        Thorn,
        Facility01,
        Trap01,
        Heart,
        Bulb01,
        StickSeed,
        BoardSeed,
        ThornSeed,
        Tumor,
        TumorSeed,
        Grinder01,
        SuppliedBattery,
        SuppliedFood,
        Water,
        NyxCollector01,
        Nyx,
        Fruit,
        FruitSeed,
        Sawtooth
    };

    GameObject[] itemSlot = new GameObject[15];
    List<ItemInfo> Items = new List<ItemInfo>();
    GameObject Cursor;
    GameObject Arrow;
    GameObject[] GetEffect = new GameObject[15];

    GameObject player = null;
    Monologue monologue;
    HungerGauge hungerGauge;
    EnergyGauge energyGauge;
    //ResearchWindow researchWindow;
    InteractionMenu interactionMenu;
    GrinderWindow grinderWindow;
    ReportUI reportUI;

    public Sprite FoodSp; //
    public Sprite OxygenSp;
    public Sprite BatterySp;
    public Sprite StickSp;
    public Sprite BoardSp;
    public Sprite HoseSp;
    public Sprite MassSp;
    public Sprite ThornSp;
    public Sprite Facility01Sp;
    public Sprite Trap01Sp;
    public Sprite HeartSp;
    public Sprite Bulb01Sp;
    public Sprite StickSeedSp;
    public Sprite BoardSeedSp;
    public Sprite ThornSeedSp;
    public Sprite TumorSp;
    public Sprite TumorSeedSp;
    public Sprite EscapePodSp;
    public Sprite Grinder01Sp;
    public Sprite SuppliedBatterySp;
    public Sprite SuppliedFoodSp;
    public Sprite WaterSp;
    public Sprite NyxCollector01Sp;
    public Sprite NyxSp;
    public Sprite FruitSp;
    public Sprite FruitSeedSp;
    public Sprite SawtoothSp;

    public Sprite CaptureSp;
    public Sprite Earth0Sp;
    public Sprite Earth1Sp;
    public Sprite Earth2Sp;
    public Sprite HungerSp;
    public Sprite SeedingPlantSp;

    public bool isInventoryActive = false;
    int selectedIndex = 0;

    Animator animaitor;
    float openTimer = 0;

    //public Dictionary<Item, Sprite> itemDictionary = new Dictionary<Item, Sprite>();
    public Dictionary<Item, ItemName> itemDictionary = new Dictionary<Item, ItemName>();
    public List<Item> discoveredItemList = new List<Item>();

    bool isOpenedByGrinder = false;

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        itemDictionary[Item.Food] = new ItemName(Resources.Load<Sprite>("Sprite/Object/Item/Item_Food"), "식량", "유전자 조작으로 만든 종양을 가공해서 먹을 수 있게 만들었다.\n사용하면 허기 게이지가 50%만큼 회복된다.");
        itemDictionary[Item.Oxygen] = new ItemName(OxygenSp, "산소", "숨쉬는데 필요한 산소이다.\n사용하면 산소 게이지가 70%만큼 회복된다.");
        itemDictionary[Item.Battery] = new ItemName(BatterySp, "배터리", "괴물의 심장을 가공해서 만든 배터리이다.\n사용하면 에너지 게이지가 35%만큼 회복된다.");
        itemDictionary[Item.Stick] = new ItemName(Resources.Load<Sprite>("Sprite/Object/Item/Item_Stick"), "막대", "집게발 대나무에서 채집한 막대이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary[Item.Board] = new ItemName(BoardSp, "판자", "판자 식물에서 채집한 판자이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary[Item.Hose] = new ItemName(HoseSp, "호스", "막대와 판자를 합쳐서 만들어낸 아이템이다.");
        itemDictionary[Item.Mass] = new ItemName(MassSp, "괴상한 덩어리", "괴물의 조직으로 추정되는 덩어리이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary[Item.Thorn] = new ItemName(ThornSp, "가시", "가시 덩굴에서 채집한 가시이다.\n무언가를 만드는 데 사용할 수 있을 것 같다.");
        itemDictionary[Item.Facility01] = new ItemName(Facility01Sp, "소형 워크벤치", "다양한 아이템을 만드는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.");
        itemDictionary[Item.Trap01] = new ItemName(Trap01Sp, "소형 덫", "작은 괴물을 잡기 위해 만든 덫이다.\n괴물의 둥지 근처에 설치하면 괴물의 심장을 얻을 수 있다.");
        itemDictionary[Item.Heart] = new ItemName(HeartSp, "괴물의 심장", "작은 괴물의 심장이다.\n배터리나 시설을 만드는 데 사용할 수 있다.");
        itemDictionary[Item.Bulb01] = new ItemName(Bulb01Sp, "간이 전구", "빛을 내서 괴물의 접근을 막는 시설이다.\n탈출포드나 다른 전구 근처에는 설치할 수 없다.");
        itemDictionary[Item.StickSeed] = new ItemName(StickSeedSp, "집게발 대나무 모종", "원하는 곳에 심으면 집게발 대나무가 자란다.\n게임 시간으로 하루에 한번 채집할 수 있다.");
        itemDictionary[Item.BoardSeed] = new ItemName(BoardSeedSp, "판자 식물 모종", "원하는 곳에 심으면 판자 식물이 자란다.\n자라는 속도가 빨라서 연속으로 채집할 수 있다.");
        itemDictionary[Item.ThornSeed] = new ItemName(ThornSeedSp, "가시 덩굴 모종", "원하는 곳에 심으면 가시 덩굴이 자란다.\n현실 시간으로 1분에 한번 채집할 수 있다.");
        itemDictionary[Item.Tumor] = new ItemName(TumorSp, "종양", "사용하면 허기 게이지가 10%만큼 회복된다.\n가공해서 식량으로 만들어 먹는 편이 효율적이다.");
        itemDictionary[Item.TumorSeed] = new ItemName(TumorSeedSp, "종양 씨앗", "식물에 심으면 식용으로 쓸 수 있는 종양이 자란다.\n수확 후 기력을 잃은 식물에만 심을 수 있다.");
        itemDictionary[Item.Grinder01] = new ItemName(Grinder01Sp, "간이 분해기", "필요 없는 아이템을 분해해서 다른 아이템을 만드는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.");
        itemDictionary[Item.SuppliedBattery] = new ItemName(SuppliedBatterySp, "보급용 배터리", "탈출포드에 들어있던 비상용 배터리이다.\n사용하면 에너지 게이지가 35%만큼 회복된다.");
        itemDictionary[Item.SuppliedFood] = new ItemName(SuppliedFoodSp, "보급용 식량", "탈출포드에 들어있던 비상용 식량이다.\n사용하면 허기 게이지가 50%만큼 회복된다.");
        itemDictionary[Item.Water] = new ItemName(WaterSp, "물", "아이템 분해를 통해 얻은 물이다.\n사용하면 허기 게이지가 5%만큼 회복된다.");
        itemDictionary[Item.NyxCollector01] = new ItemName(NyxCollector01Sp, "닉스입자 수집기", "닉스입자를 수집하는 시설이다.\n탈출포드나 전구 근처에 설치할 수 있다.");
        itemDictionary[Item.Nyx] = new ItemName(NyxSp, "닉스입자", "더미 아이템");
        itemDictionary[Item.Fruit] = new ItemName(FruitSp, "괴식물의 열매", "맛있어보인다. 먹을 수 있을 것 같다.\n사용하면 허기 게이지가 10%만큼 회복된다.");
        itemDictionary[Item.FruitSeed] = new ItemName(FruitSeedSp, "열매 나무 모종", "원하는 곳에 심으면 열매 나무가 자란다.\n게임 시간으로 하루에 한번 채집할 수 있다.");
        itemDictionary[Item.Sawtooth] = new ItemName(SawtoothSp, "톱날", "가시와 판자를 합쳐서 만들어낸 아이템이다.");
    }

    public void OpenMenu() //
    {
        if (Items.Count <= selectedIndex)
        {
            return;
        }
        isInventoryActive = false;
        Arrow.SetActive(false);

        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(itemDictionary[Items[selectedIndex].name].ObjectName, itemDictionary[Items[selectedIndex].name].ObjectExplanation);

        switch (Items[selectedIndex].name)
        {
            case Item.Food:
            case Item.SuppliedFood:
            case Item.Tumor:
            case Item.Water:
            case Item.Fruit:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Food);
                break;
            case Item.Oxygen:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Oxygen);
                break;
            case Item.Battery:
            case Item.SuppliedBattery:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Battery);
                break;
            case Item.Facility01:
            case Item.Trap01:
            case Item.Bulb01:
            case Item.Grinder01:
            case Item.NyxCollector01:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Install);
                break;
            case Item.StickSeed:
            case Item.BoardSeed:
            case Item.ThornSeed:
            case Item.FruitSeed:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Plant);
                break;
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Dump);

        interactionMenu.OpenMenu(this.gameObject, "Inventory", itemDictionary[Items[selectedIndex].name].sprite);
    }

    public void SelectMenu(InteractionMenu.MenuItem m) //
    {
        openTimer = 0;
        isInventoryActive = true;
        Arrow.SetActive(true);
        int sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        switch (m)
        {
            case InteractionMenu.MenuItem.Food:
                if (Items[selectedIndex].name == Item.Food || Items[selectedIndex].name == Item.SuppliedFood)
                {
                    hungerGauge.SetAmount(50);
                }
                else if(Items[selectedIndex].name == Item.Tumor)
                {
                    hungerGauge.SetAmount(10);
                }
                else if (Items[selectedIndex].name == Item.Water)
                {
                    hungerGauge.SetAmount(5);
                }
                else if (Items[selectedIndex].name == Item.Fruit)
                {
                    hungerGauge.SetAmount(10);
                }
                DeleteItem(Items[selectedIndex].name);
                SoundManager.instance.PlaySE(40);
                break;

            case InteractionMenu.MenuItem.Oxygen:
                DeleteItem(Items[selectedIndex].name);
                break;

            case InteractionMenu.MenuItem.Battery:
                energyGauge.SetAmount(35);
                DeleteItem(Items[selectedIndex].name);
                break;

            case InteractionMenu.MenuItem.Install:
                if(energyGauge.GetAmount() < 5)
                {
                    monologue.DisplayLog("에너지가 부족해서 설치할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
                    break;
                }
                switch (Items[selectedIndex].name)
                {
                    case Item.Facility01:
                    case Item.Grinder01:
                    case Item.NyxCollector01:
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Bulb", "Bulb01") == false && SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Facility", "EscapePod") == false)
                        {
                            monologue.DisplayLog("여기에 설치해두면 공격을 받을 것 같군.\n탈출포드나 전구가 있는 곳에 설치하자.");
                        }
                        else if(SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                        }
                        else
                        {
                            if(Items[selectedIndex].name == Item.Facility01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Facility", "TempFacility"));
                            }
                            else if (Items[selectedIndex].name == Item.Grinder01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Facility", "Grinder01"));
                            }
                            else if (Items[selectedIndex].name == Item.NyxCollector01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("NyxCollector", "NyxCollector01"));
                            }
                            energyGauge.SetAmount(-5);
                            DeleteItem(Items[selectedIndex].name);
                            SoundManager.instance.PlaySE(29);
                        }
                        break;
                    case Item.Trap01:
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Nest") == false)
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n괴물의 둥지 근처에 설치하자.");
                        }
                        else if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                        }
                        else
                        {
                            if (Items[selectedIndex].name == Item.Trap01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "Trap01", 3));
                            }
                            energyGauge.SetAmount(-5);
                            DeleteItem(Items[selectedIndex].name);
                            SoundManager.instance.PlaySE(28);
                        }
                        break;
                    case Item.Bulb01:
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Bulb") == true || SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Facility", "EscapePod") == true)
                        {
                            monologue.DisplayLog("근처에 이미 다른 광원이 있군.\n근처에 탈출포드나 전구가 없는 곳에 설치하자.");
                        }
                        else if (SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Bulb", "Bulb01")) == true)
                        {
                            energyGauge.SetAmount(-5);
                            DeleteItem(Items[selectedIndex].name);
                            SoundManager.instance.PlaySE(29);
                        }
                        else
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                        }
                        break;
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
                if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                {
                    monologue.DisplayLog("여기는 모종을 심을 수 없을 것 같군.\n다른 곳에 심도록 하자.");
                }
                else
                {
                    if (Items[selectedIndex].name == Item.StickSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "StickPlant", 0));
                    }
                    else if (Items[selectedIndex].name == Item.BoardSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "BoardPlant", 0));
                    }
                    else if (Items[selectedIndex].name == Item.ThornSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "ThornPlant", 0));
                    }
                    else if (Items[selectedIndex].name == Item.FruitSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.ObjectInfo("Plant", "FruitPlant", 0));
                    }
                    energyGauge.SetAmount(-5);
                    DeleteItem(Items[selectedIndex].name);
                    SoundManager.instance.PlaySE(41);
                }
                openTimer = 1;
                CloseInventory(false);
                break;

            case InteractionMenu.MenuItem.Dump:
                DeleteItem(Items[selectedIndex].name);
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
        //researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
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
        }
        else
        {
            if(isInventoryActive == true)
            {
                openTimer += Time.deltaTime;

                MoveCursor();
                if (Input.GetKeyUp(KeyCode.C))
                {
                    if(isOpenedByGrinder == false)
                    {
                        OpenMenu();
                    }
                    else
                    {
                        if (Items.Count > selectedIndex && openTimer > 0.1f)
                        {
                            grinderWindow.SelectItem(false, Items[selectedIndex].name);
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
            if (Items.Count <= selectedIndex)
            {
                grinderWindow.InventoryMove(true);
            }
            else
            {
                grinderWindow.InventoryMove(false, Items[selectedIndex].name);
            }
        }
    }

    void DisplayItem()
    {
        if (Items.Count > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                itemSlot[i].GetComponent<Image>().sprite = itemDictionary[Items[i].name].sprite;
                itemSlot[i].SetActive(true);
                itemSlot[i].transform.Find("Text").GetComponent<Text>().text = "x" + Items[i].count;
            }
        }

        //빈칸 아이템 안보이게
        for(int i = 14; i >= Items.Count; i--)
        {
            itemSlot[i].SetActive(false);
        }
    }

    //아이템 획득에 실패하면 false를 반환 
    public bool GetItem(Item itemName, int num = 1)
    {
        if (isFull(1, itemName) == false)
        {
            int? temp = isContains(itemName);
            if(temp.HasValue)
            {
                if(Items[(int)temp].count + num > 999)
                {
                    return false;
                }
                Items[(int)temp].count += num;
                GetEffectOn((int)temp);
            }
            else
            {
                Items.Add(new ItemInfo(itemName, num));
                GetEffectOn(Items.Count - 1);

                //if(discoveredItemList.Contains(itemName) == false)
                //{
                //    discoveredItemList.Add(itemName);
                //    researchWindow.DiscoverNewResearch(itemName);
                //}
            }
            DisplayItem();
            reportUI.RefreshUI();
            player.GetComponent<PlayerInteraction>().DisplayFT(itemDictionary[itemName].ObjectName + " +" + num, Color.white, true, itemName);
            SoundManager.instance.PlaySE(33);
            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리에 없는 아이템을 소모하려 하면 false를 반환
    public bool DeleteItem(Item itemName, int num = 1, bool isTest = false)
    {
        int? temp = isContains(itemName);

        if(temp.HasValue)
        {
            if(Items[(int)temp].count - num >= 1)
            {
                Items[(int)temp].count -= num;
            }
            else if(Items[(int)temp].count - num == 0)
            {
                Items.RemoveAt((int)temp);
            }
            else if(Items[(int)temp].count - num < 0)
            {
                return false;
            }
            DisplayItem();
            reportUI.RefreshUI();
            if(isTest == false)
            {
                player.GetComponent<PlayerInteraction>().DisplayFT(itemDictionary[itemName].ObjectName + " -" + num, Color.white, true, itemName);
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
        Items.Clear();
        DisplayItem();
        reportUI.RefreshUI();
    }

    //해당 아이템이 없으면 null을, 있으면 해당 아이템의 리스트번호를 리턴
    int? isContains(Item itemName)
    {
        for(int i=0; i<Items.Count; i++)
        {
            if (Items[i].name == itemName)
            {
                return i;
            }
        }
        return null;
    }

    //갯수만큼 존재하면 true
    public bool HasItem(Item itemName, int count = 1)
    {
        foreach(ItemInfo i in Items)
        {
            if(i.name == itemName && i.count >= count)
            {
                return true;
            }
        }

        return false;
    }

    public int CountOfItem(Item itemName)
    {
        foreach (ItemInfo i in Items)
        {
            if (i.name == itemName)
            {
                return i.count;
            }
        }
        return 0;
    }

    // isFull 메소드에서 임시로 아이템을 추가해보는데 사용하는 메소드
    bool FullTestGetItem(Item itemName, int num = 1)
    {
        if (Items.Count >= 15 && (HasItem(itemName) == false || HasItem(itemName, 1000 - num) == true))
        {
            return false;
        }
        else
        {
            int? temp = isContains(itemName);
            if (temp.HasValue)
            {
                if (Items[(int)temp].count + num > 999)
                {
                    return false;
                }
                Items[(int)temp].count += num;
            }
            else
            {
                Items.Add(new ItemInfo(itemName, num));
            }
            return true;
        }
    }

    // 가득 차있으면 true를 반환
    public bool isFull(int NumOfItemType, Item itemName1, int num1 = 1, Item itemName2 = Item.Battery, int num2 = 1, Item itemName3 = Item.Battery, int num3 = 1, Item itemName4 = Item.Battery, int num4 = 1, Item itemName5 = Item.Battery, int num5 = 1)
    {
        bool result = false;
        bool[] temp = new bool[5];
        Item[] iName = new Item[5] { itemName1, itemName2, itemName3, itemName4, itemName5 };
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

        for (int i = 0; i < Items.Count; i++)
        {
            temp.Add(new InventoryItem_Save(Items[i].name, Items[i].count));
        }

        return temp;
    }

    public void LoadItemList(List<InventoryItem_Save> list)
    {
        DeleteAllItems();

        for(int i = 0; i < list.Count; i++)
        {
            Items.Add(new ItemInfo(list[i].item, list[i].count));
        }
        
        DisplayItem();
    }
}
