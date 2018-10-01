using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation; //인터렉션메뉴에 나오는 설명
    [TextArea]
    public string ExamineText; //메뉴에서 조사하기를 누르면 나오는 대사
    public string MenuTargetType; //인터렉션 메뉴에서 구분을 위해 사용하는 타입

    protected InteractionIcon interactionIcon;
    protected InteractionMenu interactionMenu;
    protected Inventory inventory;
    protected EnergyGauge energyGauge;
    protected Monologue monologue;
    protected int sceneNum;

    protected void LoadMenuUIAndSeneNum()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();
        monologue = GameObject.Find("Player").transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            //interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
            DisplayIcon();
        }
    }

    public void DisplayIcon()
    {
        interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
            interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Portal);
        }
    }

    public void Examine()
    {
        monologue.DisplayLog(ExamineText);
    }

    public void RemoveObject()
    {
        if (energyGauge.GetAmount() < 5)
        {
            monologue.DisplayLog("에너지가 부족해서 철거할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
            return;
        }
        energyGauge.SetAmount(-5);
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, MenuTargetType, GetComponent<SpriteRenderer>().sprite, w, h);
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
