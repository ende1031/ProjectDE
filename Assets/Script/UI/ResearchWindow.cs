using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoBehaviour
{
    Inventory inventory;
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

    GameObject ResultBG;
    GameObject ResultBigItem;
    GameObject ExplainText;
    GameObject[] Content = new GameObject[3];
    GameObject[] ContentIcon = new GameObject[3];
    GameObject[] ContentText = new GameObject[3];
    GameObject[] ContentFacIcon = new GameObject[3];

    public Sprite YellowButton;
    public Sprite RedButton;
    public Sprite BlueButton;

    Animator animaitor;

    bool isResearchActive = false;
    bool isResultActive = false;
    float openTimer = 0;

    int selectedIndex = 0;
    bool completeResearch = false;

    ResearchItem[] itemArray = new ResearchItem[16];
    Dictionary<global::Inventory.Item, ResultContent> contentDictionary = new Dictionary<global::Inventory.Item, ResultContent>();

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

        public void SetResultItem(int num, global::Inventory.Item c1, global::Inventory.Item c2 = 0, global::Inventory.Item c3 = 0)
        {
            resultNum = num;
            resultItem[0] = c1;
            resultItem[1] = c2;
            resultItem[2] = c3;
        }

        public global::Inventory.Item item;
        public Sprite itemSp;
        public int maxNum;
        public int putNum;
        public bool isKnown;
        public string expText;
        public int resultNum;
        public global::Inventory.Item[] resultItem = new global::Inventory.Item[3];
    }

    public class ResultContent
    {
        public ResultContent(Sprite isp, string t, Sprite fsp)
        {
            itemSp = isp;
            expText = t;
            facilitySp = fsp;
        }

        public ResultContent(Sprite isp, string t)
        {
            itemSp = isp;
            expText = t;
            facilitySp = null;
        }

        public Sprite itemSp;
        public string expText;
        public Sprite facilitySp;
    }

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        animaitor = GetComponent<Animator>();
        ResearchBG = transform.Find("ResearchBG").gameObject;
        ResultBG = transform.Find("ResultBG").gameObject;

        SetWindowObject();
        SetResultContent();
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

        ResultBigItem = ResultBG.transform.Find("BigItem").gameObject;
        ExplainText = ResultBG.transform.Find("ExplainText").gameObject;
        for (int i = 0; i < 3; i++)
        {
            Content[i] = ResultBG.transform.Find("Content" + (i + 1)).gameObject;
            ContentIcon[i] = Content[i].transform.Find("Item").gameObject;
            ContentText[i] = Content[i].transform.Find("Text").gameObject;
            ContentFacIcon[i] = Content[i].transform.Find("FacilityIcon").gameObject;
        }
    }

    void SetWindowItem()
    {
        itemArray[0] = new ResearchItem(global::Inventory.Item.Mass, inventory.MassSp, 5, true);
        itemArray[0].expText = "덩어리는 덩어리.\n쨔쟌~ 워크벤치를 만들 수 있게 되었다!";
        itemArray[0].SetResultItem(1, global::Inventory.Item.Facility01);

        itemArray[1] = new ResearchItem(global::Inventory.Item.Mass, inventory.MassSp, 5, true);
        itemArray[1].expText = "덩어리를 연구해서 종양씨앗을 만들어낼 수 있다는 놀라운 사실을 알게 되었다. 이럴수가.";
        itemArray[1].SetResultItem(1, global::Inventory.Item.TumorSeed);

        itemArray[2] = new ResearchItem(global::Inventory.Item.Tumor, inventory.TumorSp, 3, true);
        itemArray[2].expText = "종양을 연구했다. 만족스럽다.";
        itemArray[2].SetResultItem(2, global::Inventory.Item.Food, global::Inventory.Item.Bulb01);

        itemArray[3] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, true);
        itemArray[3].expText = "대나무는 대나무다.";
        itemArray[3].SetResultItem(1, global::Inventory.Item.StickSeed);

        itemArray[4] = new ResearchItem(global::Inventory.Item.Board, inventory.BoardSp, 20, true);
        itemArray[4].expText = "이 식물을 연구해본 결과 식용으로 쓰기에는 부적합하다는 사실을 알게 됐다. (불쾌한 표정)";
        itemArray[4].SetResultItem(1, global::Inventory.Item.BoardSeed);

        itemArray[5] = new ResearchItem(global::Inventory.Item.Thorn, inventory.ThornSp, 20, true);
        itemArray[5].expText = "연구 결과로 알게 된 사실에 따르면 놀랍게도 이 식물의 초기 설정은 선인장이었다고 한다.\n어쩌다 이렇게 변했는지 담당 디자이너를 만나봐야겠다.";
        itemArray[5].SetResultItem(1, global::Inventory.Item.ThornSeed, global::Inventory.Item.Trap01);

        itemArray[6] = new ResearchItem(global::Inventory.Item.Heart, inventory.HeartSp, 5, true);
        itemArray[6].expText = "심장은 두근두근\n지난달 발표된 캘리포니아 주 스탠포드 대학의 연구결과에 따르면 천체우주역학적 이유에 따라 우리가 바라보는 절대우주는 팽창과 수축을 반복하며 무한히 연쇄한다고 한다. 즉, 괴물의 심장으로 배터리를 만들 수 있다는 뜻이다.";
        itemArray[6].SetResultItem(1, global::Inventory.Item.Battery);

        itemArray[7] = new ResearchItem(global::Inventory.Item.Hose, inventory.HoseSp, 10, true);
        itemArray[7].expText = "호스를 연구하면 분해기를 만들 수 있다. 아마도.";
        itemArray[7].SetResultItem(1, global::Inventory.Item.Facility01);

        itemArray[8] = new ResearchItem(global::Inventory.Item.Mass, inventory.MassSp, 20, true);
        itemArray[8].expText = "덩어리를 또 연구하기로 한지 3개월이 지났다. 드디어 빛이 보인다! 연구 결과로 알게 된 사실은 이 다음 연구도 덩어리 연구라는 것이다.";
        itemArray[8].SetResultItem(1, global::Inventory.Item.Facility01);

        itemArray[9] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
        itemArray[10] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
        itemArray[11] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
        itemArray[12] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
        itemArray[13] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
        itemArray[14] = new ResearchItem(global::Inventory.Item.Stick, inventory.MassSp, 20, false);
        itemArray[15] = new ResearchItem(global::Inventory.Item.Stick, inventory.StickSp, 20, false);
    }

    void SetResultContent()
    {
        contentDictionary[global::Inventory.Item.Facility01] = new ResultContent(inventory.Facility01Sp, "연금술의 오의를 깨달아서 현자의 돌 없이 워크벤치를 연성할 수 있게 됐다.");
        contentDictionary[global::Inventory.Item.TumorSeed] = new ResultContent(inventory.TumorSeedSp, "피의 대가를 치뤄서 마계로부터 종양씨앗을 소환할 수 있게 됐다.");
        contentDictionary[global::Inventory.Item.Food] = new ResultContent(inventory.FoodSp, "선대 요리왕의 혼이 깃들어서 식량을 만들 수 있게 됐다.", inventory.EscapePodSp);
        contentDictionary[global::Inventory.Item.Bulb01] = new ResultContent(inventory.Bulb01Sp, "희생은 컸다. 나는 평생에 걸친 연구 끝에 스스로 빛을 내는 물건을 만들 수 있게 됐다.");
        contentDictionary[global::Inventory.Item.StickSeed] = new ResultContent(inventory.StickSeedSp, "생명의 나무 세피로트의 가호를 받아서 대나무 모종을 생산할 수 있게 됐다.");
        contentDictionary[global::Inventory.Item.BoardSeed] = new ResultContent(inventory.BoardSeedSp, "고대문명 속 제국의 고서로부터 얻은 지식을 통해서 판자식물을 생산하는 방법을 깨달았다.");
        contentDictionary[global::Inventory.Item.ThornSeed] = new ResultContent(inventory.ThornSeedSp, "금지된 술법으로 불러낸 마계의 요마와의 계약을 통해 선인장 모종을 키우는 법을 알게 됐다.");
        contentDictionary[global::Inventory.Item.Trap01] = new ResultContent(inventory.BatterySp, "별이 그려진 신비한 구슬 7개를 모아 기도하자 천계의 용이 나타나 덫을 만드는 방법을 알려줬다.");
        contentDictionary[global::Inventory.Item.Battery] = new ResultContent(inventory.BatterySp, "간절한 염원을 담아 기도하자 대지의 어머니 크리스탈이 빛을 내며 배터리를 만드는 방법을 알려줬다.");
    }

    void Update ()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }

        // 연구 윈도우
        if (isResearchActive == true)
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

        // 연구결과 윈도우
        if(isResultActive == true)
        {
            if (openTimer <= 0.3f)
            {
                openTimer += Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
                {
                    CloseResultWindow();
                }
            }
        }
    }

    void MoveCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedIndex > 0)
        {
            selectedIndex--;
            RefreshWindow();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && selectedIndex < 15)
        {
            selectedIndex++;
            RefreshWindow();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedIndex > 6)
        {
            selectedIndex -= 7;
            RefreshWindow();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && selectedIndex < 9)
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
            Button.SetActive(false);
            completeResearch = false;
        }
        else
        {
            BigItem.SetActive(true);
            Button.SetActive(true);
            BigItem.GetComponent<Image>().sprite = itemArray[selectedIndex].itemSp;
            if (itemArray[selectedIndex].putNum < itemArray[selectedIndex].maxNum)
            {
                completeResearch = false;
                if (inventory.HasItem(itemArray[selectedIndex].item) == true)
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
        isResearchActive = true;
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
        isResearchActive = false;
        selectedIndex = 0;
        openTimer = 0;
        MoveCursor();
        animaitor.SetBool("isOpen", false);
    }

    void OpenResultWindow()
    {
        ResultBG.SetActive(true);
        ResearchBG.SetActive(false);
        isResearchActive = false;
        isResultActive = true;
        openTimer = 0;

        ResultBigItem.GetComponent<Image>().sprite = itemArray[selectedIndex].itemSp;
        ExplainText.GetComponent<Text>().text = itemArray[selectedIndex].expText;


        for(int i = 0; i < 3; i++)
        {
            if( i < itemArray[selectedIndex].resultNum)
            {
                Content[i].SetActive(true);
                ContentIcon[i].GetComponent<Image>().sprite = contentDictionary[itemArray[selectedIndex].resultItem[i]].itemSp;
                ContentText[i].GetComponent<Text>().text = contentDictionary[itemArray[selectedIndex].resultItem[i]].expText;
                if(contentDictionary[itemArray[selectedIndex].resultItem[i]].facilitySp == null)
                {
                    ContentFacIcon[i].GetComponent<Image>().sprite = inventory.Facility01Sp;
                }
                else
                {
                    ContentFacIcon[i].GetComponent<Image>().sprite = contentDictionary[itemArray[selectedIndex].resultItem[i]].facilitySp;
                }
            }
            else
            {
                Content[i].SetActive(false);
            }
        }
    }

    void CloseResultWindow()
    {
        ResultBG.SetActive(false);
        ResearchBG.SetActive(true);
        isResearchActive = true;
        isResultActive = false;
        //openTimer = 0;
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
            if(inventory.HasItem(itemArray[selectedIndex].item) == true)
            {
                inventory.DeleteItem(itemArray[selectedIndex].item);
                itemArray[selectedIndex].InputItem();
                RefreshWindow();
            }
        }
        else if(completeResearch == true)
        {
            OpenResultWindow();
        }
    }
}
