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

    //아이템 이름
    public enum Item
    {
        Stick,
        RedStick,
        Board
    };

    GameObject[] itemSlot = new GameObject[6];
    List<ItemInfo> Items = new List<ItemInfo>();

    //아이템 스프타이트
    public Sprite StickSp;
    public Sprite RedStickSp;

    //아이템 이름에 따라 스프라이트 설정
    void SetItemSprite(GameObject slot, Item itemName)
    {
        if (itemName == Item.Stick)
            slot.GetComponent<Image>().sprite = StickSp;
        if (itemName == Item.RedStick)
            slot.GetComponent<Image>().sprite = RedStickSp;
    }

    void Start ()
    {
        itemSlot[0] = transform.Find("Item1").gameObject;
        itemSlot[1] = transform.Find("Item2").gameObject;
        itemSlot[2] = transform.Find("Item3").gameObject;
        itemSlot[3] = transform.Find("Item4").gameObject;
        itemSlot[4] = transform.Find("Item5").gameObject;
        itemSlot[5] = transform.Find("Item6").gameObject;
        
        for(int i=0; i<6; i++)
        {
            itemSlot[i].SetActive(false);
        }

        itemSlot[2].GetComponent<Image>().sprite = RedStickSp;
    }
	
	void Update ()
    {
        DisplayItem();

        // 테스트용 코드
        /*
        if (Input.GetKeyUp(KeyCode.Z))
        {
            GetItem(Item.Stick);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            GetItem(Item.RedStick);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            DeleteItem(Item.Stick);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            DeleteItem(Item.RedStick);
        }
        */
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
        for(int i = 5; i >= Items.Count; i--)
        {
            itemSlot[i].SetActive(false);
        }
    }

    //아이템 획득에 실패하면 false를 반환
    public bool GetItem(Item itemName)
    {
        if (Items.Count < 6)
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
