﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Inventory inventory;
    GameObject Monologue;

    Animator animaitor;
    bool isGather = false;

    bool isTrigger = false;
    GameObject target;

    bool isInteractionPossible = true;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        Monologue = transform.Find("Monologue").gameObject;
    }
	
	void Update ()
    {
        if(isTrigger)
        {
            if (inventory.isInventoryActive == false && isInteractionPossible == true)
            {
                Interaction();
            }
        }

        //테스트용 코드
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.GetItem(global::Inventory.Item.Tumor, 1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            inventory.GetItem(global::Inventory.Item.Mass, 1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.GetItem(global::Inventory.Item.Mass, 50);
            inventory.GetItem(global::Inventory.Item.Tumor, 50);
            inventory.GetItem(global::Inventory.Item.Stick, 50);
            inventory.GetItem(global::Inventory.Item.Board, 50);
            inventory.GetItem(global::Inventory.Item.Thorn, 50);
            inventory.GetItem(global::Inventory.Item.Heart, 50);
            inventory.GetItem(global::Inventory.Item.Hose, 50);
        }
    }

    void PlayerDirection()
    {
        if (target.transform.position.x > transform.position.x)
        {
            GetComponent<PlayerMove>().SetDirection(1);
        }
        else if (target.transform.position.x < transform.position.x)
        {
            GetComponent<PlayerMove>().SetDirection(0);
        }
    }

    void Interaction()
    {
        if (target == null)
            return;

        if (target.gameObject.tag == "Plant")
        {
            if (Input.GetKeyDown(KeyCode.C) && target.GetComponent<Plant>().isGatherPossible == true)
            {
                PlayerDirection();
                if (target.GetComponent<Plant>().InventoryCheck() == true)
                {
                    GatherAnimation(target.GetComponent<Plant>().GatherAnimationType, true);
                    target.GetComponent<Plant>().GatherStart();
                }
                else
                {
                    Monologue.GetComponent<Monologue>().DisplayLog("인벤토리 공간이 부족하군.\n채집하기 전에 필요없는 아이템을 버리는게 좋겠어.");
                    return;
                }
            }
            else if (Input.GetKeyDown(KeyCode.C) && target.GetComponent<Plant>().isGatherPossible == false)
            {
                if(target.GetComponent<Plant>().plantName == "Trap01")
                {
                    Monologue.GetComponent<Monologue>().DisplayLog("아직 덫에 아무 것도 걸리지 않았군.\n괴물은 빛을 싫어하니까 내가 까가이 있으면 잡히지 않을거야.");
                }
                else
                {
                    Monologue.GetComponent<Monologue>().DisplayLog("조금 더 자란 다음에 채집하는게 좋겠군.");
                }
                return;
            }
        }
        else if (target.gameObject.tag == "Facility")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (target.GetComponent<Facility>().isOn == true)
                {
                    if (target.GetComponent<FacilityBalloon>().isMake == false && target.GetComponent<FacilityBalloon>().isMakeFinish == false)
                    {
                        target.GetComponent<Facility>().OpenProductionWindow();
                        PlayerDirection();
                    }
                    if (target.GetComponent<FacilityBalloon>().isMakeFinish == true)
                    {
                        PlayerDirection();
                        if (target.GetComponent<FacilityBalloon>().InventoryCheck() == true)
                        {
                            target.GetComponent<FacilityBalloon>().GetItem();
                        }
                        else
                        {
                            Monologue.GetComponent<Monologue>().DisplayLog("인벤토리 공간이 부족하군.\n아이템을 획득하려면 인벤토리에 빈 공간이 필요해.");
                            return;
                        }
                    }
                    else if (target.GetComponent<FacilityBalloon>().isMake == true)
                    {
                        Monologue.GetComponent<Monologue>().DisplayLog("아직 제작중이군.\n다른 일을 하면서 조금 기다려보자.");
                        return;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (target.GetComponent<FacilityBalloon>().isMake == false && target.GetComponent<FacilityBalloon>().isMakeFinish == false)
                {
                    PlayerDirection();
                    target.GetComponent<Facility>().OnOff();
                    target.GetComponent<Facility>().Research();
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (target.GetComponent<FacilityBalloon>().isMake == false && target.GetComponent<FacilityBalloon>().isMakeFinish == false)
                {
                    target.GetComponent<Facility>().Sleep();
                }
                else if (target.GetComponent<FacilityBalloon>().isMake == true)
                {
                    PlayerDirection();
                    target.GetComponent<FacilityBalloon>().Dunp();
                }
            }
        }
        else if (target.gameObject.tag == "Portal")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GetComponent<PlayerMove>().SetMovePossible(false);
                SceneObjectManager.instance.SaveObject();
                SceneChanger.instance.FadeAndLoadScene(target.GetComponent<Portal>().sceneName, target.GetComponent<Portal>().AfterMoveGrid);
            }
        }
        else if (target.gameObject.tag == "Bulb")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                target.GetComponent<Bulb>().OnOff();
            }
        }
    }

    void GatherAnimation(int num, bool onOff)
    {
        isGather = onOff;
        animaitor.SetInteger("GatherType", num);
        animaitor.SetBool("isGather", isGather);
        GetComponent<PlayerMove>().SetMovePossible(!onOff);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        target = other.gameObject;
        isTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        target = null;
        isTrigger = false;
    }

    //애니메이션 이벤트에서 불러오는 메소드
    void GatherEnd()
    {
        if (target == null)
            return;
        GatherAnimation(0, false);
        target.GetComponent<Plant>().GetItem();
    }

    public void SetInteractionPossible(bool possibility)
    {
        isInteractionPossible = possibility;
    }

    public bool GetInteractionPossible()
    {
        return isInteractionPossible;
    }
}