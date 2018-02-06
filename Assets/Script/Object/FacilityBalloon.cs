using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityBalloon : MonoBehaviour
{
    Inventory inventory;
    InteractionIcon interactionIcon;

    Animator animaitor;

    GameObject Balloon;
    GameObject TimeText;
    GameObject Item;
    GameObject Item_back;

    public Sprite blueBalloon;
    public Sprite yellowBalloon;

    float progress;
    public float timeToMake; //초
    public float progressTimer;

    public global::Inventory.Item makeItem = global::Inventory.Item.Stick; // 만드는 아이템
    public bool isMake = false;
    public bool isMakeFinish = false;
    public bool isLoadByManager = false;

    void Start ()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();

        Balloon = transform.Find("Balloon").gameObject;
        TimeText = Balloon.transform.Find("TimeText").gameObject;
        Item = Balloon.transform.Find("Item").gameObject;
        Item_back = Balloon.transform.Find("Item_back").gameObject;

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
            }
            if (isMakeFinish == true)
            {
                Balloon.SetActive(true);
                Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
                Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, 0.6f);
            }
            Item.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem];
            Item_back.GetComponent<SpriteRenderer>().sprite = inventory.itemDictionary[makeItem];
            isLoadByManager = false;
        }

        if (isMake == true)
        {
            Timer();
        }
        if (isMakeFinish == true)
        {
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
            Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, 0.6f);
        }

        Display();

    }

    public void MakeItem(global::Inventory.Item itemName, int time)
    {
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

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.DeleteAllIcons();
            GetComponent<Facility>().DisplayIcon();
        }
    }

    void MakeFinish()
    {
        isMakeFinish = true;
        Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
        Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, 0.6f);
        if (animaitor != null)
        {
            animaitor.SetBool("isMaking", false);
        }

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.DeleteAllIcons();
            GetComponent<Facility>().DisplayIcon();
        }
        isMake = false;
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        temp = !inventory.isFull(1, makeItem);
        return temp;
    }

    public void GetItem()
    {
        inventory.GetItem(makeItem);
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        isMakeFinish = false;
        Balloon.SetActive(false);

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
            GetComponent<Facility>().DisplayIcon();
        }
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

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.DeleteAllIcons();
            GetComponent<Facility>().DisplayIcon();
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

        if (isMakeFinish == true)
        {
            Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, 0.6f);
        }
        else
        {
            Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, progress * 0.55f);
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
