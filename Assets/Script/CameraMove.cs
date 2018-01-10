using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 100;
    GameObject player;

	void Start ()
    {
        player = GameObject.Find("Player");

    }
	
	void Update ()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player != null)
        {
            Vector3 TempPos = transform.position;
            TempPos.x += (TempPos.x - player.transform.position.x) / -15 * speed * Time.deltaTime;
            transform.position = TempPos;
        }
    }
}
