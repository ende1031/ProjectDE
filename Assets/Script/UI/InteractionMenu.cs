using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenu : MonoBehaviour
{
    public enum MenuItem //메뉴 추가시 수정할 부분
    {
        Battery,
        Cancle,
        Dump,
        Food,
        Gather,
        Install,
        Make,
        Off,
        Oxygen,
        Plant,
        Remove,
        Research,
        Sleep,
        Tumor,
        Grind,
        Repair,
        Examine
    };

    enum Direction
    {
        Left,
        Right
    };

    public Sprite BatterySp; //메뉴 추가시 수정할 부분
    public Sprite CancleSp;
    public Sprite DumpSp;
    public Sprite FoodSp;
    public Sprite GatherSp;
    public Sprite InstallSp;
    public Sprite MakeSp;
    public Sprite OffSp;
    public Sprite OxygenSp;
    public Sprite PlantSp;
    public Sprite RemoveSp;
    public Sprite ResearchSp;
    public Sprite SleepSp;
    public Sprite TumorSp;
    public Sprite GrindSp;
    public Sprite RepairSp;
    public Sprite ExamineSp;

    public class MenuItemInfo
    {
        public MenuItemInfo(Sprite s, string t)
        {
            itemSp = s;
            buttonText = t;
        }

        public Sprite itemSp;
        public string buttonText;
    }

    Animator animaitor;
    GameObject Player;

    GameObject IMenu_bg;
    Image ItemImage;
    RectTransform ItemImageRT;
    Text ButtonText;
    GameObject IconGroup;
    GameObject[] MenuIcon = new GameObject[7];
    GameObject WarningPopup;
    Text WarningText;

    PopupWindow popupWindow;
    GrinderWindow grinderWindow;

    Text NameText;
    Text ExpText;

    List<MenuItem> MenuList = new List<MenuItem>();
    Dictionary<MenuItem, MenuItemInfo> MenuDictionary = new Dictionary<MenuItem, MenuItemInfo>();

    int selectedIndex = 0;

    bool isPopupActive = false;
    float openTimer = 0;
    float reOpenTimer = 0;
    bool isMove = false;
    bool isCursorMoveActive = false;

    bool isWarningPopupActive = false;

    float circleRadius = 135.0f;
    float moveAngle = 0;
    float moveSeed = 5.0f;
    float moveVec = 10.0f;
    Direction moveDirection = Direction.Right;
    float[] iconAlpha = new float[7] { 0.25f, 0.5f, 1.0f, 0.5f, 0.25f, 0, 0 };

    GameObject targetObject;
    string targetType;

    void Start ()
    {
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        grinderWindow = GameObject.Find("GrinderWindow").GetComponent<GrinderWindow>();
        IMenu_bg = transform.Find("IMenu_bg").gameObject;
        animaitor = IMenu_bg.GetComponent<Animator>();
        ItemImage = IMenu_bg.transform.Find("ItemImage").gameObject.GetComponent<Image>();
        ItemImageRT = IMenu_bg.transform.Find("ItemImage").gameObject.GetComponent<RectTransform>();
        ButtonText = IMenu_bg.transform.Find("ButtonText").gameObject.GetComponent<Text>();
        IconGroup = IMenu_bg.transform.Find("IconGroup").gameObject;
        for(int i =0; i < 7; i++)
        {
            MenuIcon[i] = IconGroup.transform.Find("Icon" + (i + 1)).gameObject;
        }

        NameText = IMenu_bg.transform.Find("NameText").gameObject.GetComponent<Text>();
        ExpText = IMenu_bg.transform.Find("ExpText").gameObject.GetComponent<Text>();

        WarningPopup = transform.Find("WarningPopup").gameObject;
        WarningText = WarningPopup.transform.Find("WarningText").gameObject.GetComponent<Text>();

        SetDictionary();
    }

    void SetDictionary() //메뉴 추가시 수정할 부분
    {
        MenuDictionary[MenuItem.Battery] = new MenuItemInfo(BatterySp, "배터리 사용");
        MenuDictionary[MenuItem.Cancle] = new MenuItemInfo(CancleSp, "작동 취소");
        MenuDictionary[MenuItem.Dump] = new MenuItemInfo(DumpSp, "버리기");
        MenuDictionary[MenuItem.Food] = new MenuItemInfo(FoodSp, "식품 섭취");
        MenuDictionary[MenuItem.Gather] = new MenuItemInfo(GatherSp, "아이템 획득");
        MenuDictionary[MenuItem.Install] = new MenuItemInfo(InstallSp, "설치하기");
        MenuDictionary[MenuItem.Make] = new MenuItemInfo(MakeSp, "아이템 제작");
        MenuDictionary[MenuItem.Off] = new MenuItemInfo(OffSp, "전원 끄기");
        MenuDictionary[MenuItem.Oxygen] = new MenuItemInfo(OxygenSp, "산소 충전");
        MenuDictionary[MenuItem.Plant] = new MenuItemInfo(PlantSp, "식물 심기");
        MenuDictionary[MenuItem.Remove] = new MenuItemInfo(RemoveSp, "제거하기");
        MenuDictionary[MenuItem.Research] = new MenuItemInfo(ResearchSp, "연구하기");
        MenuDictionary[MenuItem.Sleep] = new MenuItemInfo(SleepSp, "잠자기");
        MenuDictionary[MenuItem.Tumor] = new MenuItemInfo(TumorSp, "종양 심기");
        MenuDictionary[MenuItem.Grind] = new MenuItemInfo(GrindSp, "아이템 분해");
        MenuDictionary[MenuItem.Repair] = new MenuItemInfo(RepairSp, "수리하기");
        MenuDictionary[MenuItem.Examine] = new MenuItemInfo(ExamineSp, "살펴보기");
    }

    void Update ()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }

        if (isPopupActive == true)
        {
            if (openTimer <= 0.1f)
            {
                openTimer += Time.deltaTime;
            }
            else if (isMove == false)
            {
                MoveCursor();
                if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
                {
                    if (targetType == "Inventory")
                    {
                        targetObject.GetComponent<Inventory>().CancleMenu();
                    }
                    CloseWindow();
                }
                else if (Input.GetKeyUp(KeyCode.C))
                {
                    if(MenuList[selectedIndex] == MenuItem.Remove || MenuList[selectedIndex] == MenuItem.Dump || MenuList[selectedIndex] == MenuItem.Cancle || MenuList[selectedIndex] == MenuItem.Sleep)
                    {
                        OpenWarningPopup();
                    }
                    else
                    {
                        SelectMenu();
                    }
                }
            }
            else if(isMove == true)
            {
                MoveMenuIcon();
            }
        }
        else if(isWarningPopupActive == true)
        {
            if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
            {
                CloseWarningPopup();
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                SelectMenu();
            }
        }
        else if(popupWindow.GetPopupActive() == false && grinderWindow.GetUsingGrinder() == false)
        {
            reOpenTimer += Time.deltaTime;
        }
    }

    void MoveCursor()
    {
        if (isCursorMoveActive == true)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                moveDirection = Direction.Left;
                isMove = true;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                moveDirection = Direction.Right;
                isMove = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                isCursorMoveActive = true;
            }
        }
    }

    void MoveMenuIcon()
    {
        if (moveDirection == Direction.Right)
        {
            moveVec += Time.deltaTime * moveSeed;
            moveAngle -= moveVec;

            iconAlpha[0] = 0.25f + (moveAngle / 45 * 0.25f);
            iconAlpha[1] = 0.5f + (moveAngle / 45 * 0.5f);
            iconAlpha[2] = 1 - (moveAngle / 45 * 0.5f);
            iconAlpha[3] = 0.5f - (moveAngle / 45 * 0.25f);
            iconAlpha[4] = 0.25f - (moveAngle / 45 * 0.25f);
            iconAlpha[5] = 0;
            iconAlpha[6] = 0 + (moveAngle / 45 * 0.25f);

            // 이동 종료 조건
            if (moveAngle <= -1 * 360 / 8)
            {
                isMove = false;
                if (selectedIndex == MenuList.Count - 1)
                {
                    selectedIndex = 0;
                }
                else
                {
                    selectedIndex++;
                }
                RefreshWindow();
                moveAngle = 0;
                moveVec = 10.0f;
                return;
            }
        }
        else if (moveDirection == Direction.Left)
        {
            moveVec += Time.deltaTime * moveSeed;
            moveAngle += moveVec;

            iconAlpha[0] = 0.25f - (moveAngle / 45 * -1 * 0.25f);
            iconAlpha[1] = 0.5f - (moveAngle / 45 * -1 * 0.25f);
            iconAlpha[2] = 1 - (moveAngle / 45 * -1 * 0.5f);
            iconAlpha[3] = 0.5f + (moveAngle / 45 * -1 * 0.5f);
            iconAlpha[4] = 0.25f + (moveAngle / 45 * -1 * 0.25f);
            iconAlpha[5] = 0 + (moveAngle / 45 * -1 * 0.25f);
            iconAlpha[6] = 0;

            // 이동 종료 조건
            if (moveAngle >= 360 / 8)
            {
                isMove = false;
                if (selectedIndex == 0)
                {
                    selectedIndex = MenuList.Count - 1;
                }
                else

                {
                    selectedIndex--;
                }
                RefreshWindow();
                moveAngle = 0;
                moveVec = 10.0f;
                return;
            }
        }

        // 이동 적용
        for (int i = 0; i < 7; i++)
        {
            Vector3 tempVec = MenuIcon[i].transform.position;

            if (i != 6)
            {
                tempVec.x = Mathf.Cos((360 / 8 * i + 180 + moveAngle) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.x;
                tempVec.y = Mathf.Sin((360 / 8 * i + 180 + moveAngle) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.y;
            }
            else
            {
                tempVec.x = Mathf.Cos((360 / 8 * (i + 1) + 180 + moveAngle) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.x;
                tempVec.y = Mathf.Sin((360 / 8 * (i + 1) + 180 + moveAngle) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.y;
            }
            MenuIcon[i].transform.position = tempVec;
        }

        //투명도 적용
        Color tempColor = MenuIcon[0].GetComponent<Image>().color;
        for (int i = 0; i < 7; i++)
        {
            tempColor.a = iconAlpha[i];
            MenuIcon[i].GetComponent<Image>().color = tempColor;
        }
    }

    void RefreshWindow()
    {
        for (int i =0; i<7; i++)
        {
            Vector3 tempVec = MenuIcon[i].transform.position;

            if (i != 6)
            {
                tempVec.x = Mathf.Cos((360 / 8 * i + 180) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.x;
                tempVec.y = Mathf.Sin((360 / 8 * i + 180) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.y;
            }
            else
            {
                tempVec.x = Mathf.Cos((360 / 8 * (i + 1) + 180) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.x;
                tempVec.y = Mathf.Sin((360 / 8 * (i + 1) + 180) * Mathf.PI / 180) * circleRadius + IconGroup.transform.position.y;
            }
            MenuIcon[i].transform.position = tempVec;
        }

        //아이콘 스프라이트 설정
        int[] tempNum = new int[5];
        for(int i = 0; i < 5; i++)
        {
            tempNum[i] = selectedIndex - 2 + i;

            if (tempNum[i] < 0)
            {
                tempNum[i] += MenuList.Count;
            }
            if (tempNum[i] >= MenuList.Count)
            {
                tempNum[i] -= MenuList.Count;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            MenuIcon[i].GetComponent<Image>().sprite = MenuDictionary[MenuList[tempNum[i]]].itemSp;
        }
        //회전할때 살짝 보이고 평소엔 투병한 아이콘 2개
        MenuIcon[5].GetComponent<Image>().sprite = MenuDictionary[MenuList[tempNum[0]]].itemSp;
        MenuIcon[6].GetComponent<Image>().sprite = MenuDictionary[MenuList[tempNum[4]]].itemSp;

        //아이콘 투명도 설정
        iconAlpha = new float[7] { 0.25f, 0.5f, 1.0f, 0.5f, 0.25f, 0, 0 };
        Color tempColor = MenuIcon[0].GetComponent<Image>().color;
        for (int i = 0; i < 7; i++)
        {
            tempColor.a = iconAlpha[i];
            MenuIcon[i].GetComponent<Image>().color = tempColor;
        }

        //버튼 텍스트 설정
        ButtonText.text = MenuDictionary[MenuList[selectedIndex]].buttonText;
    }

    public void OpenMenu(GameObject to, string t, Sprite sp = null, float w = 220, float h = 220)
    {
        if (reOpenTimer < 0.3f)
        {
            if(t == "Inventory")
            {
                targetObject.GetComponent<Inventory>().CancleMenu();
            }
            return;
        }

        targetObject = to;
        targetType = t;

        if (MenuList.Count == 0)
        {
            MenuList.Add(MenuItem.Battery);
        }

        while (MenuList.Count < 5)
        {
            int temp = MenuList.Count;
            for (int i = 0; i < temp; i++)
            {
                MenuList.Add(MenuList[i]);
            }
        }

        ItemImage.sprite = sp;
        ItemImageRT.sizeDelta = new Vector2(w, h);

        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(false);
        IMenu_bg.SetActive(true);
        isPopupActive = true;
        selectedIndex = 0;
        openTimer = 0;
        isCursorMoveActive = false;
        RefreshWindow();
        animaitor.SetBool("isOpen", true);
    }

    // 반드시 ClearMenu이후, OpenMenu 이전에 호출할 것.
    public void SetNameAndExp(string n, string e)
    {
        NameText.text = n;
        ExpText.text = e;
    }

    public void CloseWindow()
    {
        reOpenTimer = 0;

        if (targetType != "Inventory")
        {
            Player.GetComponent<PlayerMove>().SetMovePossible(true);
        }
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        openTimer = 0;
        //IMenu_bg.SetActive(false);
        if(isWarningPopupActive == false)
        {
            animaitor.SetBool("isOpen", false);
        }
        isWarningPopupActive = false;
        WarningPopup.SetActive(false);
    }

    void OpenWarningPopup()
    {
        switch(MenuList[selectedIndex])
        {
            case MenuItem.Dump:
                WarningText.text = "정말 아이템을 버리시겠습니까?\n되돌릴 수 없습니다.";
                break;
            case MenuItem.Remove:
                WarningText.text = "정말 제거하시겠습니까?\n제거하시면 되돌릴 수 없습니다.";
                break;
            case MenuItem.Cancle:
                WarningText.text = "작동을 중지 하시겠습니까?\n시설에 넣어 둔 아이템은 사라집니다.";
                break;
            case MenuItem.Sleep:
                WarningText.text = "전원이 켜져있는 시설은\n잠자는 동안 괴물의 공격을 받게 됩니다.";
                break;
            default:
                WarningText.text = "정말 실행하시겠습니까?\n실행하시면 되돌릴 수 없습니다.";
                break;
        }

        isPopupActive = false;
        isWarningPopupActive = true;
        IMenu_bg.SetActive(false);
        WarningPopup.SetActive(true);
    }

    void CloseWarningPopup()
    {
        isPopupActive = true;
        isWarningPopupActive = false;
        IMenu_bg.SetActive(true);
        WarningPopup.SetActive(false);
    }

    public void ClearMenu()
    {
        MenuList.Clear();
        NameText.text = "미확인 대상";
        ExpText.text = "아직 밝혀지지 않은 대상입니다.";
    }

    public void AddMenu(MenuItem mi)
    {
        MenuList.Add(mi);
    }

    void SelectMenu()
    {
        CloseWindow();

        switch (targetType)
        {
            case "Plant":
                targetObject.GetComponent<Plant>().SelectMenu(MenuList[selectedIndex]);
                break;

            case "Facility":
                targetObject.GetComponent<Facility>().SelectMenu(MenuList[selectedIndex]);
                break;

            case "Bulb":
                targetObject.GetComponent<Bulb>().SelectMenu(MenuList[selectedIndex]);
                break;

            case "Nest":
                targetObject.GetComponent<Nest>().SelectMenu(MenuList[selectedIndex]);
                break;

            case "NyxCollector":
                targetObject.GetComponent<NyxCollector>().SelectMenu(MenuList[selectedIndex]);
                break;

            case "Inventory":
                targetObject.GetComponent<Inventory>().SelectMenu(MenuList[selectedIndex]);
                break;
        }
    }
}
