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
    }

    public enum Item
    {
        Food, //아이템 추가시 수정할 부분
        Oxygen,
        Battery,
        Stick,
        RedStick,
        Board
    };

    GameObject[] itemSlot = new GameObject[7];
    List<ItemInfo> Items = new List<ItemInfo>();
    GameObject Cursor;
    GameObject[] InventoryMenu = new GameObject[3];
    GameObject[] InventoryMenuText = new GameObject[3];
    GameObject[] GetEffect = new GameObject[7];

    GameObject player = null;

    GameObject OxygenUI;
    GameObject HungerUI;
    GameObject EnergyUI;

    public Sprite FoodSp; //아이템 추가시 수정할 부분
    public Sprite OxygenSp;
    public Sprite BatterySp;
    public Sprite StickSp;
    public Sprite RedStickSp;

    public bool isInventoryActive = false;
    int selectedIndex = 0;

    void SetItemSprite(GameObject slot, Item itemName) //아이템 추가시 수정할 부분
    {
        if (itemName == Item.Food)
            slot.GetComponent<Image>().sprite = FoodSp;
        else if (itemName == Item.Oxygen)
            slot.GetComponent<Image>().sprite = OxygenSp;
        else if (itemName == Item.Battery)
            slot.GetComponent<Image>().sprite = BatterySp;
        else if (itemName == Item.Stick)
            slot.GetComponent<Image>().sprite = StickSp;
        else if (itemName == Item.RedStick)
            slot.GetComponent<Image>().sprite = RedStickSp;
    }

    void RefreshItemMenu()
    {
        for(int i = 0; i<3; i++)
        {
            InventoryMenu[i].SetActive(false);
        }
        if(Items.Count <= selectedIndex)
        {
            return;
        }

        switch (Items[selectedIndex].name) //아이템 추가시 수정할 부분
        {
            case Item.Food:
            case Item.Oxygen:
            case Item.Battery:
                InventoryMenu[0].SetActive(true);
                InventoryMenuText[0].GetComponent<Text>().text = "C : Remove";
                InventoryMenu[1].SetActive(true);
                InventoryMenuText[1].GetComponent<Text>().text = "Z : Use";
                break;
            
            case Item.Stick:
                InventoryMenu[0].SetActive(true);
                InventoryMenuText[0].GetComponent<Text>().text = "C : Remove";
                break;

            case Item.RedStick:
                InventoryMenu[0].SetActive(true);
                InventoryMenuText[0].GetComponent<Text>().text = "C : 먹기";
                InventoryMenu[1].SetActive(true);
                InventoryMenuText[1].GetComponent<Text>().text = "X : 마시기";
                InventoryMenu[2].SetActive(true);
                InventoryMenuText[2].GetComponent<Text>().text = "Z : 발차기";
                break;

            case Item.Board:
                InventoryMenu[0].SetActive(true);
                InventoryMenuText[0].GetComponent<Text>().text = "Z : 먹기";
                break;
        }
    }

    void InteractionItem() //아이템 추가시 수정할 부분
    {
        if (Items.Count <= selectedIndex)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            switch (Items[selectedIndex].name)
            {
                case Item.Food:
                    HungerUI.GetComponent<HungerGauge>().SetAmount(30);
                    DeleteItem(Items[selectedIndex].name);
                    RefreshItemMenu();
                    break;
                case Item.Oxygen:
                    OxygenUI.GetComponent<OxygenGauge>().SetAmount(30);
                    DeleteItem(Items[selectedIndex].name);
                    RefreshItemMenu();
                    break;
                case Item.Battery:
                    EnergyUI.GetComponent<EnergyGauge>().SetAmount(30);
                    DeleteItem(Items[selectedIndex].name);
                    RefreshItemMenu();
                    break;
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {

        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            switch (Items[selectedIndex].name)
            {
                case Item.Food:
                case Item.Oxygen:
                case Item.Battery:
                case Item.Stick:
                    DeleteItem(Items[selectedIndex].name);
                    RefreshItemMenu();
                    break;
            }
        }
    }

    void Start ()
    {
        Cursor = transform.Find("Cursor").gameObject;

        for (int i = 0; i < 3; i++)
        {
            InventoryMenu[i] = Cursor.transform.Find("InvenMenu" + (i + 1)).gameObject;
            InventoryMenuText[i] = InventoryMenu[i].transform.Find("Text").gameObject;
        }

        for (int i=0; i<7; i++)
        {
            itemSlot[i] = transform.Find("Item" + (i + 1)).gameObject;
            GetEffect[i] = transform.Find("GetEffect" + (i + 1)).gameObject;
            itemSlot[i].SetActive(false);
            GetEffect[i].SetActive(false);
        }

        OxygenUI = GameObject.Find("Oxygen_Needle");
        HungerUI = GameObject.Find("Hunger_Guage");
        EnergyUI = GameObject.Find("LeftUI");
    }
	
	void Update ()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        else
        {
            DisplayItem();

            if (Input.GetKeyUp(KeyCode.A) && isInventoryActive == false)
            {
                OpenInventory();
            }
            else if (Input.GetKeyUp(KeyCode.A) && isInventoryActive == true)
            {
                CloseInventory();
            }

            if(isInventoryActive == true)
            {
                MoveCursor();
                InteractionItem();
            }
        }
    }

    void OpenInventory()
    {
        if (player.GetComponent<PlayerMove>().GetMovePossible() == true)
        {
            selectedIndex = 0;
            isInventoryActive = true;
            Cursor.SetActive(true);
            player.GetComponent<PlayerMove>().SetMovePossible(false);
            RefreshItemMenu();
        }
    }

    void CloseInventory()
    {
        isInventoryActive = false;
        Cursor.SetActive(false);
        player.GetComponent<PlayerMove>().SetMovePossible(true);
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
                selectedIndex = 6;
            }
            RefreshItemMenu();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (selectedIndex < 6)
            {
                selectedIndex++;
            }
            else
            {
                selectedIndex = 0;
            }
            RefreshItemMenu();
        }

        Vector3 temp = Cursor.transform.position;
        temp.x = transform.position.x - 63 * 3 + selectedIndex * 63;
        Cursor.transform.position = temp;
    }

    void DisplayItem()
    {
        if (Items.Count > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                SetItemSprite(itemSlot[i], Items[i].name);
                itemSlot[i].SetActive(true);
                itemSlot[i].transform.Find("Text").GetComponent<Text>().text = "x" + Items[i].count;
            }
        }

        //빈칸 아이템 안보이게
        for(int i = 6; i >= Items.Count; i--)
        {
            itemSlot[i].SetActive(false);
        }
    }

    //나중에 한번에 여러개 획득, 여러개 제거 만들기
    //아이템 획득에 실패하면 false를 반환 
    public bool GetItem(Item itemName)
    {
        if (Items.Count < 7)
        {
            int? temp = isContains(itemName);
            if(temp.HasValue)
            {
                if(Items[(int)temp].count >= 99)
                {
                    return false;
                }
                Items[(int)temp].count++;
                GetEffectOn((int)temp);
            }
            else
            {
                Items.Add(new ItemInfo(itemName, 1));
                GetEffectOn(Items.Count - 1);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리에 없는 아이템을 소모하려 하면 false를 반환
    public bool DeleteItem(Item itemName)
    {
        int? temp = isContains(itemName);

        if(temp.HasValue)
        {
            if(Items[(int)temp].count > 1)
            {
                Items[(int)temp].count--;
            }
            else
            {
                Items.RemoveAt((int)temp);
            }
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
}
