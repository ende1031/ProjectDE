using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrinderWindow : MonoBehaviour
{
    Inventory inventory;
    GameObject Player;
    GameObject Facility;

    GameObject Grinder_BG;
    GameObject BigItem;
    Text BigItemNum;
    GameObject PickItem;
    GameObject PickNumber;
    GameObject[] ResultItem = new GameObject[3];
    GameObject[] ResultItemNum = new GameObject[3];
    GameObject Button;
    Text ButtonText;
    Text EnergyNum;

    Animator animaitor;

    public Sprite YellowButton;
    public Sprite RedButton;

    bool isPopupActive = false;
    float openTimer = 0;

    Inventory.Item selectedItem;
    int selectedItemNum = 1;
    Inventory.Item[] resultItem = new Inventory.Item[3];
    int[] resultItemNum = new int[3] { 0, 0, 0 };
    int resultItemCount = 0;
    bool selectNull = true;

    bool isSelectingNumber = false;

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        Grinder_BG = transform.Find("Grinder_BG").gameObject;
        animaitor = GetComponent<Animator>();
        SetWindowObject();
    }

    void SetWindowObject()
    {
        BigItem = Grinder_BG.transform.Find("BigItem").gameObject;
        BigItemNum = BigItem.transform.Find("Num").GetComponent<Text>();
        EnergyNum = BigItem.transform.Find("EnergyNum").GetComponent<Text>();
        PickItem = Grinder_BG.transform.Find("PickItem").gameObject;
        PickNumber = Grinder_BG.transform.Find("PickNumber").gameObject;
        Button = Grinder_BG.transform.Find("Button").gameObject;
        ButtonText = Button.transform.Find("Text").GetComponent<Text>();

        for (int i = 0; i < 3; i++)
        {
            ResultItem[i] = Grinder_BG.transform.Find("ResultItem" + (i + 1)).gameObject;
            ResultItemNum[i] = ResultItem[i].transform.Find("Num").gameObject;
        }
    }

    public void RefreshWindow()
    {
        if(selectNull == false)
        {
            BigItem.SetActive(true);
            BigItem.GetComponent<Image>().sprite = inventory.itemDictionary[selectedItem];
        }
        else
        {
            BigItem.SetActive(false);
            resultItemCount = 0;
            selectedItemNum = 1;
        }
        if(selectedItemNum >= 10)
        {
            BigItemNum.text = selectedItemNum.ToString();
            EnergyNum.text = selectedItemNum.ToString();
        }
        else
        {
            BigItemNum.text = "0" + selectedItemNum;
            EnergyNum.text = "0" + selectedItemNum;
        }
        int c = 2;
        while(c >= resultItemCount)
        {
            ResultItem[c].SetActive(false);
            ResultItemNum[c].SetActive(false);
            c--;
        }
        for(int i = 0; i < resultItemCount; i++)
        {
            ResultItem[c].GetComponent<Image>().sprite = inventory.itemDictionary[resultItem[i]];
            if (resultItemNum[i] >= 10)
            {
                ResultItemNum[c].GetComponent<Text>().text = resultItemNum[i].ToString();
            }
            else
            {
                ResultItemNum[c].GetComponent<Text>().text = "0" + resultItemNum[i];
            }
        }
        if(isSelectingNumber == true)
        {
            Button.GetComponent<Image>().sprite = RedButton;
            ButtonText.text = "C : 분해 시작";
            PickItem.SetActive(false);
            PickNumber.SetActive(true);
        }
        else
        {
            Button.GetComponent<Image>().sprite = YellowButton;
            ButtonText.text = "C : 분해 아이템 선택";
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
                        //MakeItem();
                    }
                }
            }
        }
    }

    /*
    bool InventoryCheck()
    {
        bool temp = true;

        temp = !inventory.isFull(resultItemCount, resultItem[0], resultItemNum[0], resultItem[1], resultItemNum[1], resultItem[2], resultItemNum[2]);
        
        return temp;
    }
    */

    public void SelectItem(bool isNull, Inventory.Item it = 0)
    {
        selectNull = isNull;
        selectedItem = it;
        isPopupActive = true;
        isSelectingNumber = true;
        selectedItemNum = inventory.CountOfItem(selectedItem);
        RefreshWindow();
    }

    public void InventoryMove(bool isNull, Inventory.Item it = 0)
    {
        selectNull = isNull;
        selectedItem = it;
        selectedItemNum = inventory.CountOfItem(selectedItem);
        RefreshWindow();
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) && selectedItemNum < inventory.CountOfItem(selectedItem))
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
    }

    public void CloseWindow()
    {
        Player.GetComponent<PlayerMove>().SetMovePossible(true);
        Player.GetComponent<PlayerInteraction>().SetInteractionPossible(true);
        isPopupActive = false;
        openTimer = 0;
        animaitor.SetBool("isOpen", false);
    }
}