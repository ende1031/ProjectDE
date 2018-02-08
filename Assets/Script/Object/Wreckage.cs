using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreckage : MonoBehaviour
{
    InteractionIcon interactionIcon;
    Inventory inventory;
    int sceneNum;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Remove);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Remove);
        }
    }

    public void RemoveObject()
    {
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }
}