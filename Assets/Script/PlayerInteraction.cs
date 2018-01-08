using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Animator animaitor;
    bool isGather01;

    bool isTrigger = false;
    GameObject target;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
    }
	
	void Update ()
    {
        if(isTrigger)
        {
            Interaction();
        }
    }

    void Interaction()
    {
        if (target == null)
            return;

        if (target.gameObject.tag == "Plant")
        {
            if (Input.GetKeyUp(KeyCode.S) && target.GetComponent<Plant>().isGatherPossible == true)
            {
                GatherAnimation(target.GetComponent<Plant>().GatherAnimationType);
            }
        }

        if (target.gameObject.tag == "Facility")
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                target.GetComponent<Facility>().DeleteItem();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                target.GetComponent<Facility>().Sleep();
            }
        }
    }

    void GatherAnimation(int num)
    {
        switch (num)
        {
            case 1:
                isGather01 = true;
                animaitor.SetBool("isGather01", isGather01);
                break;

            default:
                isGather01 = true;
                animaitor.SetBool("isGather01", isGather01);
                break;
        }
        GetComponent<PlayerMove>().SetMovePossible(false);
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

        isGather01 = false;
        animaitor.SetBool("isGather01", isGather01);
        GetComponent<PlayerMove>().SetMovePossible(true);
        target.GetComponent<Plant>().GetItem();
    }
}