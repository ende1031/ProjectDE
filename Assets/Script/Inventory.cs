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

    void DisplayItemMenu()
    {
        if(Items.Count <= selectedIndex)
        {
            return;
        }

        switch (Items[selectedIndex].name) //아이템 추가시 수정할 부분
        {
            case Item.Stick:
            case Item.RedStick:
                //버리기, 사용하기, 설치하기 등
                break;
            case Item.Board:
                //버리기, 사용하기, 설치하기 등
                break;
        }
    }

    void Start ()
    {
        Cursor = transform.Find("Cursor").gameObject;
        itemSlot[0] = transform.Find("Item1").gameObject;
        itemSlot[1] = transform.Find("Item2").gameObject;
        itemSlot[2] = transform.Find("Item3").gameObject;
        itemSlot[3] = transform.Find("Item4").gameObject;
        itemSlot[4] = transform.Find("Item5").gameObject;
        itemSlot[5] = transform.Find("Item6").gameObject;
        itemSlot[6] = transform.Find("Item7").gameObject;

        for (int i=0; i<7; i++)
        {
            itemSlot[i].SetActive(false);
        }

        itemSlot[2].GetComponent<Image>().sprite = RedStickSp;
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
                DisplayItemMenu();
            }
        }
    }

    void OpenInventory()
    {
        if (player.GetComponent<PlayerMove>().GetMovePossible() == true)
        {
            isInventoryActive = true;
            Cursor.SetActive(true);
            player.GetComponent<PlayerMove>().SetMovePossible(false);
        }
    }

    void CloseInventory()
    {
        isInventoryActive = false;
        Cursor.SetActive(false);
        player.GetComponent<PlayerMove>().SetMovePossible(true);
    }

    void MoveCursor()
    {
        Vector3 temp = Cursor.transform.position;
        temp.x = transform.position.x - 63 * 3 + selectedIndex * 63;
        Cursor.transform.position = temp;

        if (Input.GetKeyUp(KeyCode.LeftArrow) && selectedIndex > 0)
        {
            selectedIndex--;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && selectedIndex < 6)
        {
            selectedIndex++;
        }
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
            }
            else
            {
                Items.Add(new ItemInfo(itemName, 1));
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
}
