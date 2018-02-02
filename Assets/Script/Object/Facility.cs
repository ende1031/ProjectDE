using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public string facilityName = "TempFacility";

    GameObject InteractionIcon;
    GameObject Inventory;
    GameObject Timer;
    GameObject PopupWindow;
    GameObject ResearchWindow;

    Animator animaitor;

    public bool isOn = true;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");
        Timer = GameObject.Find("Timer");
        PopupWindow = GameObject.Find("PopupWindow");
        ResearchWindow = GameObject.Find("ResearchWindow");

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
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
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
                        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.OnOff);
                        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Make);
                        break;
                    case "EscapePod":
                        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Research);
                        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Sleep);
                        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Make);
                        break;
                }
            }
            else if (GetComponent<FacilityBalloon>().isMakeFinish == true)
            {
                InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Gather);
            }
            else if (GetComponent<FacilityBalloon>().isMake == true)
            {
                InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Dump);
            }
        }
        else if(facilityName != "EscapePod")
        {
            InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.OnOff);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            switch (facilityName)
            {
                case "TempFacility":
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Make);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Dump);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.OnOff);
                    break;
                case "EscapePod":
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Make);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Sleep);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Dump);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Research);
                    break;
            }
        }
    }

    public void OpenProductionWindow()
    {
        switch (facilityName)
        {
            case "TempFacility":
                PopupWindow.GetComponent<PopupWindow>().ClearItemList();
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Trap01);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Battery);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Bulb01);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.StickSeed);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.BoardSeed);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.ThornSeed);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Facility01);
                PopupWindow.GetComponent<PopupWindow>().OpenWindow(this.gameObject);
                break;
            case "EscapePod":
                PopupWindow.GetComponent<PopupWindow>().ClearItemList();
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Food);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Oxygen);
                PopupWindow.GetComponent<PopupWindow>().OpenWindow(this.gameObject);
                break;
        }
    }

    public void Sleep()
    {
        if (facilityName == "EscapePod")
        {
            SceneObjectManager.instance.SaveObject();
            Timer.GetComponent<Timer>().ResetTimer();
            SceneObjectManager.instance.SleepAfter();
            SceneChanger.instance.FadeAndLoadScene(SceneChanger.instance.GetSceneName(), Grid.instance.PlayerGrid());
        }
    }

    public void Research()
    {
        if (facilityName == "EscapePod")
        {
            ResearchWindow.GetComponent<ResearchWindow>().OpenWindow();
        }
    }

    public void OnOff()
    {
        if (facilityName != "EscapePod")
        {
            isOn = !isOn;
            animaitor.SetBool("isOn", isOn);
            InteractionIcon.GetComponent<InteractionIcon>().DeleteAllIcons();
            DisplayIcon();
        }
    }
}
