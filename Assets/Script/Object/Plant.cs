using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    Inventory inventory;
    InteractionIcon interactionIcon;

    Animator animaitor;

    int sceneNum;

    public bool isGatherPossible;
    public string plantName = "StickPlant";
    public int GatherAnimationType = 1;
    public int state; //0:자라는 중(덫설치후 대기중), 1:채집가능(덫잡힘), 2:채집후, 3:덫설치중, 4:종양심음, 5:종양채집가능

    public float growthTime; //성장하는데 걸리는 시간(초)
    public float growthTimer = 0;
    public bool isTumor = false;

    void Start ()
    {
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();

        animaitor = GetComponent<Animator>();
        animaitor.SetInteger("State", state);
        animaitor.SetBool("isGathering", false); //플레이어가 채집을 하는것인지, 아니면 바로 애니메이션 전환을 할지
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
                    inventory.GetItem(global::Inventory.Item.Stick, 2);
                    break;
                case "MassPlant":
                    inventory.GetItem(global::Inventory.Item.Mass, 1);
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
                case "BoardPlant":
                    inventory.GetItem(global::Inventory.Item.Board, 1);
                    break;
                case "ThornPlant":
                    inventory.GetItem(global::Inventory.Item.Thorn, 5);
                    break;
                case "Trap01":
                    inventory.GetItem(global::Inventory.Item.Hose, 1);
                    inventory.GetItem(global::Inventory.Item.Heart, 1);
                    inventory.GetItem(global::Inventory.Item.Mass, 1);
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    inventory.GetItem(global::Inventory.Item.Tumor, 2);
                    break;
                case "BoardPlant":
                    inventory.GetItem(global::Inventory.Item.Tumor, 2);
                    break;
                case "ThornPlant":
                    inventory.GetItem(global::Inventory.Item.Tumor, 6);
                    break;
            }
        }
        isGatherPossible = false;
        isTumor = false;

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
        }
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
                    temp = !inventory.isFull(1, global::Inventory.Item.Stick, 2);
                    break;
                case "MassPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Mass, 1);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Board, 1);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Thorn, 5);
                    break;
                case "Trap01":
                    temp = !inventory.isFull(3, global::Inventory.Item.Hose, 1, global::Inventory.Item.Heart, 1, global::Inventory.Item.Mass, 1);
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Tumor, 2);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Tumor, 2);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, global::Inventory.Item.Tumor, 6);
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
            animaitor.SetInteger("State", state);
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Tumor);
            inventory.DeleteItem(Inventory.Item.TumorSeed);
        }
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleFalse()
    {
        isGatherPossible = false;
        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            if (state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
            {
                if (inventory.HasItem(Inventory.Item.TumorSeed) == true)
                {
                    interactionIcon.AddIcon(global::InteractionIcon.Icon.Tumor);
                }
            }
            interactionIcon.AddIcon(global::InteractionIcon.Icon.Remove);
        }
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleTrue()
    {
        isGatherPossible = true;
        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            interactionIcon.AddIcon(global::InteractionIcon.Icon.Gather);
        }
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetTrapOn()
    {
        state = 0;
        animaitor.SetInteger("State", state);
    }

    void Update ()
    {
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
            if(state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
            {
                if (inventory.HasItem(Inventory.Item.TumorSeed) == true)
                {
                    interactionIcon.AddIcon(global::InteractionIcon.Icon.Tumor);
                }
            }
            if (isGatherPossible == true)
            {
                interactionIcon.AddIcon(global::InteractionIcon.Icon.Gather);
                interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Remove);
                interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Tumor);
            }
            else
            {
                interactionIcon.AddIcon(global::InteractionIcon.Icon.Remove);
                interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Remove);
            interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Tumor);
        }
    }
}
