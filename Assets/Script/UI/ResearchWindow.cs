using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoBehaviour
{
    GameObject Inventory;
    GameObject Player;

    GameObject ResearchBG;
    GameObject[] Item = new GameObject[16];
    GameObject[] ItemIcon = new GameObject[16];
    GameObject[] ItemIconBack = new GameObject[16];
    GameObject[] NoItem = new GameObject[16];
    GameObject[] ItemCount = new GameObject[16];
    GameObject ItemCursor;
    GameObject Button;
    GameObject ButtonText;
    GameObject BigItem;

    public Sprite YellowButton;
    public Sprite RedButton;
    public Sprite BlueButton;

    Animator animaitor;

    bool isPopupActive = false;
    float openTimer = 0;

    int selectedIndex = 0;
    bool completeResearch = false;

    ResearchItem[] itemArray = new ResearchItem[16];

    public class ResearchItem
    {
        public ResearchItem(global::Inventory.Item i, Sprite sp, int m, bool k) //기본
        {
            item = i;
            itemSp = sp;
            maxNum = m;
            isKnown = k;
            putNum = 0;
        }

        public void SetKnown(bool k)
        {
            isKnown = k;
        }

        public void InputItem()
        {
            if(putNum < maxNum)
            {
                putNum++;
            }
        }

        public global::Inventory.Item item;
        public Sprite itemSp;
        public int maxNum;
        public int putNum;
        public bool isKnown;
    }

    void Start ()
    {
        Inventory = GameObject.Find("Inventory");
        animaitor = GetComponent<Animator>();
        ResearchBG = transform.Find("ResearchBG").gameObject;

        SetWindowObject();
        SetWindowItem();
    }

    void SetWindowObject()
    {
        GameObject ItemList = ResearchBG.transform.Find("ItemList").gameObject;
        for(int i = 0; i < 16; i++)
        {
            Item[i] = ItemList.transform.Find("Item" + (i + 1)).gameObject;
            NoItem[i] = Item[i].transform.Find("NoItem").gameObject;
            ItemIcon[i] = Item[i].transform.Find("Item").gameObject;
            ItemIconBack[i] = Item[i].transform.Find("Item_back").gameObject;
            ItemCount[i] = Item[i].transform.Find("Text").gameObject;
        }
        ItemCursor = ItemList.transform.Find("Cursor").gameObject;

        GameObject BottomWindow = ResearchBG.transform.Find("BottomWindow").gameObject;
        Button = BottomWindow.transform.Find("Button").gameObject;
        ButtonText = Button.transform.Find("Text").gameObject;
        BigItem = BottomWindow.transform.Find("BigItem").gameObject;
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
                    InputItem();
                }
            }
        }
    }

    void SetWindowItem()
    {
        itemArray[0] = new ResearchItem(global::Inventory.Item.Mass, Inventory.GetComponent<Inventory>().MassSp, 10, true);
        itemArray[1] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, true);
        itemArray[2] = new ResearchItem(global::Inventory.Item.Board, Inventory.GetComponent<Inventory>().BoardSp, 20, true);
        itemArray[3] = new ResearchItem(global::Inventory.Item.Thorn, Inventory.GetComponent<Inventory>().ThornSp, 20, true);
        itemArray[4] = new ResearchItem(global::Inventory.Item.Hose, Inventory.GetComponent<Inventory>().HoseSp, 20, true);
        itemArray[5] = new ResearchItem(global::Inventory.Item.Heart, Inventory.GetComponent<Inventory>().HeartSp, 20, true);
        itemArray[6] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, true);
        itemArray[7] = new ResearchItem(global::Inventory.Item.Mass, Inventory.GetComponent<Inventory>().MassSp, 30, true);
        itemArray[8] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, true);
        itemArray[9] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
        itemArray[10] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
        itemArray[11] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
        itemArray[12] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
        itemArray[13] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
        itemArray[14] = new ResearchItem(global::Inventory.Item.Mass, Inventory.GetComponent<Inventory>().MassSp, 20, false);
        itemArray[15] = new ResearchItem(global::Inventory.Item.Stick, Inventory.GetComponent<Inventory>().StickSp, 20, false);
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) && selectedIndex > 0)
        {
            selectedIndex--;
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && selectedIndex < 15)
        {
            selectedIndex++;
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && selectedIndex > 6)
        {
            selectedIndex -= 7;
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && selectedIndex < 9)
        {
            selectedIndex += 7;
            RefreshWindow();
        }

        ItemCursor.transform.position = Item[selectedIndex].transform.position;
    }

    void RefreshWindow()
    {
        for (int i = 0; i < 16; i++)
        {
            if(itemArray[i].isKnown == false)
            {
                NoItem[i].SetActive(true);
                ItemIcon[i].SetActive(false);
                ItemIconBack[i].SetActive(false);
                ItemCount[i].SetActive(false);
            }
            else
            {
                NoItem[i].SetActive(false);
                ItemIcon[i].SetActive(true);
                ItemIconBack[i].SetActive(true);
                ItemCount[i].SetActive(true);
                ItemIcon[i].GetComponent<Image>().sprite = itemArray[i].itemSp;
                ItemIconBack[i].GetComponent<Image>().sprite = itemArray[i].itemSp;
                if (itemArray[i].putNum < 10)
                {
                    ItemCount[i].GetComponent<Text>().text = "0" + itemArray[i].putNum + "/" + itemArray[i].maxNum;
                }
                else
                {
                    ItemCount[i].GetComponent<Text>().text = itemArray[i].putNum + "/" + itemArray[i].maxNum;
                }
                ItemIcon[i].GetComponent<Image>().fillAmount = (float)itemArray[i].putNum / (float)itemArray[i].maxNum;
            }
        }

        if (itemArray[selectedIndex].isKnown == false)
        {
            BigItem.SetActive(false);
            Button.GetComponent<Image>().sprite = RedButton;
            ButtonText.GetComponent<Text>().text = "연구 불가";
            completeResearch = false;
        }
        else
        {
            BigItem.SetActive(true);
            BigItem.GetComponent<Image>().sprite = itemArray[selectedIndex].itemSp;
            if (itemArray[selectedIndex].putNum < itemArray[selectedIndex].maxNum)
            {
                completeResearch = false;
                if (Inventory.GetComponent<Inventory>().HasItem(itemArray[selectedIndex].item) == true)
                {
                    Button.GetComponent<Image>().sprite = YellowButton;
                    ButtonText.GetComponent<Text>().text = "C : 아이템 넣기";
                }
                else
                {
                    Button.GetComponent<Image>().sprite = RedButton;
                    ButtonText.GetComponent<Text>().text = "아이템 부족";
                }
            }
            else
            {
                completeResearch = true;
                Button.GetComponent<Image>().sprite = BlueButton;
                ButtonText.GetComponent<Text>().text = "C : 연구 결과 확인";
            }
        }
    }

    public void OpenWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(false);
        isPopupActive = true;
        selectedIndex = 0;
        openTimer = 0;
        RefreshWindow();
        ResearchBG.SetActive(true);
        animaitor.SetBool("isOpen", true);
    }

    public void CloseWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(true);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        selectedIndex = 0;
        openTimer = 0;
        MoveCursor();
        animaitor.SetBool("isOpen", false);
    }

    public void DiscoverNewResearch(int num)
    {
        itemArray[num].SetKnown(true);
        RefreshWindow();
    }

    void InputItem()
    {
        if(completeResearch == false && itemArray[selectedIndex].isKnown == true)
        {
            if(Inventory.GetComponent<Inventory>().HasItem(itemArray[selectedIndex].item) == true)
            {
                Inventory.GetComponent<Inventory>().DeleteItem(itemArray[selectedIndex].item);
                itemArray[selectedIndex].InputItem();
                RefreshWindow();
            }
        }
    }
}
