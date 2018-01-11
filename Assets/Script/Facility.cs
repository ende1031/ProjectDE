using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public string facilityName = "TempFacility";

    GameObject InteractionIcon;
    GameObject Inventory;
    GameObject FadeOut;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");
        FadeOut = GameObject.Find("FadeOut");
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            switch (facilityName)
            {
                case "TempFacility":
                    InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Input);
                    break;
                case "EscapePod":
                    InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Input);
                    InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Sleep);
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            switch (facilityName)
            {
                case "TempFacility":
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Input);
                    break;
                case "EscapePod":
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Input);
                    InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Sleep);
                    break;
            }
        }
    }

    public void DeleteItem()
    {
        switch (facilityName)
        {
            case "TempFacility":
                if (Inventory.GetComponent<Inventory>().DeleteItem(global::Inventory.Item.Stick) == true)
                {
                    Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.RedStick);
                }
                break;
            case "EscapePod":
                Inventory.GetComponent<Inventory>().DeleteItem(global::Inventory.Item.Stick);
                break;
        }
    }

    public void Sleep()
    {
        switch (facilityName)
        {
            case "EscapePod":
                //작동중이던 시설 파괴, 심어놓은 농작물 최대성장
                FadeOut.GetComponent<SleepFade>().Sleep();

                break;
        }
    }
}
