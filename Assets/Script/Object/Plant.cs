using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    Inventory inventory;
    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Monologue monologue;
    GameObject Player;

    Animator animaitor;

    int sceneNum;

    public bool isGatherPossible;
    public string plantName = "StickPlant";
    public int GatherAnimationType = 1;
    public int state; //0:자라는 중(덫설치후 대기중), 1:채집가능(덫잡힘), 2:채집후, 3:덫설치중, 4:종양심음, 5:종양채집가능

    public float growthTime; //성장하는데 걸리는 시간(초)
    public float growthTimer = 0;
    public bool isTumor = false;

    public bool isLoadByManager = false;

    void Start ()
    {
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        Player = GameObject.Find("Player");
        monologue = Player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();

        animaitor = GetComponent<Animator>();

        animaitor.SetInteger("State", state);
        animaitor.SetBool("isGathering", false); //플레이어가 채집을 하는것인지, 아니면 바로 애니메이션 전환을 할지
        animaitor.SetBool("isGrowSkip", false); //성장 애니메이션을 스킵할지
    }

    public void GatherStart()
    {
        state = 2;
        animaitor.SetBool("isGathering", true);
        animaitor.SetInteger("State", state);
    }

    public void GetItem()
    {
        if (isTumor == false)
        {
            switch (plantName)
            {
                case "StickPlant":
                    inventory.GetItem(Inventory.Item.Stick, 2);
                    break;
                case "MassPlant":
                    inventory.GetItem(Inventory.Item.Mass, 1);
                    if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
                    {
                        interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
                    }
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
                case "BoardPlant":
                    inventory.GetItem(Inventory.Item.Board, 1);
                    break;
                case "ThornPlant":
                    inventory.GetItem(Inventory.Item.Thorn, 5);
                    break;
                case "Trap01":
                    inventory.GetItem(Inventory.Item.Hose, 1);
                    inventory.GetItem(Inventory.Item.Heart, 1);
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    inventory.GetItem(Inventory.Item.Tumor, 2);
                    break;
                case "BoardPlant":
                    inventory.GetItem(Inventory.Item.Tumor, 2);
                    break;
                case "ThornPlant":
                    inventory.GetItem(Inventory.Item.Tumor, 6);
                    break;
            }
        }
        isGatherPossible = false;
        isTumor = false;
    }

    public void RemoveObject()
    {
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        if (isTumor == false)
        {
            switch (plantName)
            {
                case "StickPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Stick, 2);
                    break;
                case "MassPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Mass, 1);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Board, 1);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Thorn, 5);
                    break;
                case "Trap01":
                    temp = !inventory.isFull(3, Inventory.Item.Hose, 1, Inventory.Item.Heart, 1);
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Tumor, 2);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Tumor, 2);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, Inventory.Item.Tumor, 6);
                    break;
            }
        }
        return temp;
    }

    public void SetTumor()
    {
        if (state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
        {
            state = 4;
            inventory.DeleteItem(Inventory.Item.TumorSeed);
            animaitor.SetInteger("State", state);
        }
        else
        {
            monologue.DisplayLog("식물이 다 자라서 종양을 설치할 수 없게 돼버렸군.\n일단 채집을 한 후에 다시 시도해보자.");
        }
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleFalse()
    {
        isGatherPossible = false;
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleTrue()
    {
        isGatherPossible = true;
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetTrapOn()
    {
        state = 0;
        animaitor.SetInteger("State", state);
    }

    void Update ()
    {
        if (isLoadByManager == true)
        {
            if (growthTimer >= growthTime)
            {
                growthTimer = 0;
                if (state == 4)
                {
                    state = 5;
                }
                else
                {
                    state = 1;
                }
                animaitor.SetInteger("State", state);
                animaitor.SetBool("isGrowSkip", true);
            }
            isLoadByManager = false;
        }

        if (plantName != "MassPlant")
        {
            Growth();
        }
        if(state == 4 || state == 5)
        {
            isTumor = true;
        }
        animaitor.SetInteger("State", state);
    }

    void Growth()
    {
        if (state == 0 || state == 2 || state == 4)
        {
            if (plantName == "Trap01")
            {
                if (Mathf.Abs(Grid.instance.PlayerGrid() - Grid.instance.PosToGrid(transform.position.x)) > 4)
                {
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01", true) == false)
                    {
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                        {
                            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Nest", "Nest01") == true)
                            {
                                growthTimer += Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else
            {
                growthTimer += Time.deltaTime;
            }
            
            if (growthTimer >= growthTime)
            {
                growthTimer = 0;
                if(state == 4)
                {
                    state = 5;
                }
                else
                {
                    state = 1;
                }
                animaitor.SetInteger("State", state);
                animaitor.SetBool("isGrowSkip", false);
            }
        }

        if ((state == 1 || state == 5) && growthTimer != 0)
        {
            growthTimer = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
        }
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        if (isGatherPossible == true)
        {
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Gather);
        }
        else if (state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
        {
            if (inventory.HasItem(Inventory.Item.TumorSeed) == true)
            {
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Tumor);
            }
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, "Plant", GetComponent<SpriteRenderer>().sprite, w, h);
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
    {
        switch (m)
        {
            case InteractionMenu.MenuItem.Gather:
                if(InventoryCheck() == true)
                {
                    GatherStart();
                    Player.GetComponent<PlayerInteraction>().GatherPlant(GatherAnimationType);
                }
                else
                {
                    monologue.DisplayLog("인벤토리 공간이 부족하군.\n채집하기 전에 필요없는 아이템을 버리는게 좋겠어.");
                }
                break;
            case InteractionMenu.MenuItem.Tumor:
                SetTumor();
                break;
            case InteractionMenu.MenuItem.Remove:
                RemoveObject();
                break;
        }
    }
}
