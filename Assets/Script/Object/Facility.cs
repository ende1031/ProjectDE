﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : SceneObject
{
    //public string ObjectName;
    //[TextArea]
    //public string ObjectExplanation;

    public string facilityName = "TempFacility";

    GrinderWindow grinderWindow;
    
    PopupWindow popupWindow;
    ResearchWindow researchWindow;
    Animator animaitor;
    GameObject Player;
    ReportUI reportUI;
    //InteractionIcon interactionIcon;
    //InteractionMenu interactionMenu;
    //Monologue monologue;
    //Inventory inventory;
    //EnergyGauge energyGauge;
    //int sceneNum;
    //public int state = 1;
    //public bool isLoadByManager = false;

    float offTimer = 0;
    float sleepTimer = 0;

    // state 0:꺼짐, 1:평상시, 2:제작중, 3:제작끝, 4:죽음

    void Start ()
    {
        LoadMenuUIAndSeneNum();
        //interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        //interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        //inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        //sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        //monologue = Player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        //energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();

        grinderWindow = GameObject.Find("GrinderWindow").GetComponent<GrinderWindow>();
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
        Player = GameObject.Find("Player");
        reportUI = GameObject.Find("ReportUI").GetComponent<ReportUI>();

        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
    }

    public override void Init()
    {
        if (state != 0)
        {
            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                {
                    Ruin();
                }
            }
        }
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }

        GetComponent<FacilityBalloon>().isInit = true;
    }

    void Update ()
    {
        //if (isLoadByManager == true)
        //{
        //    if (state != 0)
        //    {
        //        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
        //        {
        //            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
        //            {
        //                Ruin();
        //            }
        //        }
        //    }
        //    if (animaitor != null)
        //    {
        //        animaitor.SetInteger("State", state);
        //    }
        //    isLoadByManager = false;
        //}

        if (RuinCheck() == true)
        {
            Ruin();
        }

        if(offTimer < 0.3f)
        {
            offTimer += Time.deltaTime;
        }
        if(sleepTimer < 0.5f)
        {
            sleepTimer += Time.deltaTime;
        }
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
    //    {
    //        DisplayIcon();
    //    }
    //}

    public override void DisplayIcon()
    {
        if (state == 0 && facilityName != "EscapePod")
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
            return;
        }

        interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
    //    {
    //        interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
    //        interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
    //    }
    //}

    public void OpenProductionWindow()
    {
        switch (facilityName)
        {
            case "TempFacility":
            case "EscapePod":
                popupWindow.ClearItemList();
                popupWindow.AddItem("Item_Hose");
                popupWindow.AddItem("Item_Sawtooth");
                popupWindow.AddItem("Item_Food");
                popupWindow.AddItem("Item_Facility01");
                popupWindow.AddItem("Item_Grinder01");
                popupWindow.AddItem("Item_Bulb");
                popupWindow.AddItem("Item_NyxCollector01");
                popupWindow.AddItem("Item_Trap01");
                popupWindow.AddItem("Item_TumorSeed");
                popupWindow.AddItem("Item_StickSeed");
                popupWindow.AddItem("Item_BoardSeed");
                popupWindow.AddItem("Item_ThornSeed");
                popupWindow.AddItem("Item_FruitSeed");
                popupWindow.OpenWindow(this.gameObject);
                break;
        }
    }

    public void OpenGrinderWindow()
    {
        grinderWindow.OpenWindow(this.gameObject);
    }

    void Sleep()
    {
        if (facilityName != "EscapePod")
        {
            return;
        }

        if (sleepTimer < 0.5f)
        {
            monologue.DisplayLog("지금은 졸리지 않아.\n일어난지 얼마 안됐는데 벌써 잘 수는 없지.");
            return;
        }

        Player.GetComponent<PlayerMove>().SetMovePossible(false);
        reportUI.AddDay();
        SceneObjectManager.instance.SaveObject();
        SceneObjectManager.instance.SleepAfter();
        SaveAndLoad.instance.SaveGame(); //Json
        SceneChanger.instance.FadeAndLoadScene(SceneChanger.instance.GetSceneName(), Grid.instance.PlayerGrid());
    }

    public void Research()
    {
        if (facilityName == "EscapePod")
        {
            researchWindow.OpenWindow();
        }
    }

    public override void OnOff()
    {
        if(offTimer < 0.3f)
        {
            return;
        }

        if (facilityName != "EscapePod")
        {
            if(state == 0)
            {
                state = 1;
                SoundManager.instance.PlaySE(31);
            }
            else
            {
                state = 0;
                SoundManager.instance.PlaySE(30);
            }
            animaitor.SetInteger("State", state);
            interactionIcon.DeleteAllIcons();
            DisplayIcon();
            offTimer = 0;
        }
    }

    public override void RemoveObject()
    {
        if (facilityName != "EscapePod")
        {
            if (energyGauge.GetAmount() < 5)
            {
                monologue.DisplayLog("에너지가 부족해서 철거할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
                return;
            }
            energyGauge.SetAmount(-5);
            interactionIcon.DeleteAllIcons();
            SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
        }
    }

    bool RuinCheck()
    {
        if (facilityName != "EscapePod" && state != 0)
        {
            if (Mathf.Abs(Grid.instance.PlayerGrid() - Grid.instance.PosToGrid(transform.position.x)) > 4)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
                {
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void Ruin()
    {
        GetComponent<FacilityBalloon>().Dump();
        state = 4;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
    }

    public override void Repair()
    {
        if (state != 4)
        {
            return;
        }
        if (energyGauge.GetAmount() < 5)
        {
            monologue.DisplayLog("에너지가 부족해서 수리할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
            return;
        }

        energyGauge.SetAmount(-5);
        state = 1;
        SoundManager.instance.PlaySE(32);
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
    }

    public override void Examine()
    {
        if(state == 4)
        {
            monologue.DisplayLog("괴물의 습격을 받은 것 같다.\n에너지를 소모해서 수리하도록 하자.");
            return;
        }
        monologue.DisplayLog(ExamineText);
        //switch(facilityName)
        //{
        //    case "EscapePod":
        //        monologue.DisplayLog("내가 타고 온 탈출포드이다.\n다행히 손상이 심한 것 같지는 않다.\n아이템 연구나 제작을 할 수 있다.");
        //        break;
        //    case "TempFacility":
        //        monologue.DisplayLog("급하게 만든 간이 워크벤치이다.\n재료와 닉스입자를 소모해서 아이템을 제작할 수 있다.");
        //        break;
        //    case "Grinder01":
        //        monologue.DisplayLog("아이템을 분해할 수 있는 분해기이다.\n분해를 통해 재료아이템을 얻을 수 있다.");
        //        break;
        //}
    }

    public override void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        if(state == 3)
        {
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Gather);
        }
        if (facilityName == "EscapePod")
        {
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Research);
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Sleep);
        }
        switch (state)
        {
            case 1:
                if (facilityName == "Grinder01")
                {
                    interactionMenu.AddMenu(InteractionMenu.MenuItem.Grind);
                }
                else
                {
                    interactionMenu.AddMenu(InteractionMenu.MenuItem.Make);
                }
                if (facilityName != "EscapePod")
                {
                    interactionMenu.AddMenu(InteractionMenu.MenuItem.Off);
                }
                break;
            case 2:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Cancle);
                break;
            case 4:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Repair);
                break;
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        if (facilityName != "EscapePod")
        {
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);
        }

        if (facilityName != "EscapePod")
        {
            float w = GetComponent<SpriteRenderer>().sprite.rect.width;
            float h = GetComponent<SpriteRenderer>().sprite.rect.height;
            interactionMenu.OpenMenu(this.gameObject, MenuTargetType, GetComponent<SpriteRenderer>().sprite, w, h);
        }
        else
        {
            interactionMenu.OpenMenu(this.gameObject, MenuTargetType, transform.Find("render").GetComponent<SpriteRenderer>().sprite, 341 * 0.6f, 382 * 0.6f);
        }
    }

    public override void Gather()
    {
        if (GetComponent<FacilityBalloon>().InventoryCheck() == false)
        {
            monologue.DisplayLog("인벤토리 공간이 부족하군.\n아이템을 획득하려면 인벤토리에 빈 공간이 필요해.");
            return;
        }
        GetComponent<FacilityBalloon>().GetItem();
    }

    public override void SelectMenu(InteractionMenu.MenuItem m)
    {
        switch (m)
        {
            case InteractionMenu.MenuItem.Make:
                OpenProductionWindow();
                break;
            case InteractionMenu.MenuItem.Grind:
                OpenGrinderWindow();
                break;
            case InteractionMenu.MenuItem.Gather:
                Gather();
                break;
            case InteractionMenu.MenuItem.Cancle:
                GetComponent<FacilityBalloon>().Dump();
                break;
            case InteractionMenu.MenuItem.Research:
                Research();
                break;
            case InteractionMenu.MenuItem.Sleep:
                Sleep();
                break;
            case InteractionMenu.MenuItem.Off:
                OnOff();
                break;
            case InteractionMenu.MenuItem.Remove:
                RemoveObject();
                break;
            case InteractionMenu.MenuItem.Repair:
                Repair();
                break;
            case InteractionMenu.MenuItem.Examine:
                Examine();
                break;
        }
    }
}
