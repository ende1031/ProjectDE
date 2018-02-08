using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public string facilityName = "TempFacility";

    InteractionIcon interactionIcon;
    Inventory inventory;
    PopupWindow popupWindow;
    ResearchWindow researchWindow;
    Animator animaitor;
    int sceneNum;

    public bool isOn = true;
    public bool isLoadByManager = false;
    public bool isAlive = true;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetBool("isOn", isOn);
        }
    }
	
	void Update ()
    {
        if (isLoadByManager == true)
        {
            if (isOn == true)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01", true) == false)
                {
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                    {
                        isAlive = false;
                    }
                }
            }
            isLoadByManager = false;
        }

        if (RuinCheck() == true)
        {
            isAlive = false;
        }
        if(isAlive == false)
        {
            ruin();
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
        if (isOn == true)
        {
            if (GetComponent<FacilityBalloon>().isMake == false && GetComponent<FacilityBalloon>().isMakeFinish == false)
            {
                switch (facilityName)
                {
                    case "TempFacility":
                    case "Grinder01":
                        interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
                        interactionIcon.AddIcon(InteractionIcon.Icon.Remove);
                        interactionIcon.AddIcon(InteractionIcon.Icon.Make);
                        break;
                    case "EscapePod":
                        interactionIcon.AddIcon(InteractionIcon.Icon.Research);
                        interactionIcon.AddIcon(InteractionIcon.Icon.Sleep);
                        interactionIcon.AddIcon(InteractionIcon.Icon.Make);
                        break;
                }
            }
            else if (GetComponent<FacilityBalloon>().isMakeFinish == true)
            {
                interactionIcon.AddIcon(InteractionIcon.Icon.Gather);
            }
            else if (GetComponent<FacilityBalloon>().isMake == true)
            {
                interactionIcon.AddIcon(InteractionIcon.Icon.Dump);
            }
        }
        else if(facilityName != "EscapePod")
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
            interactionIcon.AddIcon(InteractionIcon.Icon.Remove);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            switch (facilityName)
            {
                case "TempFacility":
                case "Grinder01":
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Make);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Gather);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Dump);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Remove);
                    break;
                case "EscapePod":
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Make);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Gather);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Sleep);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Dump);
                    interactionIcon.DeleteIcon(InteractionIcon.Icon.Research);
                    break;
            }
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
            case "Grinder01":
                popupWindow.ClearItemList();
                popupWindow.AddItem(Inventory.Item.Mass);
                popupWindow.OpenWindow(this.gameObject);
                break;
            case "EscapePod":
                popupWindow.ClearItemList();
                popupWindow.AddItem(Inventory.Item.Food);
                popupWindow.AddItem(Inventory.Item.Oxygen);
                popupWindow.OpenWindow(this.gameObject);
                break;
        }
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
        if (facilityName != "EscapePod")
        {
            isOn = !isOn;
            animaitor.SetBool("isOn", isOn);
            interactionIcon.DeleteAllIcons();
            DisplayIcon();
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
        if (facilityName != "EscapePod" && isOn == true)
        {
            if (Mathf.Abs(Grid.instance.PlayerGrid() - Grid.instance.PosToGrid(transform.position.x)) > 4)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01", true) == false)
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

    void ruin()
    {
        SceneObjectManager.instance.ChangeObject(sceneNum, Grid.instance.PosToGrid(transform.position.x), new SceneObjectManager.SceneObject("Wreckage", "Wreckage"));
    }
}
