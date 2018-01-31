using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityBalloon : MonoBehaviour
{
    GameObject Inventory;
    GameObject InteractionIcon;

    Animator animaitor;

    GameObject Balloon;
    GameObject TimeText;
    GameObject Item;
    GameObject Item_back;

    public Sprite blueBalloon;
    public Sprite yellowBalloon;

    float progress;
    public float timeToMake = 15; //초
    public float progressTimer = 15;

    public global::Inventory.Item makeItem = global::Inventory.Item.Stick; // 만드는 아이템
    public bool isMake = false;
    public bool isMakeFinish = false;
    public bool isLoadByManager = false;

    Dictionary<global::Inventory.Item, Sprite> itemDictionary = new Dictionary<global::Inventory.Item, Sprite>();

    void Start ()
    {
        Inventory = GameObject.Find("Inventory");
        InteractionIcon = GameObject.Find("InteractionIcon");
        SetDictionary();

        Balloon = transform.Find("Balloon").gameObject;
        TimeText = Balloon.transform.Find("TimeText").gameObject;
        Item = Balloon.transform.Find("Item").gameObject;
        Item_back = Balloon.transform.Find("Item_back").gameObject;

        animaitor = GetComponent<Animator>();

        Balloon.SetActive(false);
    }

    void SetDictionary() //아이템 추가시 수정할 부분
    {
        itemDictionary[global::Inventory.Item.Food] = Inventory.GetComponent<Inventory>().FoodSp;
        itemDictionary[global::Inventory.Item.Oxygen] = Inventory.GetComponent<Inventory>().OxygenSp;
        itemDictionary[global::Inventory.Item.Battery] = Inventory.GetComponent<Inventory>().BatterySp;
        itemDictionary[global::Inventory.Item.Stick] = Inventory.GetComponent<Inventory>().StickSp;
        itemDictionary[global::Inventory.Item.Board] = Inventory.GetComponent<Inventory>().BoardSp;
        itemDictionary[global::Inventory.Item.Hose] = Inventory.GetComponent<Inventory>().HoseSp;
        itemDictionary[global::Inventory.Item.Mass] = Inventory.GetComponent<Inventory>().MassSp;
        itemDictionary[global::Inventory.Item.Thorn] = Inventory.GetComponent<Inventory>().ThornSp;
        itemDictionary[global::Inventory.Item.Facility01] = Inventory.GetComponent<Inventory>().Facility01Sp;
        itemDictionary[global::Inventory.Item.Trap01] = Inventory.GetComponent<Inventory>().Trap01Sp;
        itemDictionary[global::Inventory.Item.Heart] = Inventory.GetComponent<Inventory>().HeartSp;
        itemDictionary[global::Inventory.Item.Bulb01] = Inventory.GetComponent<Inventory>().Bulb01Sp;
    }

    void Update ()
    {
        if(isMake == true)
        {
            Timer();
        }
        if(isMakeFinish == true)
        {
            Balloon.GetComponent<SpriteRenderer>().sprite = yellowBalloon;
            Item.GetComponent<SpriteRenderer>().size = new Vector2(0.6f, 0.6f);
        }
        

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
            Item.GetComponent<SpriteRenderer>().sprite = itemDictionary[makeItem];
            Item_back.GetComponent<SpriteRenderer>().sprite = itemDictionary[makeItem];
            isLoadByManager = false;
        }

        Display();

    }

    public void MakeItem(global::Inventory.Item itemName, int time)
    {
        Balloon.SetActive(true);
        makeItem = itemName;
        Item.GetComponent<SpriteRenderer>().sprite = itemDictionary[itemName];
        Item_back.GetComponent<SpriteRenderer>().sprite = itemDictionary[itemName];
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
            InteractionIcon.GetComponent<InteractionIcon>().DeleteAllIcons();
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
            InteractionIcon.GetComponent<InteractionIcon>().DeleteAllIcons();
            GetComponent<Facility>().DisplayIcon();
        }
        isMake = false;
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        temp = !Inventory.GetComponent<Inventory>().isFull(1, makeItem);
        return temp;
    }

    public void GetItem()
    {
        Inventory.GetComponent<Inventory>().GetItem(makeItem);
        Balloon.GetComponent<SpriteRenderer>().sprite = blueBalloon;
        isMakeFinish = false;
        Balloon.SetActive(false);

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
            GetComponent<Facility>().DisplayIcon();
        }
    }

    public void Dunp()
    {
        Balloon.SetActive(false);
        isMake = false;
        isMakeFinish = false;
        animaitor.SetBool("isMaking", false);

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteAllIcons();
            GetComponent<Facility>().DisplayIcon();
        }
    }

    void Display()
    {
        float temp = Mathf.Ceil(progressTimer % 60);
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
