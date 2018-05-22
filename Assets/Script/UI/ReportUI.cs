using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    Inventory inventory;
    ResearchWindow researchWindow;
    Monologue monologue;

    Text Day_Text;
    Text D_Day_Text;
    Text ToDo_Text;

    int day = 1;
    int d_Day = 99;

    int questNum = 0;
    bool tutorialFinish = false;

    List<ReportItem> ReportItemList = new List<ReportItem>();

    public class ReportItem
    {
        public ReportItem(string txt, Inventory.Item it, int max)
        {
            text = txt;
            type = "Item";
            item = it;
            maxNum = max;
            itemNum = 0;
            questCompletion = false;
            tip = string.Empty;
            comment = string.Empty;
        }

        public ReportItem(string txt, int num)
        {
            text = txt;
            type = "Research";
            researchNum = num;
            questCompletion = false;
            tip = string.Empty;
            comment = string.Empty;
        }

        public void SetTip(string t)
        {
            tip = "\n\nTip.\n" + t;
        }

        public void SetComment(string t)
        {
            comment = t;
        }

        public string text;
        public string type;
        public string tip;
        public string comment;

        public int itemNum;
        public int maxNum;
        public Inventory.Item item;
        public int researchNum;

        public bool questCompletion;
    }

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();

        Day_Text = transform.Find("Day").gameObject.GetComponent<Text>();
        D_Day_Text = transform.Find("D_Day").gameObject.GetComponent<Text>();
        ToDo_Text = transform.Find("ToDo").gameObject.GetComponent<Text>();

        SetReportItemList();

        RefreshUI();
    }

    void SetReportItemList()
    {
        ReportItemList.Add(new ReportItem("땅에 떨어져 있는 '괴상한 덩어리' 3개를 줍자.", Inventory.Item.Mass, 3));
        ReportItemList[0].SetTip("땅에 떨어져 있는 덩어리 앞에서 [C]버튼을 눌러보자.");
        ReportItemList[0].SetComment("어딘가에 쓸 수 있을 것 같군.\n탈출포드에 가져가서 연구를 해보자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '괴상한 덩어리' 연구를 완료하자.", 0));
        ReportItemList[1].SetTip("탈출포드 앞에서 [C]버튼을 누르고 '연구하기'를 선택하자.");

        ReportItemList.Add(new ReportItem("'괴식물의 열매'를 3개 채집하자.", Inventory.Item.Fruit, 3));
        ReportItemList[2].SetTip("열매 나무 앞에서 [C]버튼을 누르면 나오는 메뉴를 통해 얻을 수 있다.");
        ReportItemList[2].SetComment("이 열매는 먹을 수 있을 것 같군.\n혹시 모르니까 탈출포드에서 연구를 해보자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '괴식물의 열매' 연구를 완료하자.", 1));
        ReportItemList.Add(new ReportItem("'가시 덩굴'에서 '가시' 9개를 채집하자.", Inventory.Item.Thorn, 9));
        ReportItemList[4].SetComment("괴식물의 가시로 무언가를 만들 수 있을 것 같군.\n탈출포드에서 연구해보자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '가시' 연구를 완료하자.", 2));
        ReportItemList.Add(new ReportItem("탈출포드에서 '소형 덫'을 제작하자.", Inventory.Item.Trap01, 1));
        ReportItemList[6].SetTip("탈출포드에서 '아이템 제작'을 눌러보자.");
        ReportItemList[6].SetComment("덫으로 괴물을 잡을 수 있겠어.\n왼쪽 끝에 있는 괴물의 둥지 옆에 설치하자.");

        ReportItemList.Add(new ReportItem("소형 덫을 설치해서 '괴물의 심장'을 얻자.", Inventory.Item.Heart, 1));
        ReportItemList[7].SetTip("'괴물의 둥지' 2칸 이내에 설치할 수 있다.\n설치후 탈출포드에서 '잠자기'를 해보자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '괴물의 심장' 연구를 완료하자.", 3));
        ReportItemList.Add(new ReportItem("'집게발 대나무'에서 '막대' 6개를 채집하자.", Inventory.Item.Stick, 6));
        ReportItemList[9].SetTip("'집게발 대나무'가 있을만한 주변 건물 내부를 조사하자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '막대' 연구를 완료하자.", 4));
        ReportItemList.Add(new ReportItem("'판자 식물'에서 '판자' 3개를 채집하자.", Inventory.Item.Board, 3));
        ReportItemList[11].SetTip("'판자 식물'은 매우 빠르게 자라므로 계속해서 채집할 수 있다.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '판자' 연구를 완료하자.", 5));
        ReportItemList.Add(new ReportItem("'닉스입자 수집기'를 제작하자.", Inventory.Item.NyxCollector01, 1));
        ReportItemList[13].SetTip("탈출포드에서 '아이템 제작'을 눌러보자.");
        ReportItemList[13].SetComment("닉스입자 수집기는 탈출포드 근처에 설치하자.\n멀리 설치하면 괴물의 공격을 받을수도 있어.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '괴물의 심장' 연구를 완료하자.", 6));
        ReportItemList[14].SetTip("'괴물의 둥지' 2칸 이내에 설치할 수 있다.\n설치후 탈출포드에서 '잠자기'를 해보자.");
        ReportItemList.Add(new ReportItem("'소형 워크벤치'를 제작하자.", Inventory.Item.Facility01, 1));
        ReportItemList.Add(new ReportItem("탈출포드에서 '막대' 연구를 완료하자.", 7));
        ReportItemList.Add(new ReportItem("'간이 전구'를 제작하자.", Inventory.Item.Bulb01, 1));
        ReportItemList[17].SetComment("간이 전구는 탈출포드에서 멀리 떨어진 곳에 설치하자.");

        ReportItemList.Add(new ReportItem("탈출포드에서 '판자' 연구를 완료하자.", 8));
        ReportItemList.Add(new ReportItem("'간이 분해기'를 제작하자.", Inventory.Item.Grinder01, 1));
        ReportItemList[19].SetComment("이제 필요 없는 아이템은 분해기에서 분해하면 되겠군.");
    }

    void Update ()
    {
        if (monologue == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
            }
        }

        if (tutorialFinish == false)
        {
            QuestCheck();
        }
    }

    void QuestCheck()
    {
        if (tutorialFinish == true)
        {
            return;
        }

        if (ReportItemList[questNum].type == "Item")
        {
            if (inventory.CountOfItem(ReportItemList[questNum].item) >= ReportItemList[questNum].maxNum)
            {
                ReportItemList[questNum].questCompletion = true;
                QuestNumUp();
                RefreshUI();
            }
        }
        else if (ReportItemList[questNum].type == "Research")
        { 
            if(researchWindow.Getfinishment(ReportItemList[questNum].researchNum) == true)
            {
                ReportItemList[questNum].questCompletion = true;
                QuestNumUp();
                RefreshUI();
            }
        }
    }

    void QuestNumUp()
    {
        if(ReportItemList[questNum].comment != string.Empty)
        {
            monologue.DisplayLog(ReportItemList[questNum].comment);
        }
        
        questNum++;
        if(questNum >= ReportItemList.Count)
        {
            tutorialFinish = true;
        }
    }

    public void RefreshUI()
    {
        Day_Text.text = day + "일째";
        D_Day_Text.text = "구조까지 " + d_Day + "일 남음";

        if (tutorialFinish == true)
        {
            ToDo_Text.text = string.Empty;
            return;
        }

        if (ReportItemList[questNum].type == "Item")
        {
            ToDo_Text.text = "· " + ReportItemList[questNum].text + " [" + inventory.CountOfItem(ReportItemList[questNum].item) + "/" + ReportItemList[questNum].maxNum + "]" + ReportItemList[questNum].tip;
        }
        else if (ReportItemList[questNum].type == "Research")
        {
            ToDo_Text.text = "· " + ReportItemList[questNum].text + " [미완료]" + ReportItemList[questNum].tip;
        }

    }

    public void AddDay()
    {
        day++;
        d_Day--;
        RefreshUI();
    }
}
