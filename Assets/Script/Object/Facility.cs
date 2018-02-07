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
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.OnOff);
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.Remove);
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.Make);
                        break;
                    case "EscapePod":
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.Research);
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.Sleep);
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.Make);
                        break;
                }
            }
            else if (GetComponent<FacilityBalloon>().isMakeFinish == true)
            {
                interactionIcon.AddIcon(global::InteractionIcon.Icon.Gather);
            }
            else if (GetComponent<FacilityBalloon>().isMake == true)
            {
                interactionIcon.AddIcon(global::InteractionIcon.Icon.Dump);
            }
        }
        else if(facilityName != "EscapePod")
        {
            interactionIcon.AddIcon(global::InteractionIcon.Icon.OnOff);
            interactionIcon.AddIcon(global::InteractionIcon.Icon.Remove);
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
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Make);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Dump);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.OnOff);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Remove);
                    break;
                case "EscapePod":
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Make);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Sleep);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Dump);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Research);
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
                popupWindow.AddItem(global::Inventory.Item.Facility01);
                popupWindow.AddItem(global::Inventory.Item.Trap01);
                popupWindow.AddItem(global::Inventory.Item.Battery);
                popupWindow.AddItem(global::Inventory.Item.TumorSeed);
                popupWindow.AddItem(global::Inventory.Item.Bulb01);
                popupWindow.AddItem(global::Inventory.Item.StickSeed);
                popupWindow.AddItem(global::Inventory.Item.BoardSeed);
                popupWindow.AddItem(global::Inventory.Item.ThornSeed);
                popupWindow.AddItem(global::Inventory.Item.Grinder01);
                popupWindow.OpenWindow(this.gameObject);
                break;
            case "Grinder01":
                popupWindow.ClearItemList();
                popupWindow.AddItem(global::Inventory.Item.Mass);
                popupWindow.OpenWindow(this.gameObject);
                break;
            case "EscapePod":
                popupWindow.ClearItemList();
                popupWindow.AddItem(global::Inventory.Item.Food);
                popupWindow.AddItem(global::Inventory.Item.Oxygen);
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
}
