using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGather : MonoBehaviour
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
            Gather();
        }
    }

    void GatherEnd()
    {
        isGather01 = false;
        animaitor.SetBool("isGather01", isGather01);
        GetComponent<PlayerMove>().SetMovePossible(true);
        target.GetComponent<Plant>().GetItem();
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

    void Gather()
    {
        if (target == null)
            return;

        if (target.gameObject.tag == "Plant")
        {
            if (Input.GetKeyUp(KeyCode.S) && target.GetComponent<Plant>().isGatherPossible == true)
            {
                isGather01 = true;
                animaitor.SetBool("isGather01", isGather01);
                GetComponent<PlayerMove>().SetMovePossible(false);
            }
        }
    }
}