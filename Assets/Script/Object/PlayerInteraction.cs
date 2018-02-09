﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Inventory inventory;
    InteractionMenu interactionMenu;
    Monologue monologue;
    Timer timer;

    Animator animaitor;
    bool isGather = false;

    bool isTrigger = false;
    GameObject target;

    bool isInteractionPossible = true;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        monologue = transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
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
            //interactionMenu.ClearMenu();
            //interactionMenu.AddMenu(InteractionMenu.MenuItem.Battery);
            //interactionMenu.AddMenu(InteractionMenu.MenuItem.Cancle);
            //interactionMenu.AddMenu(InteractionMenu.MenuItem.Dump);
            //interactionMenu.OpenMenu();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //inventory.GetItem(Inventory.Item.Mass, 20);
            //inventory.GetItem(Inventory.Item.Stick, 20);
            //inventory.GetItem(Inventory.Item.Board, 20);
            //inventory.GetItem(Inventory.Item.Thorn, 20);
            //inventory.GetItem(Inventory.Item.Hose, 20);
            //inventory.GetItem(Inventory.Item.Tumor, 20);
            //inventory.GetItem(Inventory.Item.Heart, 20);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //inventory.GetItem(Inventory.Item.Bulb01, 1);
            //inventory.GetItem(Inventory.Item.Grinder01, 1);
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

    public void GatherPlant(int GatherAnimationType)
    {
        PlayerDirection();
        GatherAnimation(GatherAnimationType, true);
        GetComponent<PlayerMove>().SetMovePossible(false);
    }

    void Interaction()
    {
        if (target == null)
            return;

        if (target.gameObject.tag == "Plant")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                target.GetComponent<Plant>().OpenMenu();
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
                    }
                    if (target.GetComponent<FacilityBalloon>().isMakeFinish == true)
                    {
                        if (target.GetComponent<FacilityBalloon>().InventoryCheck() == true)
                        {
                            target.GetComponent<FacilityBalloon>().GetItem();
                        }
                        else
                        {
                            monologue.DisplayLog("인벤토리 공간이 부족하군.\n아이템을 획득하려면 인벤토리에 빈 공간이 필요해.");
                            return;
                        }
                    }
                    else if (target.GetComponent<FacilityBalloon>().isMake == true)
                    {
                        monologue.DisplayLog("아직 제작중이군.\n다른 일을 하면서 조금 기다려보자.");
                        return;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (target.GetComponent<FacilityBalloon>().isMake == false && target.GetComponent<FacilityBalloon>().isMakeFinish == false)
                {
                    target.GetComponent<Facility>().OnOff();
                    target.GetComponent<Facility>().Research();
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (target.GetComponent<FacilityBalloon>().isMake == false && target.GetComponent<FacilityBalloon>().isMakeFinish == false)
                {
                    if (target.GetComponent<Facility>().facilityName == "EscapePod" && GetComponent<PlayerMove>().GetMovePossible() == true)
                    {
                        if (timer.PercentOfTime() > 1)
                        {
                            GetComponent<PlayerMove>().SetMovePossible(false);
                            target.GetComponent<Facility>().Sleep();
                        }
                        else
                        {
                            monologue.DisplayLog("지금은 졸리지 않아.\n일어난지 얼마 안됐는데 벌써 잘 수는 없지.");
                        }
                    }
                    else if(target.GetComponent<Facility>().facilityName != "EscapePod")
                    {
                        target.GetComponent<Facility>().RemoveObject();
                    }
                }
                else if (target.GetComponent<FacilityBalloon>().isMake == true)
                {
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
            else if (Input.GetKeyDown(KeyCode.X))
            {
                target.GetComponent<Bulb>().RemoveObject();
            }
        }
        else if (target.gameObject.tag == "Wreckage")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                target.GetComponent<Wreckage>().OpenMenu();
            }
        }
    }

    void GatherAnimation(int num, bool onOff)
    {
        isGather = onOff;
        animaitor.SetInteger("GatherType", num);
        animaitor.SetBool("isGather", isGather);
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
        GetComponent<PlayerMove>().SetMovePossible(true);
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