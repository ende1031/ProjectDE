using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityBalloon : MonoBehaviour
{
    Inventory inventory;

    Animator animaitor;

    GameObject Balloon;
    GameObject TimeText;

    GameObject MakeItems;
    GameObject Item;
    GameObject Item_back;

    GameObject GrindItems;
    GameObject[] GrindItemSet = new GameObject[3];

    public Sprite blueBalloon;
    public Sprite yellowBalloon;

    float progress;
    public float timeToMake; //초
    public float progressTimer;

    public Inventory.Item makeItem = 0; // 만드는 아이템

    public Inventory.Item[] grinderItem = new Inventory.Item[3] { 0, 0, 0 };
    public int[] grinderItemNum = new int[3] { 0, 0, 0 };

    public bool isMake = false;
    public bool isMakeFinish = false;
    public bool isLoadByManager = false;

    public bool isMakeByGrinder = false;

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        Balloon = transform.Find("Balloon").gameObject;
        TimeText = Balloon.transform.Find("TimeText").gameObject;

        MakeItems = Balloon.transform.Find("MakeItems").gameObject;
        Item = MakeItems.transform.Find("Item").gameObject;
        Item_back = MakeItems.transform.Find("Item_back").gameObject;

        GrindItems = Balloon.transform.Find("GrindItems").gameObject;
        for(int i =0; i < 3; i++)
        {
            GrindItemSet[i] = GrindItems.transform.Find("ItemSet" + (i + 1)).gameObject;
            GrindItemSet[i].SetActive(false);
        }

        animaitor = GetComponent<Animator>();

        Balloon.SetActive(false);
    }

    void Update ()
    {
        if(isLoadByManager == true)
        {
            if (animaitor != null)
            {
                animaitor.SetBool("isOn", GetComponent<Facility>().isOn);
            }
            if (isMake == true)
            {
                Balloon.SetActive(true);
                if(animaitor != null)
                {
                    animaitor.SetBool("isMaking", true);
                }

                if (isMakeByGrinder == true)
                {
                    MakeItems.SetActive(false);
                    GrindItems.SetActive(true);
                }
                else
                {
                    MakeItems.SetActive(true);
                    GrindItems.SetActive(false);
                }
            }
            if (isMakeFinish == true)
            {
                Balloon.SetActive(true);
                Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
                Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);

                if (isMakeByGrinder == true)
                {
                    MakeItems.SetActive(false);
                    GrindItems.SetActive(true);
                }
                else
                {
                    MakeItems.SetActive(true);
                    GrindItems.SetActive(false);
                }
            }
            if(isMakeByGrinder == false)
            {
                Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem];
                Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem];
            }
            else
            {
                DisplayGrinderItems();
            }

            isLoadByManager = false;
        }

        if (isMake == true)
        {
            Timer();
        }
        if (isMakeFinish == true)
        {
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
            Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
        }

        Display();

    }

    public void MakeItem(Inventory.Item itemName, int time)
    {
        isMakeByGrinder = false;
        MakeItems.SetActive(true);
        GrindItems.SetActive(false);

        Balloon.SetActive(true);
        makeItem = itemName;
        Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[itemName];
        Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[itemName];
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        timeToMake = time;
        progressTimer = timeToMake;
        isMake = true;
        isMakeFinish = false;
        if (animaitor != null)
        {
            animaitor.SetBool("isMaking", true);
        }
    }

    public void GrindItem(int time, Inventory.Item itemName1, int num1, Inventory.Item itemName2 = 0, int num2 = 0, Inventory.Item itemName3 = 0, int num3 = 0)
    {
        isMakeByGrinder = true;
        MakeItems.SetActive(false);
        GrindItems.SetActive(true);

        Balloon.SetActive(true);

        grinderItem = new Inventory.Item[3] { itemName1, itemName2, itemName3 };
        grinderItemNum = new int[3] { num1, num2, num3 };
        DisplayGrinderItems();

        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        timeToMake = time;
        progressTimer = timeToMake;
        isMake = true;
        isMakeFinish = false;
        if (animaitor != null)
        {
            animaitor.SetBool("isMaking", true);
        }   
    }

    void DisplayGrinderItems()
    {
        for (int i = 0; i < 3; i++)
        {
            GrindItemSet[i].SetActive(false);
        }
        int count = 3;
        if (grinderItemNum[2] == 0)
        {
            count = 2;
        }
        if (grinderItemNum[1] == 0)
        {
            count = 1;
        }
        switch (count)
        {
            case 1:
                GrindItemSet[0].SetActive(true);
                GrindItemSet[0].transform.Find("Item1").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]];
                break;
            case 2:
                GrindItemSet[1].SetActive(true);
                GrindItemSet[1].transform.Find("Item1").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]];
                GrindItemSet[1].transform.Find("Item2").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[1]];
                break;
            case 3:
                GrindItemSet[2].SetActive(true);
                GrindItemSet[2].transform.Find("Item1").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[0]];
                GrindItemSet[2].transform.Find("Item2").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[1]];
                GrindItemSet[2].transform.Find("Item3").GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[grinderItem[2]];
                break;
        }
    }

    void MakeFinish()
    {
        isMakeFinish = true;
        Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
        Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
        if (animaitor != null)
        {
            animaitor.SetBool("isMaking", false);
        }
        
        isMake = false;
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        if (isMakeByGrinder == false)
        {
            temp = !inventory.isFull(1, makeItem);
        }
        else
        {
            int count = 3;
            if (grinderItemNum[2] == 0)
            {
                count = 2;
            }
            if (grinderItemNum[1] == 0)
            {
                count = 1;
            }
            switch (count)
            {
                case 1:
                    temp = !inventory.isFull(1, grinderItem[0], grinderItemNum[0]);
                    break;
                case 2:
                    temp = !inventory.isFull(2, grinderItem[0], grinderItemNum[0], grinderItem[1], grinderItemNum[1]);
                    break;
                case 3:
                    temp = !inventory.isFull(3, grinderItem[0], grinderItemNum[0], grinderItem[1], grinderItemNum[1], grinderItem[2], grinderItemNum[2]);
                    break;
            }
        }
        return temp;
    }

    public void GetItem()
    {
        if (isMakeByGrinder == false)
        {
            inventory.GetItem(makeItem);
        }
        else
        {
            int count = 3;
            if (grinderItemNum[2] == 0)
            {
                count = 2;
            }
            if (grinderItemNum[1] == 0)
            {
                count = 1;
            }
            for(int i =0; i< count; i++)
            {
                inventory.GetItem(grinderItem[i], grinderItemNum[i]);
            }
        }
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        isMakeFinish = false;
        Balloon.SetActive(false);
    }

    public void Dunp()
    {
        Balloon.SetActive(false);
        isMake = false;
        isMakeFinish = false;
        if (animaitor != null)
        {
            animaitor.SetBool("isMaking", false);
        }
    }

    void Display()
    {
        float temp = (int)(progressTimer % 60);
        if (temp < 10)
        {
            TimeText.GetComponent<TextMesh>().text = (int)(progressTimer / 60.0f) + ":0" + temp;
        }
        else
        {
            TimeText.GetComponent<TextMesh>().text = (int)(progressTimer / 60.0f) + ":" + temp;
        }

        if (isMakeByGrinder == false)
        {
            if (isMakeFinish == true)
            {
                Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, 2.4f);
            }
            else
            {
                Item.GetComponent<SpriteRenderer>().size = new Vector2(2.4f, progress * 2.3f);
            }
        }
    }

    void Timer()
    {
        if(progressTimer > 0)
        {
            progressTimer -= Time.deltaTime;
        }
        if(progressTimer < 0)
        {
            progressTimer = 0;
        }
        progress = 1.0f - (progressTimer / timeToMake);

        if (progressTimer <= 0)
        {
            progress = 1.0f;
            MakeFinish();
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
        }
    }
}
