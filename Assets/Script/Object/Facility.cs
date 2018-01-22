﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public string facilityName = "TempFacility";

    GameObject InteractionIcon;
    GameObject Inventory;
    GameObject Timer;
    GameObject PopupWindow;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");
        Timer = GameObject.Find("Timer");
        PopupWindow = GameObject.Find("PopupWindow");
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

    public void OpenProductionWindow()
    {
        switch (facilityName) //테스트용 코드
        {
            case "TempFacility":
                PopupWindow.GetComponent<PopupWindow>().ClearItemList();
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Battery);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Food);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Board);
                PopupWindow.GetComponent<PopupWindow>().OpenWindow();
                break;
            case "EscapePod":
                PopupWindow.GetComponent<PopupWindow>().ClearItemList();
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Stick);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Oxygen);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Food);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Battery);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Hose);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Board);
                PopupWindow.GetComponent<PopupWindow>().AddItem(global::Inventory.Item.Mass);
                PopupWindow.GetComponent<PopupWindow>().OpenWindow();
                break;
        }
    }

    public void DeleteItem()
    {
        switch (facilityName) //테스트용 코드
        {
            case "TempFacility":
                if (Inventory.GetComponent<Inventory>().DeleteItem(global::Inventory.Item.Stick) == true)
                {
                    Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Mass);
                }
                break;
            case "EscapePod":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Food, 10);
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Oxygen, 5);
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Battery, 15);
                break;
        }
    }

    public void Sleep()
    {
        switch (facilityName)
        {
            case "EscapePod":
                // 0.3초쯤 뒤에? 작동중이던 시설 파괴, 심어놓은 농작물 최대성장
                SceneObjectManager.instance.SaveObject();
                Timer.GetComponent<Timer>().ResetTimer();
                SceneObjectManager.instance.SleepAfter();
                SceneChanger.instance.FadeAndLoadScene(SceneChanger.instance.GetSceneName(), Grid.instance.PlayerGrid());
                break;
        }
    }
}
