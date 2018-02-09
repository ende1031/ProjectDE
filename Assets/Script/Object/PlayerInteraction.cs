using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Inventory inventory;
    Monologue monologue;

    Animator animaitor;
    bool isGather = false;

    bool isTrigger = false;
    GameObject target;

    bool isInteractionPossible = true;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        monologue = transform.Find("Monologue").gameObject.GetComponent<Monologue>();
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
                if (target.GetComponent<Facility>().isOn == false)
                {
                    target.GetComponent<Facility>().OnOff();
                }
                else
                {
                    target.GetComponent<Facility>().OpenMenu();
                }
            }
        }
        else if (target.gameObject.tag == "Portal")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GetComponent<PlayerMove>().SetMovePossible(false);
                SceneObjectManager.instance.SaveObject();
                SceneChanger.instance.FadeAndLoadScene(target.GetComponent<Portal>().sceneName, target.GetComponent<Portal>().AfterMoveGrid);
            }
        }
        else if (target.gameObject.tag == "Bulb")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if(target.GetComponent<Bulb>().isOn == false)
                {
                    target.GetComponent<Bulb>().OnOff();
                }
                else
                {
                    target.GetComponent<Bulb>().OpenMenu();
                }
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

    public void GatherPlant(int GatherAnimationType)
    {
        PlayerDirection();
        GatherAnimation(GatherAnimationType, true);
        GetComponent<PlayerMove>().SetMovePossible(false);
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