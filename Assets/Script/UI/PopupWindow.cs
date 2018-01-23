using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    GameObject Inventory;
    GameObject Player;

    GameObject Facility; //제작창을 띄운 시설의 정보

    GameObject PopupBG;
    GameObject[] ScrollIndexItem = new GameObject[5];
    GameObject ScrollIndexCursor;
    GameObject ExplanationName;
    GameObject ExplanationText;
    GameObject[] MaterialsItem = new GameObject[6];
    GameObject[] MaterialsNum = new GameObject[6];

    int selectedIndex = 0;
    int displayedSelectedIndex = 0;

    bool isPopupActive = false;
    Animator animaitor;

    float openTimer = 0;

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
        PopupBG = transform.Find("PopupBG").gameObject;
        animaitor = GetComponent<Animator>();

        SetWindowObject();
        SetDictionary();

        SetWindowItem(); // 테스트용 코드
        RefreshWindow();
    }

    void SetWindowObject()
    {
        GameObject ScrollIndex = PopupBG.transform.Find("ScrollIndex").gameObject;
        for (int i = 0; i < 5; i++)
        {
            ScrollIndexItem[i] = ScrollIndex.transform.Find("Item" + (i + 1)).gameObject;
        }
        ScrollIndexCursor = ScrollIndex.transform.Find("Cursor").gameObject;

        GameObject Explanation = PopupBG.transform.Find("Explanation").gameObject;
        ExplanationName = Explanation.transform.Find("ItemName").gameObject;
        ExplanationText = Explanation.transform.Find("ItemText").gameObject;

        GameObject MakingMaterials = PopupBG.transform.Find("MakingMaterials").gameObject;
        for (int i = 0; i < 6; i++)
        {
            MaterialsItem[i] = MakingMaterials.transform.Find("Item" + (i + 1)).gameObject;
            MaterialsNum[i] = MaterialsItem[i].transform.Find("Text").gameObject;
        }
    }

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        itemDictionary[global::Inventory.Item.Food] = Inventory.GetComponent<Inventory>().FoodSp;
        itemDictionary[global::Inventory.Item.Oxygen] = Inventory.GetComponent<Inventory>().OxygenSp;
        itemDictionary[global::Inventory.Item.Battery] = Inventory.GetComponent<Inventory>().BatterySp;
        itemDictionary[global::Inventory.Item.Stick] = Inventory.GetComponent<Inventory>().StickSp;
        itemDictionary[global::Inventory.Item.Board] = Inventory.GetComponent<Inventory>().BoardSp;
        itemDictionary[global::Inventory.Item.Hose] = Inventory.GetComponent<Inventory>().HoseSp;
        itemDictionary[global::Inventory.Item.Mass] = Inventory.GetComponent<Inventory>().MassSp;
    }

    public void AddItem(global::Inventory.Item itemName) //아이템 추가시 수정할 부분
    {
        switch (itemName)
        {
            case global::Inventory.Item.Battery:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Battery, "멋진배터리", "배터리 중에서 가장 잘생긴 인기배터리이다.\n\n걸리는 시간 : 5억년 31초", global::Inventory.Item.Stick, 1));
                break;
            case global::Inventory.Item.Food:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Food, "특제 계란후라이", "한솥도시락 주방장이 심혈을 기울여 만든 반숙 후라이를 첨단기술로 재현했다.\n\n걸리는 시간 : 3개월", global::Inventory.Item.Stick, 2));
                break;
            case global::Inventory.Item.Oxygen:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Oxygen, "San-So", "괴식물 막대를 어떻게 가공하면 산소가 되는걸까?\n미래 우주의 기술은 놀랍다.\n\n걸리는 시간 : 보름", global::Inventory.Item.Stick, 2, global::Inventory.Item.Battery, 1));
                break;
            case global::Inventory.Item.Stick:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Stick, "막대", "여기서 막대 만들 시간에 괴식물한테 채집하는게 이득이다.\n\n그렇다고 한다.\n\n걸리는 시간 : 조만간", global::Inventory.Item.Battery, 1, global::Inventory.Item.Food, 3));
                break;
            case global::Inventory.Item.Board:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Board, "판자", "판\n자\n다\n\n걸리는 시간 : 금방", global::Inventory.Item.Stick, 1, global::Inventory.Item.Food, 3));
                break;
            case global::Inventory.Item.Hose:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Hose, "호스", "테스트용 호스\n\n걸리는 시간 : 21세기", global::Inventory.Item.Stick, 7, global::Inventory.Item.Board, 5, global::Inventory.Item.Food, 7, global::Inventory.Item.Oxygen, 2));
                break;
            case global::Inventory.Item.Mass:
                WindowItemList.Add(new WindowItem(global::Inventory.Item.Mass, "덩어리 M.A.S.S.", "테스트용으로 재료를 엄청나게 많게 설정했다.\n참고로 MASS는 고유명사임.\n\n걸리는 시간 : 내일", global::Inventory.Item.Stick, 73, global::Inventory.Item.Board, 10, global::Inventory.Item.Food, 57, global::Inventory.Item.Oxygen, 42, global::Inventory.Item.Battery, 91));
                break;
        }
    }

    // 테스트용 코드
    void SetWindowItem()
    {
        AddItem(global::Inventory.Item.Battery);
        AddItem(global::Inventory.Item.Oxygen);
        AddItem(global::Inventory.Item.Food);
        AddItem(global::Inventory.Item.Mass);
    }

    void Update ()
    {
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }

        if(isPopupActive == true)
        {
            if(openTimer <= 0.3f)
            {
                openTimer += Time.deltaTime;
            }
            else
            {
                MoveCursor();
                if (Input.GetKeyUp(KeyCode.C))
                {
                    CloseWindow();
                }
                else if (Input.GetKeyUp(KeyCode.Z))
                {
                    MakeItem();
                }
            }
        }
    }

    public void OpenWindow(GameObject fac)
    {
        Facility = fac;
        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(false);
        isPopupActive = true;
        selectedIndex = 0;
        displayedSelectedIndex = 0;
        openTimer = 0;
        RefreshWindow();
        PopupBG.SetActive(true);
        animaitor.SetBool("isOpen", true);
    }

    public void CloseWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(true);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        selectedIndex = 0;
        displayedSelectedIndex = 0;
        openTimer = 0;
        MoveCursor();
        RefreshWindow();
        //PopupBG.SetActive(false);
        animaitor.SetBool("isOpen", false);
    }

    public void ClearItemList()
    {
        WindowItemList.Clear();
    }

    void MakeItem()
    {
        bool makePossible = true;
        for(int i=0; i<6; i++)
        {
            if(WindowItemList[selectedIndex].materialNum[i] > 0)
            {
                if(Inventory.GetComponent<Inventory>().HasItem(WindowItemList[selectedIndex].material[i], WindowItemList[selectedIndex].materialNum[i]) == false)
                {
                    makePossible = false;
                }
            }
        }

        if(makePossible == true)
        {
            for (int i = 0; i < 6; i++)
            {
                if (WindowItemList[selectedIndex].materialNum[i] > 0)
                {
                    Inventory.GetComponent<Inventory>().DeleteItem(WindowItemList[selectedIndex].material[i], WindowItemList[selectedIndex].materialNum[i]);
                }
            }
            //Inventory.GetComponent<Inventory>().GetItem(WindowItemList[selectedIndex].name);
            Facility.GetComponent<FacilityBalloon>().MakeItem(WindowItemList[selectedIndex].name, 15);
            CloseWindow();
        }

    }

    void MoveCursor()
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
            selectedIndex++;
            if (displayedSelectedIndex < 4)
            {
                displayedSelectedIndex++;
            }
            RefreshWindow();
        }

        ScrollIndexCursor.transform.position = ScrollIndexItem[displayedSelectedIndex].transform.position;
    }

    void RefreshWindow()
    {
        for(int i=0; i<5; i++)
        {
            if(i < WindowItemList.Count)
            {
                ScrollIndexItem[i].SetActive(true);
                ScrollIndexItem[i].GetComponent<Image>().sprite = itemDictionary[WindowItemList[i + selectedIndex - displayedSelectedIndex].name];
            }
            else
            {
                ScrollIndexItem[i].SetActive(false);
            }
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