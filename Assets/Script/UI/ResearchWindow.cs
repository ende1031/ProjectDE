using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoBehaviour
{
    Inventory inventory;
    PopupWindow popupWindow;
    GameObject Player;
    NyxUI nyxUI;

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
    //GameObject[] ContentFacIcon = new GameObject[3];

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
    Dictionary<string, ResultContent> contentDictionary = new Dictionary<string, ResultContent>();
    //Dictionary<Inventory.Item, int> ResearchNumberDictionary = new Dictionary<Inventory.Item, int>();

    public class ResearchItem
    {
        public ResearchItem(Inventory.Item i, int m, string t)
        {
            item = i;
            maxNum = m;
            expText = t;
            isKnown = false;
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

        public void SetResultItem(int num, string c1, string c2 = "", string c3 = "")
        {
            resultNum = num;
            resultItem[0] = c1;
            resultItem[1] = c2;
            resultItem[2] = c3;
        }

        public void AddNextResearch(int n)
        {
            nextResearch.Add(n);
        }

        public Inventory.Item item;
        public int maxNum;
        public int putNum;
        public bool isKnown;
        public string expText;
        public int resultNum;
        public string[] resultItem = new string[3];
        public List<int> nextResearch = new List<int>();
    }

    public class ResultContent
    {
        public ResultContent(string t, Sprite sp)
        {
            expText = t;
            ContentSp = sp;
            isOpenItem = false;
        }

        public ResultContent(string t, Sprite sp, Inventory.Item it)
        {
            expText = t;
            ContentSp = sp;
            item = it;
            isOpenItem = true;
        }

        public string expText;
        public Sprite ContentSp;
        public bool isOpenItem;
        public Inventory.Item item;
    }

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        animaitor = GetComponent<Animator>();
        ResearchBG = transform.Find("ResearchBG").gameObject;
        ResultBG = transform.Find("ResultBG").gameObject;

        SetWindowObject();
        SetResultContent();
        SetWindowItem();

        // 첫 연구 오픈
        DiscoverNewResearch(0);
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
            //ContentFacIcon[i] = Content[i].transform.Find("FacilityIcon").gameObject;
        }
    }

    void SetWindowItem()
    {
        //덩어리 3개
        itemArray[0] = new ResearchItem(Inventory.Item.Mass, 3, "연구를 통해 닉스입자를 발견했다.\n연구 성과로 닉스입자 100개를 획득했다! 닉스입자는 화면 상단 UI에서 확인할 수 있다.");
        itemArray[0].SetResultItem(1, "Nyx");
        itemArray[0].AddNextResearch(1);
        //사과 2개
        itemArray[1] = new ResearchItem(Inventory.Item.Fruit, 2, "이번 연구는 먹을 수 있는 것을 발견했다는 점에 큰 의의가 있다. 구조가 오기 전 까지 무사히 이 행성에서 살아남을 수 있을 것 같다.");
        itemArray[1].SetResultItem(1, "Hunger");
        itemArray[1].AddNextResearch(2);
        //가시 3개
        itemArray[2] = new ResearchItem(Inventory.Item.Thorn, 3, "연구 결과로 알게 된 사실에 따르면 놀랍게도 이 식물의 초기 설정은 선인장이었다고 한다.\n어쩌다 이렇게 변했는지 담당 디자이너의 말을 들어보기로 했다.");
        itemArray[2].SetResultItem(3, "ThornPlant", "Trap01", "capture");
        itemArray[2].AddNextResearch(3);
        //심장 1개
        itemArray[3] = new ResearchItem(Inventory.Item.Heart, 1, "최근 발표된 뉴욕 공학 대학의 연구 결과에 따르면 형이상학적 이유에 따라 우리가 관찰 하는 절대우주는 팽창과 수축을 반복하며 무한히 연쇄한다고 한다. 쓸 말이 생각 안난다.");
        itemArray[3].SetResultItem(1, "Heart");
        itemArray[3].AddNextResearch(4);
        //막대 3개
        itemArray[4] = new ResearchItem(Inventory.Item.Stick, 3, "닉스입자에 대한 연구가 거의 완성되었다. 하지만 아직 부족하다. 다음 연구를 진행해보자.");
        itemArray[4].SetResultItem(1, "StickPlant");
        itemArray[4].AddNextResearch(5);
        //판자 3개
        itemArray[5] = new ResearchItem(Inventory.Item.Board, 3, "드디어 닉스입자를 수집하는 방법을 알게 됐다. 닉스입자를 이용해서 무언가를 만들 수 있을 것 같다.");
        itemArray[5].SetResultItem(2, "BoardPlant", "NyxCollector01");
        itemArray[5].AddNextResearch(6);
        //심장 4개
        itemArray[6] = new ResearchItem(Inventory.Item.Heart, 4, "모든 제작을 탈출포드에서 해야 된다는 것은 지나치게 비효율적이다. 괴식물의 심장을 이용해서 다른 제작 시설을 만들어보기로 했다.");
        itemArray[6].SetResultItem(1, "tempFacility");
        itemArray[6].AddNextResearch(7);
        //막대 10개
        itemArray[7] = new ResearchItem(Inventory.Item.Stick, 10, "괴물은 빛을 싫어한다. 빛을 내는 시설을 만들 수 있으면 다른 시설들을 안전하게 지킬 수 있을 것이다.");
        itemArray[7].SetResultItem(2, "Hose", "Bulb01");
        itemArray[7].AddNextResearch(8);
        //판자 10개
        itemArray[8] = new ResearchItem(Inventory.Item.Board, 10, "연구를 통해 워크벤치에서 분해기를 만들 수 있게 됐다.\n이제 필요없는 아이템은 분해하도록 하자.");
        itemArray[8].SetResultItem(2, "Sawtooth", "Grinder01");
        itemArray[8].AddNextResearch(9);
        itemArray[8].AddNextResearch(10);
        //덩어리 20개
        itemArray[9] = new ResearchItem(Inventory.Item.Mass, 20, "괴식물의 유전자를 조작해서 먹을 수 있는 안전한 조직을 만들어냈다. 자라는 중인 식물에 심어보자.");
        itemArray[9].SetResultItem(2, "TumorSeed", "SeedingTumor");
        itemArray[9].AddNextResearch(9);
        //막대 20개
        itemArray[10] = new ResearchItem(Inventory.Item.Stick, 20, "집게발 대나무의 유전자를 조작해서 모종을 만들 수 있게 됐다. 괴식물의 모종을 만들면 원하는 위치에 심을 수 있다.");
        itemArray[10].SetResultItem(2, "StickSeed", "SeedingPlant");
        itemArray[10].AddNextResearch(11);
        itemArray[10].AddNextResearch(12);
        itemArray[10].AddNextResearch(13);
        itemArray[10].AddNextResearch(14);
        //판자 20개
        itemArray[11] = new ResearchItem(Inventory.Item.Board, 20, "판자 식물의 유전자를 조작해서 모종을 만들 수 있게 됐다. 괴식물의 모종을 만들면 원하는 위치에 심을 수 있다.");
        itemArray[11].SetResultItem(1, "BoardSeed");
        itemArray[11].AddNextResearch(11);
        //가시 20개
        itemArray[12] = new ResearchItem(Inventory.Item.Thorn, 20, "가시 덩굴의 유전자를 조작해서 모종을 만들 수 있게 됐다. 괴식물의 모종을 만들면 원하는 위치에 심을 수 있다.");
        itemArray[12].SetResultItem(1, "ThornSeed");
        itemArray[12].AddNextResearch(12);
        //사과 20개
        itemArray[13] = new ResearchItem(Inventory.Item.Fruit, 20, "열매 나무의 유전자를 조작해서 모종을 만들 수 있게 됐다. 괴식물의 모종을 만들면 원하는 위치에 심을 수 있다.");
        itemArray[13].SetResultItem(1, "FruitSeed");
        itemArray[13].AddNextResearch(13);
        //호스 30개
        itemArray[14] = new ResearchItem(Inventory.Item.Hose, 30, "참치마요를 먹을 때 닭강정(소)를 같이 주문하면 만족감이 두배가 된다는 사실을 알게 됐다.");
        itemArray[14].AddNextResearch(15);
        //톱날 30개
        itemArray[15] = new ResearchItem(Inventory.Item.Sawtooth, 30, "마지막 연구를 끝냈다. 이 행성은 멸망 이후 독자적인 생태계를 만들어가고 있다. 이렇게 된 원인은 바로 ··· ···");
        itemArray[15].AddNextResearch(15);

        //for(int i = 15; i >= 0; i--)
        //{
        //    ResearchNumberDictionary[itemArray[i].item] = i;
        //}
    }

    void SetResultContent()
    {
        contentDictionary["Nyx"] = new ResultContent("[닉스 입자] 이 행성에만 존재하는 미지의 입자로 추청된다.", inventory.NyxSp);
        contentDictionary["Hunger"] = new ResultContent("[허기 회복] 먹을 수 있는 아이템을 먹으면 허기를 회복할 수 있다.", inventory.FruitSp);
        contentDictionary["ThornPlant"] = new ResultContent("[가시 덩굴] 가시가 자라는 나무이다. 가시는 제작에 쓸 수 있다.", inventory.ThornSp);
        contentDictionary["Trap01"] = new ResultContent("[소형 덫] 소형 덫을 만들 수 있게 됐다.", inventory.Trap01Sp, Inventory.Item.Trap01);
        contentDictionary["capture"] = new ResultContent("[포획] 괴물의 둥지 근처에 덫을 설치하면 괴물을 잡을 수 있다.", inventory.Trap01Sp);
        contentDictionary["Heart"] = new ResultContent("[심장] 작은 괴물의 심장이다. 시설 에너지원으로 쓸 수 있다.", inventory.HeartSp);
        contentDictionary["StickPlant"] = new ResultContent("[집게발 대나무] 대나무를 닮은 괴식물이다. 하루한번 채집할 수 있다.", inventory.StickSp);
        contentDictionary["BoardPlant"] = new ResultContent("[판자 식물] 판자를 닮은 괴식물이다. 제작에 쓸 수 있다.", inventory.BoardSp);
        contentDictionary["NyxCollector01"] = new ResultContent("[닉스입자 수집기] 닉스입자 수집기를 만들 수 있게 됐다.", inventory.NyxCollector01Sp, Inventory.Item.NyxCollector01);
        contentDictionary["tempFacility"] = new ResultContent("[소형 워크벤치] 소형 워크벤치를 만들 수 있게 됐다.", inventory.Facility01Sp, Inventory.Item.Facility01);
        contentDictionary["Hose"] = new ResultContent("[호스] 호스를 만들 수 있게 됐다.", inventory.HoseSp, Inventory.Item.Hose);
        contentDictionary["Bulb01"] = new ResultContent("[간이 전구] 간이 전구를 만들 수 있게 됐다.", inventory.Bulb01Sp, Inventory.Item.Bulb01);
        contentDictionary["Sawtooth"] = new ResultContent("[톱날] 톱날을 만들 수 있게 됐다.", inventory.SawtoothSp, Inventory.Item.Sawtooth);
        contentDictionary["Grinder01"] = new ResultContent("[간이 분해기] 간이 분해기를 만들 수 있게 됐다.", inventory.Grinder01Sp, Inventory.Item.Grinder01);
        contentDictionary["TumorSeed"] = new ResultContent("[종양 씨앗] 종양 씨앗을 만들 수 있게 됐다.", inventory.TumorSeedSp, Inventory.Item.TumorSeed);
        contentDictionary["SeedingTumor"] = new ResultContent("[종양 심기] 자라는중인 특정 식물에는 종양을 심을 수 있다.", inventory.TumorSp);
        contentDictionary["StickSeed"] = new ResultContent("[집게발 대나무 모종] 집게발 대나무 모종을 만들 수 있게 됐다.", inventory.StickSeedSp, Inventory.Item.StickSeed);
        contentDictionary["SeedingPlant"] = new ResultContent("[모종 심기] 원하는 위치에 모종을 심으면 괴식물이 자란다.", inventory.StickSeedSp);
        contentDictionary["BoardSeed"] = new ResultContent("[판자 식물 모종] 판자 식물 모종을 만들 수 있게 됐다.", inventory.BoardSeedSp, Inventory.Item.BoardSeed);
        contentDictionary["ThornSeed"] = new ResultContent("[가시 덩굴 모종] 가시 덩굴 모종을 만들 수 있게 됐다.", inventory.ThornSeedSp, Inventory.Item.ThornSeed);
        contentDictionary["FruitSeed"] = new ResultContent("[열매 나무 모종] 열매 나무 모종을 만들 수 있게 됐다.", inventory.FruitSeedSp, Inventory.Item.FruitSeed);
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
            BigItem.GetComponent<Image>().sprite = inventory.itemDictionary[itemArray[selectedIndex].item].sprite;
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
                Button.GetComponent<Image>().sprite = BlueButton;
                ButtonText.GetComponent<Text>().text = "C : 연구 결과 확인";
                completeResearch = true;

                // 연구 끝난 것은 제작 팝업 윈도우에서 제작 가능하게
                for(int i = 0; i < itemArray[selectedIndex].resultNum; i++)
                {
                    if(contentDictionary[itemArray[selectedIndex].resultItem[i]].isOpenItem == true)
                    {
                        popupWindow.SetItemMakePossible(contentDictionary[itemArray[selectedIndex].resultItem[i]].item);
                    }
                }

                // 다음 연구 오픈
                foreach(int i in itemArray[selectedIndex].nextResearch)
                {
                    DiscoverNewResearch(i);
                }

                // 첫 연구 보상
                if(selectedIndex == 0)
                {
                    nyxUI.SetAmount(100);
                }
                
            }
        }

        for (int i = 0; i < 16; i++)
        {
            if (itemArray[i].isKnown == false)
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
                ItemIcon[i].GetComponent<Image>().sprite = inventory.itemDictionary[itemArray[i].item].sprite;
                ItemIconBack[i].GetComponent<Image>().sprite = inventory.itemDictionary[itemArray[i].item].sprite;
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

        ResultBigItem.GetComponent<Image>().sprite = inventory.itemDictionary[itemArray[selectedIndex].item].sprite;
        ExplainText.GetComponent<Text>().text = itemArray[selectedIndex].expText;

        for(int i = 0; i < 3; i++)
        {
            if( i < itemArray[selectedIndex].resultNum)
            {
                Content[i].SetActive(true);
                ContentIcon[i].GetComponent<Image>().sprite = contentDictionary[itemArray[selectedIndex].resultItem[i]].ContentSp;
                ContentText[i].GetComponent<Text>().text = contentDictionary[itemArray[selectedIndex].resultItem[i]].expText;
                //if(contentDictionary[itemArray[selectedIndex].resultItem[i]].facilitySp == null)
                //{
                //    ContentFacIcon[i].GetComponent<Image>().sprite = inventory.Facility01Sp;
                //}
                //else
                //{
                //    ContentFacIcon[i].GetComponent<Image>().sprite = contentDictionary[itemArray[selectedIndex].resultItem[i]].facilitySp;
                //}
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
        //RefreshWindow();
    }

    //public void DiscoverNewResearch(Inventory.Item item)
    //{
    //    if(ResearchNumberDictionary.ContainsKey(item))
    //    {
    //        itemArray[ResearchNumberDictionary[item]].SetKnown(true);
    //    }
    //    //RefreshWindow();
    //}

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
