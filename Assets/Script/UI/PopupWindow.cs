using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    Inventory inventory;
    EnergyGauge energyGauge;
    GameObject Player;

    GameObject Facility; //제작창을 띄운 시설의 정보

    GameObject PopupBG;
    GameObject[] ScrollIndexItem = new GameObject[5];
    GameObject ScrollIndexCursor;
    GameObject ExplanationName;
    GameObject ExplanationText;
    GameObject[] MaterialsItem = new GameObject[6];
    GameObject[] MaterialsNum = new GameObject[6];
    GameObject Button;
    GameObject ButtonText;

    public Sprite YellowButton;
    public Sprite RedButton;

    int selectedIndex = 0;
    int displayedSelectedIndex = 0;

    bool isPopupActive = false;
    Animator animaitor;

    float openTimer = 0;

    bool isShortage;

    Dictionary<Inventory.Item, bool> researchDictionary = new Dictionary<Inventory.Item, bool>();

    public class WindowItem
    {
        public WindowItem(Inventory.Item n, int t, string iName, string ex, Inventory.Item m1, int mn1,
            Inventory.Item m2 = 0, int mn2 = 0, Inventory.Item m3 = 0, int mn3 = 0, Inventory.Item m4 = 0, int mn4 = 0,
            Inventory.Item m5 = 0, int mn5 = 0, Inventory.Item m6 = 0, int mn6 = 0)
        {
            name = n;
            time = t;
            itemName = iName;
            expText = ex;

            material[0] = m1; materialNum[0] = mn1;
            material[1] = m2; materialNum[1] = mn2;
            material[2] = m3; materialNum[2] = mn3;
            material[3] = m4; materialNum[3] = mn4;
            material[4] = m5; materialNum[4] = mn5;
            material[5] = m6; materialNum[5] = mn6;
        }
        public Inventory.Item name;
        public int time;
        public string itemName;
        public string expText;
        
        public Inventory.Item[] material = new Inventory.Item[6];
        public int[] materialNum = new int[6];
    }

    List<WindowItem> WindowItemList = new List<WindowItem>();

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        energyGauge = GameObject.Find("LeftUI").GetComponent<EnergyGauge>();
        PopupBG = transform.Find("PopupBG").gameObject;
        animaitor = GetComponent<Animator>();

        SetWindowObject();

        //SetWindowItem();
        RefreshWindow();

        SetItemMakePossible(Inventory.Item.Oxygen); //산소는 처음부터 제작가능
        SetItemMakePossible(Inventory.Item.Mass); //산소는 처음부터 제작가능
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
        Button = MakingMaterials.transform.Find("Button").gameObject;
        ButtonText = Button.transform.Find("Text").gameObject;
    }

    public void SetItemMakePossible(Inventory.Item itemName, bool b = true)
    {
        researchDictionary[itemName] = b;
    }

    public void AddItem(Inventory.Item itemName) //아이템 추가시 수정할 부분
    {
        if(researchDictionary.ContainsKey(itemName) == false)
        {
            return;
        }
        else if(researchDictionary[itemName] == false)
        {
            return;
        }

        switch (itemName)
        {
            case Inventory.Item.Trap01:
                WindowItemList.Add(new WindowItem(itemName, 60, "소형 덫", "괴식물한테서 획득한 가시로 만든 소형 덫이다.\n어둠 속에 설치해 두면 작은 괴물을 잡을 수 있을 것 같다.\n반드시 빛이 없는 곳에 설치하자." + TimeToString(60), Inventory.Item.Stick, 2, Inventory.Item.Thorn, 3));
                break;
            case Inventory.Item.Battery:
                WindowItemList.Add(new WindowItem(itemName, 30, "배터리", "괴물의 심장으로 만든 고밀도 배터리이다. 가공해서 형체는 많이 달라졌지만 들고 있으면 작은 움직임이 느껴진다.\n사용하면 에너지 게이지를 충전할 수 있다." + TimeToString(30), Inventory.Item.Board, 1, Inventory.Item.Mass, 2, Inventory.Item.Heart, 1, Inventory.Item.Hose, 1));
                break;
            case Inventory.Item.Facility01:
                WindowItemList.Add(new WindowItem(itemName, 150, "워크벤치", "괴물의 조직을 이용해서 만든 워크벤치이다. 살아있지만 위험하지는 않다.\n각종 물품을 생산할 수 있다." + TimeToString(150), Inventory.Item.Stick, 30, Inventory.Item.Board, 18, Inventory.Item.Mass, 10, Inventory.Item.Heart, 1));
                break;
            case Inventory.Item.Bulb01:
                WindowItemList.Add(new WindowItem(itemName, 30, "전구", "괴물은 빛을 싫어한다.\n전구를 켜두면 내가 만들어둔 시설들이 공격받는 일도 없을 것이다." + TimeToString(30), Inventory.Item.Mass, 2, Inventory.Item.Heart, 1, Inventory.Item.Hose, 1, Inventory.Item.Tumor, 1));
                break;
            case Inventory.Item.Food:
                WindowItemList.Add(new WindowItem(itemName, 45, "식량", "젠장 맛없다.\n사용하면 허기 게이지를 충전할 수 있다." + TimeToString(45), Inventory.Item.Tumor, 5));
                break;
            case Inventory.Item.StickSeed:
                WindowItemList.Add(new WindowItem(itemName, 180, "막대 모종", "심으면 긴 막대처럼 생긴 괴식물이 자라난다." + TimeToString(180), Inventory.Item.Stick, 8, Inventory.Item.Mass, 3, Inventory.Item.Heart, 1));
                break;
            case Inventory.Item.BoardSeed:
                WindowItemList.Add(new WindowItem(itemName, 180, "판자 모종", "심으면 판자처럼 생긴 괴식물이 자라난다." + TimeToString(180), Inventory.Item.Board, 5, Inventory.Item.Mass, 3, Inventory.Item.Heart, 1));
                break;
            case Inventory.Item.ThornSeed:
                WindowItemList.Add(new WindowItem(itemName, 180, "가시 모종", "심으면 가시가 달린 괴식물이 자라난다." + TimeToString(180), Inventory.Item.Thorn, 7, Inventory.Item.Mass, 3, Inventory.Item.Heart, 1));
                break;
            case Inventory.Item.Oxygen:
                WindowItemList.Add(new WindowItem(itemName, 240, "산소", "폐허가 된 지구에서 숨을 쉬기 위해서는 정제를 통해 산소를 얻는 방법 밖에는 없다.\n산소가 다 떨어지면 나는 어떻게 될까?\n사용하면 산소 게이지를 충전할 수 있다." + TimeToString(240), Inventory.Item.Mass, 12));
                break;
            case Inventory.Item.Stick:
                WindowItemList.Add(new WindowItem(itemName, 30, "막대", "여기서 막대 만들 시간에 괴식물한테 채집하는게 이득이다.\n\n그렇다고 한다." + TimeToString(30), Inventory.Item.Battery, 1, Inventory.Item.Board, 2));
                break;
            case Inventory.Item.Board:
                WindowItemList.Add(new WindowItem(itemName, 45, "판자", "판\n자\n다" + TimeToString(45), Inventory.Item.Stick, 2, Inventory.Item.Food, 1));
                break;
            case Inventory.Item.Hose:
                WindowItemList.Add(new WindowItem(itemName, 70, "호스", "테스트용 호스" + TimeToString(70), Inventory.Item.Stick, 1, Inventory.Item.Board, 3, Inventory.Item.Food, 2, Inventory.Item.Oxygen, 1));
                break;
            case Inventory.Item.Mass:
                WindowItemList.Add(new WindowItem(itemName, 5, "덩어리", "테스트용.\n분해기에서 분해를 통해 얻을 수 있게 수정 예정" + TimeToString(5), Inventory.Item.Stick, 1));
                break;
            case Inventory.Item.Thorn:
                WindowItemList.Add(new WindowItem(itemName, 300, "가시", "괴식물을 통해 얻는게 빠르다." + TimeToString(300), Inventory.Item.Stick, 99, Inventory.Item.Board, 99, Inventory.Item.Food, 99, Inventory.Item.Oxygen, 99, Inventory.Item.Battery, 99));
                break;
            case Inventory.Item.Heart:
                WindowItemList.Add(new WindowItem(itemName, 45, "심장", "두근" + TimeToString(45), Inventory.Item.Thorn, 77, Inventory.Item.Facility01, 99));
                break;
            case Inventory.Item.Tumor:
                WindowItemList.Add(new WindowItem(itemName, 45, "종양", "더미" + TimeToString(45), Inventory.Item.Thorn, 77, Inventory.Item.Facility01, 99));
                break;
            case Inventory.Item.TumorSeed:
                WindowItemList.Add(new WindowItem(itemName, 30, "종양 씨앗", "아씨엔 라하브레하의 혼이 깃든 검은 크리스탈이다.\n조디아크를 소환할 수 있다." + TimeToString(30), Inventory.Item.Mass, 6));
                break;
            case Inventory.Item.Grinder01:
                WindowItemList.Add(new WindowItem(itemName, 150, "분쇄기", "혼돈의 물질을 분해해서 내부에 깃들은 어두운 에너지를 추출하는데 사용하는 시설이다." + TimeToString(150), Inventory.Item.Stick, 10, Inventory.Item.Board, 15, Inventory.Item.Thorn, 16, Inventory.Item.Hose, 20, Inventory.Item.Mass, 5, Inventory.Item.Heart, 1));
                break;
        }
    }

    // 기본세팅
    /*
    void SetWindowItem()
    {
        AddItem(Inventory.Item.Battery);
        AddItem(Inventory.Item.Oxygen);
        AddItem(Inventory.Item.Food);
        AddItem(Inventory.Item.Mass);
    }
    */

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
                if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
                {
                    CloseWindow();
                }
                else if (Input.GetKeyUp(KeyCode.C))
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
        //RefreshWindow();
        //PopupBG.SetActive(false);
        animaitor.SetBool("isOpen", false);
    }

    public void ClearItemList()
    {
        WindowItemList.Clear();
    }

    void MakeItem()
    {
        if (WindowItemList.Count <= 0)
        {
            return;
        }

        bool makePossible = true;
        for(int i=0; i<6; i++)
        {
            if(WindowItemList[selectedIndex].materialNum[i] > 0)
            {
                if(inventory.HasItem(WindowItemList[selectedIndex].material[i], WindowItemList[selectedIndex].materialNum[i]) == false)
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
                    inventory.DeleteItem(WindowItemList[selectedIndex].material[i], WindowItemList[selectedIndex].materialNum[i]);
                }
            }
            //inventory.GetItem(WindowItemList[selectedIndex].name);
            Facility.GetComponent<FacilityBalloon>().MakeItem(WindowItemList[selectedIndex].name, WindowItemList[selectedIndex].time);
            
            switch(WindowItemList[selectedIndex].name)
            {
                case Inventory.Item.Oxygen:
                case Inventory.Item.Battery:
                case Inventory.Item.Food:
                    break;

                default:
                    energyGauge.SetAmount(-10);
                    break;
            }

            CloseWindow();
        }

    }

    string TimeToString(int t)
    {
        int m = t / 60;
        int s = t % 60;

        string temp = "\n\n걸리는 시간 : " + m + "분 " + s + "초";
        if (m == 0)
        {
            temp = "\n\n걸리는 시간 : " + s + "초";
        }
        if (s == 0)
        {
            temp = "\n\n걸리는 시간 : " + m + "분";
        }
        if (t <= 0)
        {
            temp = "\n\n걸리는 시간 : 알 수 없음(오류 코드명 : 치킨마요)";
        }

        return temp;
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
        Color tempColor = MaterialsItem[0].GetComponent<Image>().color;

        if(WindowItemList.Count <= 0)
        {
            ExplanationName.GetComponent<Text>().text = string.Empty;
            ExplanationText.GetComponent<Text>().text = "※ 이 시설에서 제작 가능한 아이템이 없습니다.";
            Button.GetComponent<Image>().sprite = YellowButton;
            ButtonText.GetComponent<Text>().text = string.Empty;
            tempColor.a = 0;

            for (int i = 0; i < 5; i++)
            {
                ScrollIndexItem[i].SetActive(false);
            }
            for (int i = 0; i < 6; i++)
            {
                MaterialsNum[i].GetComponent<Text>().text = string.Empty;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
            }
            return;
        }

        for(int i=0; i<5; i++)
        {
            if(i < WindowItemList.Count)
            {
                ScrollIndexItem[i].SetActive(true);
                ScrollIndexItem[i].GetComponent<Image>().sprite = inventory.itemDictionary[WindowItemList[i + selectedIndex - displayedSelectedIndex].name];
            }
            else
            {
                ScrollIndexItem[i].SetActive(false);
            }
        }
        ExplanationName.GetComponent<Text>().text = WindowItemList[selectedIndex].itemName;
        ExplanationText.GetComponent<Text>().text = WindowItemList[selectedIndex].expText;

        isShortage = false;
        for (int i = 0; i<6; i++)
        {
            if(WindowItemList[selectedIndex].materialNum[i] != 0)
            {
                tempColor.a = 1;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
                MaterialsItem[i].GetComponent<Image>().sprite = inventory.itemDictionary[WindowItemList[selectedIndex].material[i]];
                MaterialsNum[i].GetComponent<Text>().text = inventory.CountOfItem(WindowItemList[selectedIndex].material[i]) + "/" + WindowItemList[selectedIndex].materialNum[i];
                if(WindowItemList[selectedIndex].materialNum[i] > inventory.CountOfItem(WindowItemList[selectedIndex].material[i]))
                {
                    isShortage = true;
                    MaterialsNum[i].GetComponent<Text>().color = Color.red;
                }
                else
                {
                    MaterialsNum[i].GetComponent<Text>().color = Color.white;
                }
            }
            else
            {
                tempColor.a = 0;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
                MaterialsNum[i].GetComponent<Text>().text = string.Empty;
            }
        }

        if(isShortage == true)
        {
            Button.GetComponent<Image>().sprite = RedButton;
            ButtonText.GetComponent<Text>().text = "재료 부족";
        }
        else
        {
            Button.GetComponent<Image>().sprite = YellowButton;
            ButtonText.GetComponent<Text>().text = "C : 제작하기";
        }
    }
}