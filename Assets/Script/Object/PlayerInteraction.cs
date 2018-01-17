using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    GameObject Inventory;

    Animator animaitor;
    bool isGather = false;

    bool isTrigger = false;
    GameObject target;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        Inventory = GameObject.Find("Inventory");
    }
	
	void Update ()
    {
        if(isTrigger)
        {
            if (Inventory.GetComponent<Inventory>().isInventoryActive == false)
            {
                Interaction();
            }
        }
    }

    void Interaction()
    {
        if (target == null)
            return;

        if (target.gameObject.tag == "Plant")
        {
            if (Input.GetKeyUp(KeyCode.Z) && target.GetComponent<Plant>().isGatherPossible == true)
            {
                GatherAnimation(target.GetComponent<Plant>().GatherAnimationType, true);
                target.GetComponent<Plant>().GatherStart();
            }
        }

        if (target.gameObject.tag == "Facility")
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                target.GetComponent<Facility>().DeleteItem();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                target.GetComponent<Facility>().Sleep();
            }
        }

        if (target.gameObject.tag == "Portal")
        {
            if (Input.GetKeyUp(KeyCode.X))
            {
                SceneObjectManager.instance.SaveObject();
                SceneChanger.instance.FadeAndLoadScene(target.GetComponent<Portal>().sceneName, target.GetComponent<Portal>().AfterMoveGrid);
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
}