using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityBalloon : MonoBehaviour
{
    Inventory inventory;
    NyxUI nyxUI;

    Animator animaitor;

    GameObject Balloon;
    GameObject TimeText;

    GameObject Item;
    GameObject Item_back;
    GameObject Item2;
    GameObject Item2_back;

    public Sprite blueBalloon;
    public Sprite yellowBalloon;

    float progress;
    public float timeToMake; //초
    //public float progressTimer;

    public Inventory.Item makeItem = 0; // 만드는 아이템

    public Inventory.Item[] grinderItem = new Inventory.Item[2] { 0, 0 };
    public int[] grinderItemNum = new int[2] { 0, 0 };
    public int grinderNyxNum = 0;

    public bool isGrinder = false;

    public bool isInit = false;

    //public bool isMakeByGrinder = false;

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();

        Balloon = transform.Find("Balloon").gameObject;
        TimeText = Balloon.transform.Find("TimeText").gameObject;

        Item = Balloon.transform.Find("Item").gameObject;
        Item_back = Balloon.transform.Find("Item_back").gameObject;
        Item2 = Balloon.transform.Find("Item2").gameObject;
        Item2_back = Balloon.transform.Find("Item2_back").gameObject;

        animaitor = GetComponent<Animator>();

        Balloon.SetActive(false);
    }

    void Update ()
    {
        if(isInit == true)
        {
            if (GetComponent<Facility>().state == 2)
            {
                Balloon.SetActive(true);
            }
            if (GetComponent<Facility>().state == 3)
            {
                Balloon.SetActive(true);
                Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
                Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
                Item2.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
            }
            if (isGrinder == false)
            {
                Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem].sprite;
                Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem].sprite;
            }
            else
            {
                DisplayGrinderItems();
            }

            isInit = false;
        }

        if (GetComponent<Facility>().state == 2)
        {
            Timer();
        }
        if (GetComponent<Facility>().state == 3)
        {
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
            Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
            Item2.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
        }

        Display();

    }

    public void MakeItem(Inventory.Item itemName, int time)
    {
        isGrinder = false;

        Balloon.SetActive(true);
        makeItem = itemName;
        Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[itemName].sprite;
        Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[itemName].sprite;
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        timeToMake = time;
        GetComponent<Facility>().objectTimer = timeToMake;
        GetComponent<Facility>().state = 2;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", 2);
        }
    }

    public void GrindItem(int time, int nyx, Inventory.Item itemName1 = 0, int num1 = 0, Inventory.Item itemName2 = 0, int num2 = 0)
    {
        isGrinder = true;

        Balloon.SetActive(true);

        grinderNyxNum = nyx;
        grinderItem = new Inventory.Item[2] { itemName1, itemName2 };
        grinderItemNum = new int[2] { num1, num2 };
        DisplayGrinderItems();

        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        timeToMake = time;
        GetComponent<Facility>().objectTimer = timeToMake;
        GetComponent<Facility>().state = 2;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", 2);
        }   
    }

    void DisplayGrinderItems()
    {
        int count = 2;
        if (grinderItemNum[1] == 0)
        {
            count = 1;
        }
        if (grinderItemNum[0] == 0)
        {
            count = 0;
        }

        Vector3 temp = Item.transform.position;
        temp.x = transform.position.x;
        Item.transform.position = temp;
        temp.z += 0.01f;
        Item_back.transform.position = temp;

        switch (count)
        {
            case 2:
                temp = Item.transform.position;
                temp.x = transform.position.x - 0.14f;
                Item.transform.position = temp;
                temp.z += 0.01f;
                Item_back.transform.position = temp;

                Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]].sprite;
                Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]].sprite;
                Item2.SetActive(true);
                Item2_back.SetActive(true);
                Item2.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[1]].sprite;
                Item2_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[1]].sprite;
                break;
            case 1:
                Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]].sprite;
                Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]].sprite;
                Item2.SetActive(false);
                Item2_back.SetActive(false);
                break;
            case 0:
                Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[Inventory.Item.Nyx].sprite;
                Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[Inventory.Item.Nyx].sprite;
                Item2.SetActive(false);
                Item2_back.SetActive(false);
                break;
        }
    }

    void MakeFinish()
    {
        Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
        Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
        Item2.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);

        GetComponent<Facility>().state = 3;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", 3);
        }
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        if (isGrinder == false)
        {
            temp = !inventory.isFull(1, makeItem);
        }
        else
        {
            int count = 2;
            if (grinderItemNum[1] == 0)
            {
                count = 1;
            }
            if (grinderItemNum[0] == 0)
            {
                return true;
            }
            switch (count)
            {
                case 2:
                    temp = !inventory.isFull(2, grinderItem[0], grinderItemNum[0], grinderItem[1], grinderItemNum[1]);
                    break;
                case 1:
                    temp = !inventory.isFull(1, grinderItem[0], grinderItemNum[0]);
                    break;
            }
        }
        return temp;
    }

    public void GetItem()
    {
        if (isGrinder == false)
        {
            inventory.GetItem(makeItem);
        }
        else
        {
            int count = 2;
            if (grinderItemNum[1] == 0)
            {
                count = 1;
            }
            if (grinderItemNum[0] == 0)
            {
                count = 0;
            }
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    inventory.GetItem(grinderItem[i], grinderItemNum[i]);
                }
            }
            nyxUI.SetAmount(grinderNyxNum);
        }
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        GetComponent<Facility>().state = 1;
        Balloon.SetActive(false);
    }

    public void Dump()
    {
        Balloon.SetActive(false);
        GetComponent<Facility>().state = 1;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", 1);
        }
    }

    void Display()
    {
        float temp = (int)(GetComponent<Facility>().objectTimer % 60);
        if (temp < 10)
        {
            TimeText.GetComponent<TextMesh>().text = (int)(GetComponent<Facility>().objectTimer / 60.0f) + ":0" + temp;
        }
        else
        {
            TimeText.GetComponent<TextMesh>().text = (int)(GetComponent<Facility>().objectTimer / 60.0f) + ":" + temp;
        }

        if (GetComponent<Facility>().state == 3)
        {
            Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
            Item2.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
        }
        else
        {
            Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, progress * 2.3f);
            Item2.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, progress * 2.3f);
        }
    }

    void Timer()
    {
        if(GetComponent<Facility>().objectTimer > 0)
        {
            GetComponent<Facility>().objectTimer -= Time.deltaTime;
        }
        if(GetComponent<Facility>().objectTimer < 0)
        {
            GetComponent<Facility>().objectTimer = 0;
        }
        progress = 1.0f - (GetComponent<Facility>().objectTimer / timeToMake);

        if (GetComponent<Facility>().objectTimer <= 0)
        {
            progress = 1.0f;
            MakeFinish();
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
        }
    }
}
