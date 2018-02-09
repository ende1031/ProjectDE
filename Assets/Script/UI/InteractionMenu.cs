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
        Tumor
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

    Inventory inventory;
    GameObject Player;

    GameObject IMenu_bg;
    GameObject Cursor;
    Image ItemImage;
    Text ButtonText;
    GameObject IconGroup;
    GameObject[] MenuIcon = new GameObject[7];

    List<MenuItem> MenuList = new List<MenuItem>();
    Dictionary<MenuItem, MenuItemInfo> MenuDictionary = new Dictionary<MenuItem, MenuItemInfo>();

    int selectedIndex = 0;

    bool isPopupActive = false;
    float openTimer = 0;

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        IMenu_bg = transform.Find("IMenu_bg").gameObject;
        Cursor = IMenu_bg.transform.Find("Cursor").gameObject;
        ItemImage = IMenu_bg.transform.Find("ItemImage").gameObject.GetComponent<Image>();
        ButtonText = IMenu_bg.transform.Find("ButtonText").gameObject.GetComponent<Text>();
        IconGroup = IMenu_bg.transform.Find("IconGroup").gameObject;
        for(int i =0; i < 7; i++)
        {
            MenuIcon[i] = IconGroup.transform.Find("Icon" + (i + 1)).gameObject;
        }
        SetDictionary();
    }

    void SetDictionary()
    {
        MenuDictionary[MenuItem.Battery] = new MenuItemInfo(BatterySp, "배터리 사용");
        MenuDictionary[MenuItem.Cancle] = new MenuItemInfo(CancleSp, "제작 취소");
        MenuDictionary[MenuItem.Dump] = new MenuItemInfo(DumpSp, "버리기");
        MenuDictionary[MenuItem.Food] = new MenuItemInfo(FoodSp, "식품 섭취");
        MenuDictionary[MenuItem.Gather] = new MenuItemInfo(GatherSp, "아이템 채집");
        MenuDictionary[MenuItem.Install] = new MenuItemInfo(InstallSp, "시설 설치");
        MenuDictionary[MenuItem.Make] = new MenuItemInfo(MakeSp, "아이템 제작");
        MenuDictionary[MenuItem.Off] = new MenuItemInfo(OffSp, "전원 끄기");
        MenuDictionary[MenuItem.Oxygen] = new MenuItemInfo(OxygenSp, "산소 충전");
        MenuDictionary[MenuItem.Plant] = new MenuItemInfo(PlantSp, "식물 심기");
        MenuDictionary[MenuItem.Remove] = new MenuItemInfo(RemoveSp, "제거");
        MenuDictionary[MenuItem.Research] = new MenuItemInfo(ResearchSp, "연구");
        MenuDictionary[MenuItem.Sleep] = new MenuItemInfo(SleepSp, "잠자기");
        MenuDictionary[MenuItem.Tumor] = new MenuItemInfo(TumorSp, "종양 심기");
    }


    void Update ()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }

        if (isPopupActive == true)
        {
            if (openTimer <= 0.3f)
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

                }
            }
        }
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (selectedIndex == 0)
            {
                selectedIndex = MenuList.Count - 1;
            }
            else
            {
                selectedIndex--;
            }
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (selectedIndex == MenuList.Count - 1)
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex++;
            }
            RefreshWindow();
        }
    }

    void RefreshWindow()
    {
        float circleRadius = 135.0f;

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
        Color tempColor = MenuIcon[0].GetComponent<Image>().color;
        tempColor.a = 1.0f;
        MenuIcon[2].GetComponent<Image>().color = tempColor;
        tempColor.a = 0.5f;
        MenuIcon[1].GetComponent<Image>().color = tempColor;
        MenuIcon[3].GetComponent<Image>().color = tempColor;
        tempColor.a = 0.25f;
        MenuIcon[0].GetComponent<Image>().color = tempColor;
        MenuIcon[4].GetComponent<Image>().color = tempColor;
        tempColor.a = 0;
        MenuIcon[5].GetComponent<Image>().color = tempColor;
        MenuIcon[6].GetComponent<Image>().color = tempColor;

        //버튼 텍스트 설정
        ButtonText.text = MenuDictionary[MenuList[selectedIndex]].buttonText;
    }

    public void OpenMenu()
    {
        if(MenuList.Count == 0)
        {
            MenuList.Add(MenuItem.Battery);
        }

        while(MenuList.Count < 5)
        {
            int temp = MenuList.Count;
            for (int i = 0; i < temp; i++)
            {
                MenuList.Add(MenuList[i]);
            }
        }

        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(false);
        IMenu_bg.SetActive(true);
        isPopupActive = true;
        selectedIndex = 0;
        openTimer = 0;
        RefreshWindow();
    }

    public void CloseWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(true);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        selectedIndex = 0;
        openTimer = 0;
        IMenu_bg.SetActive(false);
    }

    public void ClearMenu()
    {
        MenuList.Clear();
    }

    public void AddMenu(MenuItem mi)
    {
        MenuList.Add(mi);
    }

    public MenuItem SelectMenu()
    {
        IMenu_bg.SetActive(false);
        isPopupActive = false;

        return MenuList[selectedIndex];
    }
}
