using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : SceneObject
{
    public string sceneName;
    public int AfterMoveGrid; //맵이동 후 플레이어의 좌표

    //InteractionIcon interactionIcon;

    float LoadTimer = 0;

    void Start ()
    {
        LoadMenuUIAndSeneNum();
        //interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
    }
	
	void Update ()
    {
		if(LoadTimer <= 0.6f)
        {
            LoadTimer += Time.deltaTime;
        }
	}

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        interactionIcon.AddIcon(InteractionIcon.Icon.Portal);
    //    }
    //}

    public override void DisplayIcon()
    {
        interactionIcon.AddIcon(InteractionIcon.Icon.Portal);
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        interactionIcon.DeleteIcon(InteractionIcon.Icon.Portal);
    //    }
    //}

    public bool isPortalReady()
    {
        if(LoadTimer >= 0.6f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
