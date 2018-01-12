using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string sceneName;
    public int AfterMoveGrid; //맵이동 후 플레이어의 좌표

    GameObject InteractionIcon;

    void Start ()
    {
        InteractionIcon = GameObject.Find("InteractionIcon");
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InteractionIcon.GetComponent<InteractionIcon>().AddIcon(global::InteractionIcon.Icon.Portal);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InteractionIcon.GetComponent<InteractionIcon>().DeleteIcon(global::InteractionIcon.Icon.Portal);
        }
    }
}
