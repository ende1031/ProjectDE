using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Inventory inventory;
    int sceneNum;
    Monologue monologue;
    EnergyGauge energyGauge;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        monologue = GameObject.Find("Player").transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
        }
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, "Nest", GetComponent<SpriteRenderer>().sprite, w, h);
    }

    public void RemoveObject()
    {
        if (energyGauge.GetAmount() < 5)
        {
            monologue.DisplayLog("에너지가 부족해서 제거할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
            return;
        }
        energyGauge.SetAmount(-5);
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }

    void Examine()
    {
        monologue.DisplayLog("둥지 내부에서 괴물이 생산되고 있는 것으로 추정된다.\n근처에 덫을 설치해보자.");
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
    {
        switch (m)
        {
            case InteractionMenu.MenuItem.Remove:
                RemoveObject();
                break;
            case InteractionMenu.MenuItem.Examine:
                Examine();
                break;
        }
    }
}
