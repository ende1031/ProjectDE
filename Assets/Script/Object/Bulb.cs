using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    GameObject InteractionIcon;
    GameObject Inventory;

    GameObject BulbLight;
    Animator animaitor;

    public bool isOn = true;
    public bool isLoadByManager;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");
        BulbLight = transform.Find("Light").gameObject;
        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetBool("isOn", isOn);
        }
    }
	
	void Update ()
    {
		if(isLoadByManager == true)
        {
            if (animaitor != null)
            {
                animaitor.SetBool("isOn", isOn);
            }
            BulbLight.SetActive(isOn);
            isLoadByManager = false;
        }
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
        InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.OnOff);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.OnOff);
        }
    }

    public void OnOff()
    {
        isOn = !isOn;
        animaitor.SetBool("isOn", isOn);
        BulbLight.SetActive(isOn);
        InteractionIcon.GetComponent<InteractionIcon>().DeleteAllIcons();
        DisplayIcon();
    }
}
