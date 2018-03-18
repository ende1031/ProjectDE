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
    Timer timer;
    int sceneNum;

    //public bool isOn = true;
    public bool isLoadByManager = false;
    //public bool isAlive = true;

    public int state = 1; // 0:꺼짐, 1:평상시, 2:제작중, 3:제작끝, 4:죽음

    float offTimer = 0;

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
        timer = GameObject.Find("Timer").GetComponent<Timer>();

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
                        //isAlive = false;
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
            //isAlive = false;
            Ruin();
        }
        //if(state == 4)
        //{
        //    Ruin();
        //}

        if(offTimer < 0.3f)
        {
            offTimer += Time.deltaTime;
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
            //isOn = !isOn;
            if(state == 0)
            {
                state = 1;
                //animaitor.SetBool("isOn", true);
            }
            else
            {
                state = 0;
                //animaitor.SetBool("isOn", false);
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
        //SceneObjectManager.instance.ChangeObject(sceneNum, Grid.instance.PosToGrid(transform.position.x), new SceneObjectManager.SceneObject("Wreckage", "Wreckage"));

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
            state = 1;
            if (animaitor != null)
            {
                animaitor.SetInteger("State", state);
            }
        }
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

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
            case 3:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Gather);
                break;
            case 4:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Repair);
                break;
        }

        if (facilityName != "EscapePod")
        {
            interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);
        }

        //if (GetComponent<FacilityBalloon>().isMakeFinish == true)
        //{
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Gather);
        //}
        //if (facilityName == "EscapePod")
        //{
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Research);
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Sleep);
        //}
        //if (GetComponent<FacilityBalloon>().isMake == false && GetComponent<FacilityBalloon>().isMakeFinish == false)
        //{
        //    if (facilityName == "Grinder01")
        //    {
        //        interactionMenu.AddMenu(InteractionMenu.MenuItem.Grind);
        //    }
        //    else
        //    {
        //        interactionMenu.AddMenu(InteractionMenu.MenuItem.Make);
        //    }
        //    if (facilityName != "EscapePod")
        //    {
        //        interactionMenu.AddMenu(InteractionMenu.MenuItem.Off);
        //    }
        //}
        //else if (GetComponent<FacilityBalloon>().isMake == true)
        //{
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Cancle);
        //}
        //if (facilityName != "EscapePod")
        //{
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);
        //}

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
                if (timer.PercentOfTime() > 2) //하루의 2%이상 지난 시점부터 수면 가능
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
        }
    }
}
