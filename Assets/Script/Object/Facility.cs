using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    public string facilityName = "TempFacility";

    GrinderWindow grinderWindow;
    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Monologue monologue;
    Inventory inventory;
    PopupWindow popupWindow;
    ResearchWindow researchWindow;
    Animator animaitor;
    GameObject Player;
    EnergyGauge energyGauge;
    int sceneNum;

    public bool isLoadByManager = false;

    public int state = 1; // 0:꺼짐, 1:평상시, 2:제작중, 3:제작끝, 4:죽음

    float offTimer = 0;
    float sleepTimer = 0;

    void Start ()
    {
        grinderWindow = GameObject.Find("GrinderWindow").GetComponent<GrinderWindow>();
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        Player = GameObject.Find("Player");
        monologue = Player.transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();

        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
    }
	
	void Update ()
    {
        if (isLoadByManager == true)
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
            isLoadByManager = false;
        }

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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            DisplayIcon();
        }
    }

    public void DisplayIcon()
    {
        if (state != 0)
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
        }
        else if(facilityName != "EscapePod")
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
            interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
        }
    }

    public void OpenProductionWindow()
    {
        switch (facilityName)
        {
            case "TempFacility":
                popupWindow.ClearItemList();
                popupWindow.AddItem(Inventory.Item.Facility01);
                popupWindow.AddItem(Inventory.Item.Trap01);
                popupWindow.AddItem(Inventory.Item.Battery);
                popupWindow.AddItem(Inventory.Item.TumorSeed);
                popupWindow.AddItem(Inventory.Item.Bulb01);
                popupWindow.AddItem(Inventory.Item.StickSeed);
                popupWindow.AddItem(Inventory.Item.BoardSeed);
                popupWindow.AddItem(Inventory.Item.ThornSeed);
                popupWindow.AddItem(Inventory.Item.Grinder01);
                popupWindow.AddItem(Inventory.Item.NyxCollector01);
                popupWindow.OpenWindow(this.gameObject);
                break;
            case "EscapePod":
                popupWindow.ClearItemList();
                popupWindow.AddItem(Inventory.Item.Facility01);
                popupWindow.AddItem(Inventory.Item.Food);
                popupWindow.AddItem(Inventory.Item.Oxygen);
                popupWindow.OpenWindow(this.gameObject);
                break;
        }
    }

    public void OpenGrinderWindow()
    {
        grinderWindow.OpenWindow(this.gameObject);
    }

    public void Sleep()
    {
        if (facilityName == "EscapePod")
        {
            SceneObjectManager.instance.SaveObject();
            SceneObjectManager.instance.SleepAfter();
            SceneChanger.instance.FadeAndLoadScene(SceneChanger.instance.GetSceneName(), Grid.instance.PlayerGrid());
        }
    }

    public void Research()
    {
        if (facilityName == "EscapePod")
        {
            researchWindow.OpenWindow();
        }
    }

    public void OnOff()
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
            }
            else
            {
                state = 0;
            }
            animaitor.SetInteger("State", state);
            interactionIcon.DeleteAllIcons();
            DisplayIcon();
            offTimer = 0;
        }
    }

    public void RemoveObject()
    {
        if (facilityName != "EscapePod")
        {
            if (energyGauge.GetAmount() < 5)
            {
                monologue.DisplayLog("에너지가 부족해서 철거할 수 없어.");
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

    void Repair()
    {
        if(state == 4)
        {
            if (energyGauge.GetAmount() < 5)
            {
                monologue.DisplayLog("에너지가 부족해서 수리할 수 없어.");
                return;
            }
            energyGauge.SetAmount(-5);
            state = 1;
            if (animaitor != null)
            {
                animaitor.SetInteger("State", state);
            }
        }
    }

    void Examine()
    {
        if(state == 4)
        {
            monologue.DisplayLog("괴물의 습격을 받은 것 같다.\n에너지를 소모해서 수리하도록 하자.");
            return;
        }
        switch(facilityName)
        {
            case "EscapePod":
                monologue.DisplayLog("내가 타고 온 탈출포드이다.\n다행히 손상이 심한 것 같지는 않다.\n아이템 연구나 제작을 할 수 있다.");
                break;
            case "TempFacility":
                monologue.DisplayLog("급하게 만든 간이 워크벤치이다.\n재료와 닉스입자를 소모해서 아이템을 제작할 수 있다.");
                break;
            case "Grinder01":
                monologue.DisplayLog("아이템을 분해할 수 있는 분해기이다.\n분해를 통해 재료아이템을 얻을 수 있다.");
                break;
        }
    }

    public void OpenMenu()
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
            interactionMenu.OpenMenu(this.gameObject, "Facility", GetComponent<SpriteRenderer>().sprite, w, h);
        }
        else
        {
            interactionMenu.OpenMenu(this.gameObject, "Facility", transform.Find("render").GetComponent<SpriteRenderer>().sprite, 341 * 0.6f, 382 * 0.6f);
        }
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
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
                if (GetComponent<FacilityBalloon>().InventoryCheck() == true)
                {
                    GetComponent<FacilityBalloon>().GetItem();
                }
                else
                {
                    monologue.DisplayLog("인벤토리 공간이 부족하군.\n아이템을 획득하려면 인벤토리에 빈 공간이 필요해.");
                    return;
                }
                break;
            case InteractionMenu.MenuItem.Cancle:
                GetComponent<FacilityBalloon>().Dump();
                break;
            case InteractionMenu.MenuItem.Research:
                Research();
                break;
            case InteractionMenu.MenuItem.Sleep:
                if (sleepTimer >= 0.5f)
                {
                    Player.GetComponent<PlayerMove>().SetMovePossible(false);
                    Sleep();
                }
                else
                {
                    monologue.DisplayLog("지금은 졸리지 않아.\n일어난지 얼마 안됐는데 벌써 잘 수는 없지.");
                }
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
