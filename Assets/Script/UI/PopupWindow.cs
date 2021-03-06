﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    Inventory inventory;
    GameObject Player;
    NyxUI nyxUI;

    GameObject Facility;

    GameObject PopupBG;
    GameObject[] ScrollIndexItem = new GameObject[5];
    GameObject ScrollIndexCursor;
    Text ExplanationName;
    Text ExplanationText;
    GameObject[] MaterialsItem = new GameObject[6];
    GameObject[] MaterialsNum = new GameObject[6];
    GameObject Button;
    Text ButtonText;
    Text TimeNum;
    Text NyxNum;

    public Sprite YellowButton;
    public Sprite RedButton;

    int selectedIndex = 0;
    int displayedSelectedIndex = 0;

    bool isPopupActive = false;
    Animator animaitor;

    float openTimer = 0;

    bool isShortage;

    Dictionary<string, bool> researchDictionary = new Dictionary<string, bool>();

    public class WindowItem
    {
        public WindowItem(string n, int t, int nx, string iName, string ex, string m1, int mn1,
            string m2 = "", int mn2 = 0, string m3 = "", int mn3 = 0, string m4 = "", int mn4 = 0,
            string m5 = "", int mn5 = 0, string m6 = "", int mn6 = 0)
        {
            name = n;
            time = t;
            nyx = nx;
            itemName = iName;
            expText = ex;

            material[0] = m1; materialNum[0] = mn1;
            material[1] = m2; materialNum[1] = mn2;
            material[2] = m3; materialNum[2] = mn3;
            material[3] = m4; materialNum[3] = mn4;
            material[4] = m5; materialNum[4] = mn5;
            material[5] = m6; materialNum[5] = mn6;
        }
        public string name;
        public int time;
        public int nyx;
        public string itemName;
        public string expText;
        
        public string[] material = new string[6];
        public int[] materialNum = new int[6];
    }

    List<WindowItem> WindowItemList = new List<WindowItem>();

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();
        PopupBG = transform.Find("PopupBG").gameObject;
        animaitor = GetComponent<Animator>();

        SetWindowObject();

        //SetWindowItem();
        RefreshWindow();

        //SetItemMakePossible(Inventory.Item.Oxygen); //산소는 처음부터 제작가능
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
        ExplanationName = Explanation.transform.Find("ItemName").gameObject.GetComponent<Text>();
        ExplanationText = Explanation.transform.Find("ItemText").gameObject.GetComponent<Text>();

        GameObject MakingMaterials = PopupBG.transform.Find("MakingMaterials").gameObject;
        for (int i = 0; i < 6; i++)
        {
            MaterialsItem[i] = MakingMaterials.transform.Find("Item" + (i + 1)).gameObject;
            MaterialsNum[i] = MaterialsItem[i].transform.Find("Text").gameObject;
        }
        Button = MakingMaterials.transform.Find("Button").gameObject;
        ButtonText = Button.transform.Find("Text").gameObject.GetComponent<Text>();

        GameObject TimeAndNyx = PopupBG.transform.Find("TimeAndNyx").gameObject;
        TimeNum = TimeAndNyx.transform.Find("TimeNum").gameObject.GetComponent<Text>();
        NyxNum = TimeAndNyx.transform.Find("NyxNum").gameObject.GetComponent<Text>();
    }

    public void SetItemMakePossible(string itemName, bool b = true)
    {
        researchDictionary[itemName] = b;
    }

    public void AddItem(string itemName) //아이템 추가시 수정할 부분
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
            case "Item_Trap01":
                WindowItemList.Add(new WindowItem(itemName, 10, 50, "소형 덫", "가시 덩굴의 가시로 만든 덫이다.\n괴물의 둥지 근처에 설치해두면 괴물의 심장을 얻을 수 있다.", "Item_Thorn", 2));
                break;
            case "Item_Battery":
                WindowItemList.Add(new WindowItem(itemName, 20, 200, "배터리", "괴물의 심장으로 만든 고밀도 배터리이다. 가공해서 형체는 많이 달라졌지만 들고 있으면 작은 움직임이 느껴진다.\n사용하면 에너지 게이지를 충전할 수 있다.", "Item_Heart", 1, "Item_Hose", 1));
                break;
            case "Item_Facility01":
                WindowItemList.Add(new WindowItem(itemName, 30, 300, "소형 워크벤치", "괴물의 조직을 이용해서 만든 워크벤치이다. 살아있지만 위험하지는 않다.\n각종 아이템을 제작할 수 있다.", "Item_Stick", 2, "Item_Board", 1, "Item_Heart", 1));
                break;
            case "Item_Bulb":
                WindowItemList.Add(new WindowItem(itemName, 30, 500, "간이 전구", "빛을 내서 괴물의 접근을 막는 시설이다.\n간이 전구의 양 옆 2칸 이내에는 다른 시설을 설치할 수 있다.", "Item_Stick", 2, "Item_Heart", 1, "Item_Hose", 1, "Item_Mass", 1));
                break;
            case "Item_Food":
                WindowItemList.Add(new WindowItem(itemName, 10, 50, "식량", "유전자 조작으로 만든 괴물종양을 가공해서 식품으로 만들었다.\n젠장 맛없다.\n사용하면 허기 게이지를 충전할 수 있다.", "Item_Tumor", 1));
                break;
            case "Item_StickSeed":
                WindowItemList.Add(new WindowItem(itemName, 30, 150, "집게발 대나무 모종", "심으면 긴 막대처럼 생긴 괴식물이 자라난다.", "Item_Stick", 1, "Item_Mass", 1));
                break;
            case "Item_BoardSeed":
                WindowItemList.Add(new WindowItem(itemName, 30, 150, "판자 식물 모종", "심으면 판자처럼 생긴 괴식물이 자라난다.", "Item_Board", 1, "Item_Mass", 1));
                break;
            case "Item_ThornSeed":
                WindowItemList.Add(new WindowItem(itemName, 30, 150, "가시 덩굴 모종", "심으면 가시가 달린 괴식물이 자라난다.", "Item_Thorn", 1, "Item_Mass", 1));
                break;
            case "Item_Oxygen":
                WindowItemList.Add(new WindowItem(itemName, 50, 40, "산소", "폐허가 된 지구에서 숨을 쉬기 위해서는 정제를 통해 산소를 얻는 방법 밖에는 없다.\n산소가 다 떨어지면 나는 어떻게 될까?\n사용하면 산소 게이지를 충전할 수 있다.", "Item_Water", 3));
                break;
            case "Item_TumorSeed":
                WindowItemList.Add(new WindowItem(itemName, 10, 30, "종양 씨앗", "유전자 조작으로 만든 안전한 종양 씨앗이다.\n수확 후 나약해진 식물에 심으면 식용 종양을 얻을 수 있다.", "Item_Mass", 1));
                break;
            case "Item_Grinder01":
                WindowItemList.Add(new WindowItem(itemName, 30, 500, "간이 분해기", "필요 없는 아이템을 분해해서 다른 아이템을 얻는데 사용하는 시설이다.", "Item_Sawtooth", 2, "Item_Hose", 1, "Item_Stick", 3, "Item_Heart", 1, "Item_Mass", 2));
                break;
            case "Item_NyxCollector01":
                WindowItemList.Add(new WindowItem(itemName, 30, 10, "닉스입자 수집기", "닉스입자를 수집하는 시설이다.\n허공에 떠도는 검은 닉스입자를 수집해서 사용할 수 있게 만들어준다.", "Item_Stick", 2, "Item_Board", 1, "Item_Thorn", 2, "Item_Heart", 1));
                break;
            case "Item_FruitSeed":
                WindowItemList.Add(new WindowItem(itemName, 30, 150, "열매 나무 모종", "심으면 먹을 수 있는 열매가 열리는 괴식물이 자라난다.", "Item_Fruit", 1, "Item_Mass", 1));
                break;
            case "Item_Hose":
                WindowItemList.Add(new WindowItem(itemName, 15, 50, "호스", "무언가를 만드는데 사용할 수 있는 재료 아이템이다.", "Item_Stick", 1, "Item_Board", 1));
                break;
            case "Item_Sawtooth":
                WindowItemList.Add(new WindowItem(itemName, 15, 50, "톱날", "무언가를 만드는데 사용할 수 있는 재료 아이템이다.", "Item_Board", 1, "Item_Thorn", 1));
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
        SoundManager.instance.PlaySE(13);
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
        SoundManager.instance.PlaySE(15);
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
        if (nyxUI.GetAmount() < WindowItemList[selectedIndex].nyx)
        {
            makePossible = false;
        }

        if (makePossible == true)
        {
            for (int i = 0; i < 6; i++)
            {
                if (WindowItemList[selectedIndex].materialNum[i] > 0)
                {
                    inventory.DeleteItem(WindowItemList[selectedIndex].material[i], WindowItemList[selectedIndex].materialNum[i]);
                }
            }
            nyxUI.SetAmount(-1 * WindowItemList[selectedIndex].nyx);
            Facility.GetComponent<FacilityBalloon>().MakeItem(WindowItemList[selectedIndex].name, WindowItemList[selectedIndex].time);

            CloseWindow();
            SoundManager.instance.PlaySE(18);
        }
        else
        {
            SoundManager.instance.PlaySE(17);
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
            SoundManager.instance.PlaySE(14);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && selectedIndex < WindowItemList.Count - 1)
        {
            selectedIndex++;
            if (displayedSelectedIndex < 4)
            {
                displayedSelectedIndex++;
            }
            RefreshWindow();
            SoundManager.instance.PlaySE(14);
        }

        ScrollIndexCursor.transform.position = ScrollIndexItem[displayedSelectedIndex].transform.position;
    }

    string IntegerToTime(int t)
    {
        int m = t / 60;
        int s = t % 60;

        string stm = m.ToString();
        string sts = s.ToString();

        if (m < 10)
        {
            stm = "0" + m;
        }
        if (s < 10)
        {
            sts = "0" + s;
        }

        return stm + ":" + sts;
    }

    void RefreshWindow()
    {
        Color tempColor = MaterialsItem[0].GetComponent<Image>().color;

        if(WindowItemList.Count <= 0)
        {
            ExplanationName.text = string.Empty;
            ExplanationText.text = "※ 지금은 이 시설에서 제작 가능한 아이템이 없습니다.";
            Button.GetComponent<Image>().sprite = YellowButton;
            ButtonText.text = string.Empty;
            TimeNum.GetComponent<Text>().text = string.Empty;
            NyxNum.GetComponent<Text>().text = string.Empty;
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
                ScrollIndexItem[i].GetComponent<Image>().sprite = inventory.GetItemSprite(WindowItemList[i + selectedIndex - displayedSelectedIndex].name);
            }
            else
            {
                ScrollIndexItem[i].SetActive(false);
            }
        }
        ExplanationName.text = WindowItemList[selectedIndex].itemName;
        ExplanationText.text = WindowItemList[selectedIndex].expText;

        isShortage = false;
        for (int i = 0; i<6; i++)
        {
            if(WindowItemList[selectedIndex].materialNum[i] != 0)
            {
                tempColor.a = 1;
                MaterialsItem[i].GetComponent<Image>().color = tempColor;
                MaterialsItem[i].GetComponent<Image>().sprite = inventory.GetItemSprite(WindowItemList[selectedIndex].material[i]);
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

        TimeNum.text = IntegerToTime(WindowItemList[selectedIndex].time);
        NyxNum.text = WindowItemList[selectedIndex].nyx.ToString();
        if(nyxUI.GetAmount() >= WindowItemList[selectedIndex].nyx)
        {
            NyxNum.color = Color.white;
        }
        else
        {
            isShortage = true;
            NyxNum.color = Color.red;
        }

        if (isShortage == true)
        {
            Button.GetComponent<Image>().sprite = RedButton;
            ButtonText.text = "재료 부족";
        }
        else
        {
            Button.GetComponent<Image>().sprite = YellowButton;
            ButtonText.text = "C : 제작하기";
        }
    }

    public bool GetPopupActive()
    {
        return isPopupActive;
    }
}