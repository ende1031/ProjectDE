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
        Stick, //아이템 추가시 수정할 부분
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

    public Sprite StickSp; //아이템 추가시 수정할 부분
    public Sprite RedStickSp;

    public bool isInventoryActive = false;
    int selectedIndex = 0;

    void SetItemSprite(GameObject slot, Item itemName) //아이템 추가시 수정할 부분
    {
        if (itemName == Item.Stick)
            slot.GetComponent<Image>().sprite = StickSp;
        if (itemName == Item.RedStick)
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
            case Item.Stick:
                InventoryMenu[0].SetActive(true);
                InventoryMenuText[0].GetComponent<Text>().text = "X : Remove";
                InventoryMenu[1].SetActive(true);
                InventoryMenuText[1].GetComponent<Text>().text = "ㅋ : Use";
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
        }
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
