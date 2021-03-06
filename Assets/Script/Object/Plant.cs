﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : SceneObject
{
    //public string ObjectName;
    //[TextArea]
    //public string ObjectExplanation;

    //Inventory inventory;
    //InteractionIcon interactionIcon;
    //InteractionMenu interactionMenu;
    //Monologue monologue;
    //EnergyGauge energyGauge;
    //int sceneNum;
    //public int state;

    GameObject player;
    Animator animaitor;

    public bool isGatherPossible;
    public string plantName = "StickPlant";
    public int GatherAnimationType = 1;

    public float growthTime;
    public bool isTumor = false;

    //public float growthTimer = 0;
    //public bool isLoadByManager = false;
    //state 0:자라는 중(덫설치후 대기중), 1:채집가능(덫잡힘), 2:채집후, 3:덫설치중, 4:종양심음, 5:종양채집가능

    void Start ()
    {
        LoadMenuUIAndSeneNum();
        //sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        //inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        //interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        //interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        //monologue = player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        //energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();

        player = GameObject.Find("Player");
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


        switch (plantName)
        {
            case "MassPlant":
            case "Trap01":
                SoundManager.instance.PlaySE(10);
                break;
            case "BoardPlant":
                SoundManager.instance.PlaySE(12);
                break;
            default:
                SoundManager.instance.PlaySE(11);
                break;
        }
    }

    public void GetItem()
    {
        energyGauge.SetAmount(-5);
        if (isTumor == false)
        {
            switch (plantName)
            {
                case "StickPlant":
                    inventory.GetItem("Item_Stick", 2);
                    break;
                case "MassPlant":
                    inventory.GetItem("Item_Mass", 1);
                    if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
                    {
                        interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
                    }
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
                case "BoardPlant":
                    inventory.GetItem("Item_Board", 2);
                    break;
                case "ThornPlant":
                    inventory.GetItem("Item_Thorn", 3);
                    break;
                case "Trap01":
                    inventory.GetItem("Item_Heart", 1);
                    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                    break;
                case "FruitPlant":
                    inventory.GetItem("Item_Fruit", 1);
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    inventory.GetItem("Item_Tumor", 2);
                    break;
                case "BoardPlant":
                    inventory.GetItem("Item_Tumor", 2);
                    break;
                case "ThornPlant":
                    inventory.GetItem("Item_Tumor", 6);
                    break;
            }
        }
        isGatherPossible = false;
        isTumor = false;
    }

    //public void RemoveObject()
    //{
    //    if (energyGauge.GetAmount() < 5)
    //    {
    //        monologue.DisplayLog("에너지가 부족해서 제거할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
    //        return;
    //    }
    //    energyGauge.SetAmount(-5);
    //    interactionIcon.DeleteAllIcons();
    //    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    //}

    public override void Examine()
    {
        if(plantName == "Trap01" && state == 1)
        {
            monologue.DisplayLog("덫에 무언가가 잡힌 것 같다!\n아이템 획득을 눌러서 확인해보자.");
            return;
        }

        monologue.DisplayLog(ExamineText);

        //switch (plantName)
        //{
        //    case "StickPlant":
        //        monologue.DisplayLog("집게발 모양의 대나무이다.\n다행히 공격적이지는 않아서 채집하기 쉬울 것 같다.");
        //        break;
        //    case "MassPlant":
        //        monologue.DisplayLog("괴물의 조직? 아니면 배설물?\n정체는 알 수 없지만 어딘가에 쓸 수는 있을 것 같다.");
        //        break;
        //    case "BoardPlant":
        //        monologue.DisplayLog("이 식물은 재생속도가 비정상적으로 빠르다.\n애초에 더이상 이 행성에 정상적인 것은 존재하지 않는다.");
        //        break;
        //    case "ThornPlant":
        //        monologue.DisplayLog("선인장의 변종으로 예상했지만 아니었다.\n이 식물의 가시 성분은 식물조직보다는 동물의 뿔에 가깝다.");
        //        break;
        //    case "Trap01":
        //        if(state == 1)
        //        {
        //            monologue.DisplayLog("덫에 무언가가 잡힌 것 같다!\n아이템 획득을 눌러서 확인해보자.");
        //        }
        //        else
        //        {
        //            monologue.DisplayLog("괴식물의 가시로 만든 덫이다.\n괴식물의 둥지 근처에 설치하면 무언가 잡힐지도 모른다.");
        //        }
        //        break;
        //    case "FruitPlant":
        //        monologue.DisplayLog("이 식물의 열매는 먹을 수 있을 것 같다.");
        //        break;
        //}
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        if (isTumor == false)
        {
            switch (plantName)
            {
                case "StickPlant":
                    temp = !inventory.isFull(1, "Item_Stick", 2);
                    break;
                case "MassPlant":
                    temp = !inventory.isFull(1, "Item_Mass", 1);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, "Item_Board", 2);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, "Item_Thorn", 3);
                    break;
                case "Trap01":
                    temp = !inventory.isFull(1, "Item_Heart", 1);
                    break;
                case "FruitPlant":
                    temp = !inventory.isFull(1, "Item_Fruit", 1);
                    break;
            }
        }
        else
        {
            switch (plantName)
            {
                case "StickPlant":
                    temp = !inventory.isFull(1, "Item_Tumor", 2);
                    break;
                case "BoardPlant":
                    temp = !inventory.isFull(1, "Item_Tumor", 2);
                    break;
                case "ThornPlant":
                    temp = !inventory.isFull(1, "Item_Tumor", 6);
                    break;
            }
        }
        return temp;
    }

    public override void SetTumor()
    {
        if(plantName != "StickPlant" && plantName != "BoardPlant" && plantName != "ThornPlant")
        {
            monologue.DisplayLog("종양을 심을 수 없는 식물이다.");
            return;
        }

        if(state != 2)
        {
            monologue.DisplayLog("식물의 상태가 종양을 심을 수 없는 상태군.\n수확 후 식물이 기력을 잃은 상태일때 다시 시도해보자.");
            return;
        }

        state = 4;
        objectTimer = 0;
        inventory.DeleteItem("Item_TumorSeed");
        animaitor.SetInteger("State", state);

        //if (state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
        //{
        //    state = 4;
        //    growthTimer = 0;
        //    inventory.DeleteItem(Inventory.Item.TumorSeed);
        //    animaitor.SetInteger("State", state);
        //}
        //else
        //{
        //    monologue.DisplayLog("식물이 다 자라서 종양을 설치할 수 없게 돼버렸군.\n일단 채집을 한 후에 다시 시도해보자.");
        //}
    }

    public override void Init()
    {
        if(animaitor == null)
        {
            animaitor = GetComponent<Animator>();
        }

        if (objectTimer >= growthTime)
        {
            objectTimer = 0;
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
    }

    void Update ()
    {
        //if (isLoadByManager == true)
        //{
        //    if (growthTimer >= growthTime)
        //    {
        //        growthTimer = 0;
        //        if (state == 4)
        //        {
        //            state = 5;
        //        }
        //        else
        //        {
        //            state = 1;
        //        }
        //        animaitor.SetInteger("State", state);
        //        animaitor.SetBool("isGrowSkip", true);
        //    }
        //    isLoadByManager = false;
        //}

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
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
                    {
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                        {
                            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Nest", "Nest01") == true)
                            {
                                objectTimer += Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else
            {
                objectTimer += Time.deltaTime;
            }
            
            if (objectTimer >= growthTime)
            {
                objectTimer = 0;
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

        if ((state == 1 || state == 5) && objectTimer != 0)
        {
            objectTimer = 0;
        }
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
    //    {
    //        interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
    //    }
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
    //    {
    //        interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
    //    }
    //}

    public override void Gather()
    {
        if (isGatherPossible == false)
        {
            if (plantName == "Trap01")
            {
                monologue.DisplayLog("아직 덫에 아무 것도 잡히지 않았다.");
            }
            else
            {
                monologue.DisplayLog("아직 채집할 수 없어.\n조금 더 자란 다음에 채집하는게 좋겠군.");
            }
        }
        else if (InventoryCheck() == true)
        {
            if (energyGauge.GetAmount() < 5)
            {
                monologue.DisplayLog("에너지가 부족해서 채집할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
                return;
            }
            GatherStart();
            player.GetComponent<PlayerInteraction>().GatherPlant(GatherAnimationType);
        }
        else
        {
            monologue.DisplayLog("인벤토리 공간이 부족하군.\n채집하기 전에 필요없는 아이템을 버리는게 좋겠어.");
        }
    }

    public override void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        interactionMenu.AddMenu(InteractionMenu.MenuItem.Gather);
        if (isGatherPossible == false)
        {
            if (state == 2 && (plantName == "StickPlant" || plantName == "BoardPlant" || plantName == "ThornPlant"))
            {
                if (inventory.HasItem("Item_TumorSeed") == true)
                {
                    interactionMenu.AddMenu(InteractionMenu.MenuItem.Tumor);
                }
            }
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, MenuTargetType, GetComponent<SpriteRenderer>().sprite, w, h);
    }

    //public override void SelectMenu(InteractionMenu.MenuItem m)
    //{
    //    switch (m)
    //    {
    //        case InteractionMenu.MenuItem.Gather:
    //            Gather();
    //            break;
    //        case InteractionMenu.MenuItem.Tumor:
    //            SetTumor();
    //            break;
    //        case InteractionMenu.MenuItem.Remove:
    //            RemoveObject();
    //            break;
    //        case InteractionMenu.MenuItem.Examine:
    //            Examine();
    //            break;
    //    }
    //}

    //이하 애니메이션 이벤트에서 사용하는 메소드들
    public void SetGatherPossibleFalse()
    {
        isGatherPossible = false;
    }
    public void SetGatherPossibleTrue()
    {
        isGatherPossible = true;
    }
    public void SetTrapOn()
    {
        state = 0;
        animaitor.SetInteger("State", state);
    }
}
