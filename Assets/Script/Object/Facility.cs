﻿using System.Collections;
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

    public bool isOn = true;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        popupWindow = GameObject.Find("PopupWindow").GetComponent<PopupWindow>();
        researchWindow = GameObject.Find("ResearchWindow").GetComponent<ResearchWindow>();

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
                        interactionIcon.AddIcon(global::InteractionIcon.Icon.OnOff);
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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            switch (facilityName)
            {
                case "TempFacility":
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Make);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Gather);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.Dump);
                    interactionIcon.DeleteIcon(global::InteractionIcon.Icon.OnOff);
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
}
