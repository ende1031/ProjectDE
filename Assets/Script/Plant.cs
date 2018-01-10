using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    GameObject InteractionIcon;
    GameObject Inventory;

    public bool isGatherPossible;
    public string plantName = "StickPlant";
    public int GatherAnimationType = 1;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");

        if(plantName == "StickPlant")
        {
            isGatherPossible = true;
            GatherAnimationType = 1;
        }
    }

    public void GetItem()
    {
        switch (plantName)
        {
            case "StickPlant":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Stick);
                break;
        }
    }

    void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isGatherPossible && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Gather);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
        }
    }
}
