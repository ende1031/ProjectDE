using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : SceneObject
{
    /*
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Inventory inventory;
    Monologue monologue;
    EnergyGauge energyGauge;
    int sceneNum;
    */

    void Start()
    {
        LoadMenuUIAndSeneNum();
    }

    //void Update ()
    //{

    //}

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
    //    {
    //        interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
    //    }
    //}

    //public void DisplayIcon()
    //{
    //    interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
    //    }
    //}

    //void Examine()
    //{
    //    monologue.DisplayLog("괴물은 빛을 싫어한다.\n이 전구 근처에는 다른 시설을 설치해도 안전할 것이다.");
    //}

    //public void RemoveObject()
    //{
    //    if (energyGauge.GetAmount() < 5)
    //    {
    //        monologue.DisplayLog("에너지가 부족해서 철거할 수 없어.\n탈출포드로 돌아가서 잠을 자도록 하자.");
    //        return;
    //    }
    //    energyGauge.SetAmount(-5);
    //    interactionIcon.DeleteAllIcons();
    //    SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    //}

    //public void OpenMenu()
    //{
    //    interactionMenu.ClearMenu();
    //    interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);
        
    //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
    //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

    //    float w = GetComponent<SpriteRenderer>().sprite.rect.width;
    //    float h = GetComponent<SpriteRenderer>().sprite.rect.height;
    //    interactionMenu.OpenMenu(this.gameObject, "Bulb", GetComponent<SpriteRenderer>().sprite, w, h);
    //}

    //public void SelectMenu(InteractionMenu.MenuItem m)
    //{
    //    switch (m)
    //    {
    //        case InteractionMenu.MenuItem.Remove:
    //            RemoveObject();
    //            break;
    //        case InteractionMenu.MenuItem.Examine:
    //            Examine();
    //            break;
    //    }
    //}
}
