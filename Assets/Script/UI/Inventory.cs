using System.Collections;
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
        public string strigName;
    }

    public enum Item
    {
        Food, //아이템 추가시 수정할 부분
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
        Water
    };

    GameObject[] itemSlot = new GameObject[15];
    List<ItemInfo> Items = new List<ItemInfo>();
    GameObject Cursor;
    GameObject Arrow;
    GameObject[] GetEffect = new GameObject[15];

    GameObject player = null;
    Monologue monologue;
    HungerGauge hungerGauge;
    OxygenGauge oxygenGauge;
    EnergyGauge energyGauge;
    ResearchWindow researchWindow;
    InteractionMenu interactionMenu;
    GrinderWindow grinderWindow;

    public Sprite FoodSp; //아이템 추가시 수정할 부분
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

    public bool isInventoryActive = false;
    int selectedIndex = 0;

    Animator animaitor;
    float openTimer = 0;

    public Dictionary<Item, Sprite> itemDictionary = new Dictionary<Item, Sprite>();
    public List<Item> discoveredItemList = new List<Item>();

    bool isOpenedByGrinder = false;

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        itemDictionary[Item.Food] = FoodSp;
        itemDictionary[Item.Oxygen] = OxygenSp;
        itemDictionary[Item.Battery] = BatterySp;
        itemDictionary[Item.Stick] = StickSp;
        itemDictionary[Item.Board] = BoardSp;
        itemDictionary[Item.Hose] = HoseSp;
        itemDictionary[Item.Mass] = MassSp;
        itemDictionary[Item.Thorn] = ThornSp;
        itemDictionary[Item.Facility01] = Facility01Sp;
        itemDictionary[Item.Trap01] = Trap01Sp;
        itemDictionary[Item.Heart] = HeartSp;
        itemDictionary[Item.Bulb01] = Bulb01Sp;
        itemDictionary[Item.StickSeed] = StickSeedSp;
        itemDictionary[Item.BoardSeed] = BoardSeedSp;
        itemDictionary[Item.ThornSeed] = ThornSeedSp;
        itemDictionary[Item.Tumor] = TumorSp;
        itemDictionary[Item.TumorSeed] = TumorSeedSp;
        itemDictionary[Item.Grinder01] = Grinder01Sp;
        itemDictionary[Item.SuppliedBattery] = SuppliedBatterySp;
        itemDictionary[Item.SuppliedFood] = SuppliedFoodSp;
        itemDictionary[Item.Water] = WaterSp;
    }

    public void OpenMenu() //아이템 추가시 수정할 부분
    {
        if (Items.Count <= selectedIndex)
        {
            return;
        }
        isInventoryActive = false;
        Arrow.SetActive(false);

        interactionMenu.ClearMenu();

        switch (Items[selectedIndex].name)
        {
            case Item.Food:
            case Item.SuppliedFood:
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
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Install);
                break;
            case Item.StickSeed:
            case Item.BoardSeed:
            case Item.ThornSeed:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Plant);
                break;
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Dump);

        interactionMenu.OpenMenu(this.gameObject, "Inventory", itemDictionary[Items[selectedIndex].name]);
    }

    public void SelectMenu(InteractionMenu.MenuItem m) //아이템 추가시 수정할 부분
    {
        openTimer = 0;
        isInventoryActive = true;
        Arrow.SetActive(true);
        int sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        switch (m)
        {
            case InteractionMenu.MenuItem.Food:
                hungerGauge.SetAmount(50);
                DeleteItem(Items[selectedIndex].name);
                break;

            case InteractionMenu.MenuItem.Oxygen:
                oxygenGauge.SetAmount(70);
                DeleteItem(Items[selectedIndex].name);
                break;

            case InteractionMenu.MenuItem.Battery:
                energyGauge.SetAmount(35);
                DeleteItem(Items[selectedIndex].name);
                break;

            case InteractionMenu.MenuItem.Install:
                switch (Items[selectedIndex].name)
                {
                    case Item.Facility01:
                    case Item.Grinder01:
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Bulb", "Bulb01", true) == false && SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 2, "Facility", "EscapePod") == false)
                        {
                            monologue.DisplayLog("여기에 설치해두면 공격을 받을 것 같군.\n빛이 있는 곳에 설치하자.");
                        }
                        else if(SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                        }
                        else
                        {
                            if(Items[selectedIndex].name == Item.Facility01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Facility", "TempFacility"));
                            }
                            else if (Items[selectedIndex].name == Item.Grinder01)
                            {
                                SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Facility", "Grinder01"));
                            }
                            DeleteItem(Items[selectedIndex].name);
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
                            SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "Trap01", 3));
                        }
                        break;
                    case Item.Bulb01:
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Bulb") == true || SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PlayerGrid(), 4, "Facility", "EscapePod") == true)
                        {
                            monologue.DisplayLog("근처에 이미 다른 광원이 있군.\n빛이 없는 곳에 설치하자.");
                        }
                        else if (SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Bulb", "Bulb01")) == true)
                        {
                            DeleteItem(Items[selectedIndex].name);
                        }
                        else
                        {
                            monologue.DisplayLog("여기는 설치할 수 없을 것 같군.\n다른 곳에 설치하자.");
                        }
                        break;
                }
                openTimer = 1;
                CloseInventory();
                break;

            case InteractionMenu.MenuItem.Plant:
                if (SceneObjectManager.instance.ContainObject(sceneNum, Grid.instance.PlayerGrid()) == true)
                {
                    monologue.DisplayLog("여기는 모종을 심을 수 없을 것 같군.\n다른 곳에 설치하자.");
                }
                else
                {
                    if (Items[selectedIndex].name == Item.StickSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "StickPlant", 0));
                    }
                    else if (Items[selectedIndex].name == Item.BoardSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "BoardPlant", 0));
                    }
                    else if (Items[selectedIndex].name == Item.ThornSeed)
                    {
                        SceneObjectManager.instance.AddObject(sceneNum, Grid.instance.PlayerGrid(), new SceneObjectManager.SceneObject("Plant", "ThornPlant", 0));
                    }
                    DeleteItem(Items[selectedIndex].name);
                }
                openTimer = 1;
                CloseInventory();
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

        oxygenGauge = GameObject.Find("Oxygen_Needle").GetComponent<OxygenGauge>();
        hungerGauge = GameObject.Find("Hunger_Guage").GetComponent<HungerGauge>();
        energyGauge = GameObject.Find("LeftUI").GetComponent<EnergyGauge>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        grinderWindow = GameObject.Find("GrinderWindow").GetComponent<GrinderWindow>();

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
        }
    }

    void CloseInventory()
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
                itemSlot[i].GetComponent<Image>().sprite = itemDictionary[Items[i].name];
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
                if(Items[(int)temp].count + num > 99)
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

                if(discoveredItemList.Contains(itemName) == false)
                {
                    discoveredItemList.Add(itemName);
                    researchWindow.DiscoverNewResearch(itemName);
                }
            }
            DisplayItem();
            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리에 없는 아이템을 소모하려 하면 false를 반환
    public bool DeleteItem(Item itemName, int num = 1)
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
            return true;
        }
        else
        {
            return false;
        }
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
        if (Items.Count >= 15 && (HasItem(itemName) == false || HasItem(itemName, 100 - num) == true))
        {
            return false;
        }
        else
        {
            int? temp = isContains(itemName);
            if (temp.HasValue)
            {
                if (Items[(int)temp].count + num > 99)
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
                DeleteItem(iName[i], num[i]);
            }
        }

        return result;
    }
}
