using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string sceneName;
    public int AfterMoveGrid; //맵이동 후 플레이어의 좌표

    InteractionIcon interactionIcon;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
    }
	
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Portal);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Portal);
        }
    }
}
