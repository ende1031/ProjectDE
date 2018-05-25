using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderWindow : MonoBehaviour
{
    public class GrinderItemInfo
    {
        public GrinderItemInfo(int nxN, Inventory.Item r1 = 0, int n1 = 0, Inventory.Item r2 = 0, int n2 = 0)
        {
            nyxNum = nxN;
            result[0] = r1;
            result[1] = r2;
            num[0] = n1;
            num[1] = n2;

            resultCount = 2;
            if (n2 == 0)
            {
                resultCount = 1;
            }
            if (n1 == 0)
            {
                resultCount = 0;
            }
        }

        public int resultCount;
        public Inventory.Item[] result = new Inventory.Item[2];
        public int[] num = new int[2];
        public int nyxNum;
    }

    Inventory inventory;
    GameObject Player;
    GameObject Facility;

    GameObject Grinder_BG;
    GameObject BigItem;
    Text BigItemNum;
    GameObject PickItem;
    GameObject PickNumber;
    GameObject[] ResultItem = new GameObject[2];
    GameObject[] ResultItemNum = new GameObject[2];
    GameObject ResultNyxNum;
    Text ButtonText;

    Animator animaitor;

    bool isPopupActive = false;
    bool isUsingGrinder = false;
    float openTimer = 0;

    Inventory.Item selectedItem;
    int selectedItemNum = 1;
    Inventory.Item[] resultItem = new Inventory.Item[2];
    int[] resultItemNum = new int[2] { 0, 0};
    int resultNyxNum = 0;
    int resultItemCount = 0;
    bool selectNull = true;

    bool isSelectingNumber = false;

    Dictionary<Inventory.Item, GrinderItemInfo> GrinderItemDictionary = new Dictionary<Inventory.Item, GrinderItemInfo>();

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        GrinderItemDictionary[Inventory.Item.Food] = new GrinderItemInfo(20, Inventory.Item.Water, 1);
        GrinderItemDictionary[Inventory.Item.Oxygen] = new GrinderItemInfo(50, Inventory.Item.Water, 1);
        GrinderItemDictionary[Inventory.Item.Battery] = new GrinderItemInfo(60, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Stick] = new GrinderItemInfo(70, Inventory.Item.Mass, 1, Inventory.Item.Water, 1);
        GrinderItemDictionary[Inventory.Item.Board] = new GrinderItemInfo(10, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Hose] = new GrinderItemInfo(55, Inventory.Item.Board, 1);
        GrinderItemDictionary[Inventory.Item.Mass] = new GrinderItemInfo(10);
        GrinderItemDictionary[Inventory.Item.Thorn] = new GrinderItemInfo(85, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Facility01] = new GrinderItemInfo(41, Inventory.Item.Board, 1, Inventory.Item.Stick, 1);
        GrinderItemDictionary[Inventory.Item.Trap01] = new GrinderItemInfo(10, Inventory.Item.Thorn, 1);
        GrinderItemDictionary[Inventory.Item.Heart] = new GrinderItemInfo(80, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Bulb01] = new GrinderItemInfo(200, Inventory.Item.Stick, 1, Inventory.Item.Hose, 1);
        GrinderItemDictionary[Inventory.Item.StickSeed] = new GrinderItemInfo(50, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.BoardSeed] = new GrinderItemInfo(6, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.ThornSeed] = new GrinderItemInfo(77, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Tumor] = new GrinderItemInfo(800, Inventory.Item.Water, 1);
        GrinderItemDictionary[Inventory.Item.TumorSeed] = new GrinderItemInfo(99, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Grinder01] = new GrinderItemInfo(55, Inventory.Item.Sawtooth, 1, Inventory.Item.Stick, 1);
        GrinderItemDictionary[Inventory.Item.SuppliedBattery] = new GrinderItemInfo(111, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.SuppliedFood] = new GrinderItemInfo(155);
        GrinderItemDictionary[Inventory.Item.Water] = new GrinderItemInfo(13);
        GrinderItemDictionary[Inventory.Item.NyxCollector01] = new GrinderItemInfo(31, Inventory.Item.Stick, 1, Inventory.Item.Board, 1);
        GrinderItemDictionary[Inventory.Item.Nyx] = new GrinderItemInfo(1);
        GrinderItemDictionary[Inventory.Item.Fruit] = new GrinderItemInfo(10, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.FruitSeed] = new GrinderItemInfo(24, Inventory.Item.Mass, 1);
        GrinderItemDictionary[Inventory.Item.Sawtooth] = new GrinderItemInfo(10, Inventory.Item.Thorn, 1);
    }

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        Grinder_BG = transform.Find("Grinder_BG").gameObject;
        animaitor = GetComponent<Animator>();
        SetWindowObject();
        SetDictionary();
    }

    void SetWindowObject()
    {
        BigItem = Grinder_BG.transform.Find("BigItem").gameObject;
        BigItemNum = BigItem.transform.Find("Num").GetComponent<Text>();
        PickItem = Grinder_BG.transform.Find("PickItem").gameObject;
        PickNumber = Grinder_BG.transform.Find("PickNumber").gameObject;
        ButtonText = Grinder_BG.transform.Find("ButtonText").GetComponent<Text>();

        for (int i = 0; i < 2; i++)
        {
            ResultItem[i] = Grinder_BG.transform.Find("ResultItem" + (i + 1)).gameObject;
            ResultItemNum[i] = ResultItem[i].transform.Find("Num").gameObject;
        }
        ResultNyxNum = Grinder_BG.transform.Find("ResultNyx").transform.Find("Num").gameObject;
    }

    public void RefreshWindow()
    {
        if(selectNull == true)
        {
            BigItem.SetActive(false);
            resultItemCount = 0;
            selectedItemNum = 1;
            ResultNyxNum.SetActive(false);
        }
        else
        {
            BigItem.SetActive(true);
            ResultNyxNum.SetActive(true);
            BigItem.GetComponent<Image>().sprite = inventory.itemDictionary[selectedItem].sprite;

            resultItemCount = GrinderItemDictionary[selectedItem].resultCount;
            for(int i = 0; i < 2; i++)
            {
                resultItemNum[i] = 0;
            }
            for(int i = 0; i < resultItemCount; i++)
            {
                resultItem[i] = GrinderItemDictionary[selectedItem].result[i];
                resultItemNum[i] = GrinderItemDictionary[selectedItem].num[i] * selectedItemNum;
            }
            resultNyxNum = GrinderItemDictionary[selectedItem].nyxNum * selectedItemNum;
        }

        if(selectedItemNum >= 10)
        {
            BigItemNum.text = selectedItemNum.ToString();
        }
        else
        {
            BigItemNum.text = "0" + selectedItemNum;
        }
        for(int i =0; i<2; i++)
        {
            ResultItem[i].SetActive(true);
            ResultItemNum[i].SetActive(true);
        }
        int c = 1;
        while(c >= resultItemCount)
        {
            ResultItem[c].SetActive(false);
            ResultItemNum[c].SetActive(false);
            c--;
        }

        for(int i =0; i< resultItemCount; i++)
        {

        }

        for(int i = 0; i < resultItemCount; i++)
        {
            ResultItem[i].GetComponent<Image>().sprite = inventory.itemDictionary[resultItem[i]].sprite;
            if (resultItemNum[i] >= 10)
            {
                ResultItemNum[i].GetComponent<Text>().text = resultItemNum[i].ToString();
            }
            else
            {
                ResultItemNum[i].GetComponent<Text>().text = "0" + resultItemNum[i];
            }
            ResultNyxNum.GetComponent<Text>().text = resultNyxNum.ToString();
        }
        if(isSelectingNumber == true)
        {
            ButtonText.text = "C : 분해 시작";
            PickItem.SetActive(false);
            PickNumber.SetActive(true);
        }
        else
        {
            ButtonText.text = "C : 아이템 선택";
            PickItem.SetActive(true);
            PickNumber.SetActive(false);
        }
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
                    OpenWindow(Facility);
                }
                else if (Input.GetKeyUp(KeyCode.C))
                {
                    if(selectNull == false)
                    {
                        inventory.DeleteItem(selectedItem, selectedItemNum);
                        Facility.GetComponent<FacilityBalloon>().GrindItem(selectedItemNum * 3, resultNyxNum, resultItem[0], resultItemNum[0], resultItem[1], resultItemNum[1]);
                        CloseWindow();
                    }
                }
            }
        }
    }

    public void SelectItem(bool isNull, Inventory.Item it = 0)
    {
        selectNull = isNull;
        selectedItem = it;
        isPopupActive = true;
        isSelectingNumber = true;
        selectedItemNum = 1;
        RefreshWindow();
    }

    public void InventoryMove(bool isNull, Inventory.Item it = 0)
    {
        selectNull = isNull;
        selectedItem = it;
        selectedItemNum = 1;
        RefreshWindow();
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) && selectedItemNum < inventory.CountOfItem(selectedItem) && selectedItemNum < 10)
        {
            selectedItemNum++;
            RefreshWindow();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && selectedItemNum > 1)
        {
            selectedItemNum--;
            RefreshWindow();
        }
    }

    public void OpenWindow(GameObject fac)
    {
        Facility = fac;
        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(false);
        selectedItemNum = 1;
        openTimer = 0;
        Grinder_BG.SetActive(true);
        animaitor.SetBool("isOpen", true);
        isSelectingNumber = false;
        isPopupActive = false;
        inventory.OpenInventory(true);
        RefreshWindow();
        isUsingGrinder = true;
    }

    public void CloseWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(true);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        openTimer = 0;
        animaitor.SetBool("isOpen", false);
        isUsingGrinder = false;
    }

    public bool GetUsingGrinder()
    {
        return isUsingGrinder;
    }
}