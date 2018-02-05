﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    GameObject InteractionIcon;
    GameObject Inventory;

    Animator animaitor;

    int sceneNum;

    public bool isGatherPossible;
    public string plantName = "StickPlant";
    public int GatherAnimationType = 1;
    public int state; //0:자라는 중(덫설치후 대기중), 1:채집가능(덫잡힘), 2:채집후, 3:덫설치중

    public float growthTime; //성장하는데 걸리는 시간(초)
    public float growthTimer = 0;

    void Start ()
    {
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        InteractionIcon = GameObject.Find("InteractionIcon");
        Inventory = GameObject.Find("Inventory");

        animaitor = GetComponent<Animator>();
        animaitor.SetInteger("State", state);
        animaitor.SetBool("isGathering", false); //플레이어가 채집을 하는것인지, 아니면 바로 애니메이션 전환을 할지
    }

    public void GatherStart()
    {
        state = 2;
        animaitor.SetBool("isGathering", true);
        animaitor.SetInteger("State", state);
    }

    public void GetItem()
    {
        switch (plantName)
        {
            case "StickPlant":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Stick, 2);
                break;
            case "MassPlant":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Mass, 1);
                SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                break;
            case "BoardPlant":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Board, 1);
                break;
            case "ThornPlant":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Thorn, 5);
                break;
            case "Trap01":
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Hose, 1);
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Heart, 1);
                Inventory.GetComponent<Inventory>().GetItem(global::Inventory.Item.Mass, 1);
                SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
                break;
        }
        isGatherPossible = false;

        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
        }
    }

    public bool InventoryCheck()
    {
        bool temp = true;
        switch (plantName)
        {
            case "StickPlant":
                temp = !Inventory.GetComponent<Inventory>().isFull(1, global::Inventory.Item.Stick, 2);
                break;
            case "MassPlant":
                temp = !Inventory.GetComponent<Inventory>().isFull(1, global::Inventory.Item.Mass, 1);
                break;
            case "BoardPlant":
                temp = !Inventory.GetComponent<Inventory>().isFull(1, global::Inventory.Item.Board, 1);
                break;
            case "ThornPlant":
                temp = !Inventory.GetComponent<Inventory>().isFull(1, global::Inventory.Item.Thorn, 5);
                break;
            case "Trap01":
                temp = !Inventory.GetComponent<Inventory>().isFull(2, global::Inventory.Item.Hose, 1, global::Inventory.Item.Heart, 1, global::Inventory.Item.Mass, 1);
                break;
        }
        return temp;
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleFalse()
    {
        isGatherPossible = false;
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetGatherPossibleTrue()
    {
        isGatherPossible = true;
        if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
        {
            InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Gather);
        }
    }

    //애니메이션 이벤트에서 사용하는 함수
    public void SetTrapOn()
    {
        state = 0;
        animaitor.SetInteger("State", state);
    }

    void Update ()
    {
        if (plantName != "MassPlant")
        {
            Growth();
        }
    }

    void Growth()
    {
        if (state == 0 || state == 2)
        {
            if (plantName == "Trap01")
            {
                if (Mathf.Abs(Grid.instance.PlayerGrid() - Grid.instance.PosToGrid(transform.position.x)) > 4)
                {
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01", true) == false)
                    {
                        if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                        {
                            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Nest", "Nest01") == true)
                            {
                                growthTimer += Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else
            {
                growthTimer += Time.deltaTime;
            }
            
            if (growthTimer >= growthTime)
            {
                growthTimer = 0;
                state = 1;
                animaitor.SetInteger("State", state);
            }
        }

        if (state == 1 && growthTimer != 0)
        {
            growthTimer = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Inventory.GetComponent<Inventory>().isInventoryActive == false)
        {
            if (isGatherPossible == true)
            {
                InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Gather);
            }
            else
            {
                InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Gather);
            }
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
