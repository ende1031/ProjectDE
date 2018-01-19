using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    GameObject Inventory;

    GameObject[] ScrollIndexItem = new GameObject[5];
    GameObject ScrollIndexCursor;

    GameObject ExplanationName;
    GameObject ExplanationText;

    GameObject[] MaterialsItem = new GameObject[6];
    GameObject[] MaterialsNum = new GameObject[6];

    int selectedIndex = 0;
    int displayedSelectedIndex = 0;

    Dictionary<global::Inventory.Item, Sprite> itemDictionary = new Dictionary<global::Inventory.Item, Sprite>();

    public class WindowItem
    {
        public WindowItem(global::Inventory.Item n, string iName, string ex, global::Inventory.Item m1, int mn1,
            global::Inventory.Item m2 = 0, int mn2 = 0, global::Inventory.Item m3 = 0, int mn3 = 0, global::Inventory.Item m4 = 0, int mn4 = 0,
            global::Inventory.Item m5 = 0, int mn5 = 0, global::Inventory.Item m6 = 0, int mn6 = 0)
        {
            name = n;
            itemName = iName;
            expText = ex;

            material[0] = m1; materialNum[0] = mn1;
            material[1] = m2; materialNum[1] = mn2;
            material[2] = m3; materialNum[2] = mn3;
            material[3] = m4; materialNum[3] = mn4;
            material[4] = m5; materialNum[4] = mn5;
            material[5] = m6; materialNum[5] = mn6;
        }
        public global::Inventory.Item name;
        public string itemName;
        public string expText;
        
        public global::Inventory.Item[] material = new global::Inventory.Item[6];
        public int[] materialNum = new int[6];
    }

    List<WindowItem> WindowItemList = new List<WindowItem>();

    void Start ()
    {
        Inventory = GameObject.Find("Inventory");
        SetWindowObject();
        SetDictionary();

        SetWindowItem();
        RefreshWindow();
    }

    void SetWindowObject()
    {
        GameObject temp = transform.Find("ScrollIndex").gameObject;
        for (int i = 0; i < 5; i++)
        {
            ScrollIndexItem[i] = temp.transform.Find("Item" + (i + 1)).gameObject;
        }
        ScrollIndexCursor = temp.transform.Find("Cursor").gameObject;

        temp = transform.Find("Explanation").gameObject;
        ExplanationName = temp.transform.Find("ItemName").gameObject;
        ExplanationText = temp.transform.Find("ItemText").gameObject;

        temp = transform.Find("MakingMaterials").gameObject;
        for (int i = 0; i < 6; i++)
        {
            MaterialsItem[i] = temp.transform.Find("Item" + (i + 1)).gameObject;
            MaterialsNum[i] = MaterialsItem[i].transform.Find("Text").gameObject;
        }
    }

    void SetDictionary()
    {
        itemDictionary[global::Inventory.Item.Food] = Inventory.GetComponent<Inventory>().FoodSp;
        itemDictionary[global::Inventory.Item.Oxygen] = Inventory.GetComponent<Inventory>().OxygenSp;
        itemDictionary[global::Inventory.Item.Battery] = Inventory.GetComponent<Inventory>().BatterySp;
        itemDictionary[global::Inventory.Item.Stick] = Inventory.GetComponent<Inventory>().StickSp;
        itemDictionary[global::Inventory.Item.RedStick] = Inventory.GetComponent<Inventory>().RedStickSp;
    }

    // 실질적으로 게임에 들어갈 목록을 짜는 부분
    // 테스트용 코드
    void SetWindowItem()
    {
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리", "배터리 중에서 가장 잘생긴 인기배터리이다.", global::Inventory.Item.Stick, 1, global::Inventory.Item.Food, 3));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Food, "멋진계란후라이", "한솥도시락 주방장이 심혈을 기울여 만든 반숙 후라이를 첨단기술로 재현했다.", global::Inventory.Item.Stick, 2));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Oxygen, "산소같은너", "괴식물 막대를 어떻게 가공하면 산소가 되는걸까?\n미래 우주의 기술은 놀랍다.", global::Inventory.Item.Stick, 2, global::Inventory.Item.Battery, 1));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리2", "배터리 중에서 가장 잘생긴 인기배터리이다.2", global::Inventory.Item.Stick, 2, global::Inventory.Item.Food, 3));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리3", "배터리 중에서 가장 잘생긴 인기배터리이다.3", global::Inventory.Item.Stick, 3, global::Inventory.Item.Food, 1));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리4", "배터리 중에서 가장 잘생긴 인기배터리이다.4", global::Inventory.Item.Stick, 4, global::Inventory.Item.Food, 2));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리5", "배터리 중에서 가장 잘생긴 인기배터리이다.5", global::Inventory.Item.Stick, 5, global::Inventory.Item.Food, 3));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리6", "배터리 중에서 가장 잘생긴 인기배터리이다.6", global::Inventory.Item.Stick, 6, global::Inventory.Item.Food, 4));
        WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "마지막", "배터리 중에서 가장 잘생긴 인기배터리이다.\n테스트 마지막 목록", global::Inventory.Item.Stick, 7, global::Inventory.Item.Food, 5));
    }

    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) && selectedIndex > 0)
        {
            selectedIndex--;
            if (displayedSelectedIndex > 0)
            {
                displayedSelectedIndex--;
            }
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && selectedIndex < WindowItemList.Count - 1)
        {
            selectedIndex++; //5
            if(displayedSelectedIndex < 4)
            {
                displayedSelectedIndex++; //4 
            }
            RefreshWindow();
        }

        ScrollIndexCursor.transform.position = ScrollIndexItem[displayedSelectedIndex].transform.position;
    }

    void RefreshWindow()
    {
        for(int i=0; i<5; i++)
        {
            ScrollIndexItem[i].GetComponent<Image>().sprite = itemDictionary[WindowItemList[i + selectedIndex - displayedSelectedIndex].name];
        }
        ExplanationName.GetComponent<Text>().text = WindowItemList[selectedIndex].itemName;
        ExplanationText.GetComponent<Text>().text = WindowItemList[selectedIndex].expText;

        Color tempColor = MaterialsItem[0].GetComponent<Image>().color;

        for (int i = 0; i<6; i++)
        {
            if(WindowItemList[selectedIndex].materialNum[i] != 0)
            {
                tempColor.a = 1;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
                MaterialsItem[i].GetComponent<Image>().sprite = itemDictionary[WindowItemList[selectedIndex].material[i]];
                MaterialsNum[i].GetComponent<Text>().text = Inventory.GetComponent<Inventory>().CountOfItem(WindowItemList[selectedIndex].material[i]) + " / " + WindowItemList[selectedIndex].materialNum[i];
            }
            else
            {
                tempColor.a = 0;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
                MaterialsNum[i].GetComponent<Text>().text = string.Empty;
            }
        }
    }
}