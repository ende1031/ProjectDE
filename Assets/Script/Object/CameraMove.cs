using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 100;
    GameObject player;

    GameObject LeftWall;
    GameObject RightWall;

    public float cameraMargin = 5.96f;

    void Start ()
    {
        player = GameObject.Find("Player");
        LeftWall = GameObject.Find("LeftWall");
        RightWall = GameObject.Find("RightWall");

    }
	
	void Update ()
    {
        if(player == null || LeftWall == null || RightWall == null)
        {
            player = GameObject.Find("Player");
            LeftWall = GameObject.Find("LeftWall");
            RightWall = GameObject.Find("RightWall");
        }
        else
        {
            Vector3 tempPos = transform.position;

            Vector3 targetPos = player.transform.position;

            if (targetPos.x - LeftWall.transform.position.x < cameraMargin)
                targetPos.x = LeftWall.transform.position.x + cameraMargin;
            if (RightWall.transform.position.x - targetPos.x < cameraMargin)
                targetPos.x = RightWall.transform.position.x - cameraMargin;


            tempPos.x += (tempPos.x - targetPos.x) / -15 * speed * Time.deltaTime;
            transform.position = tempPos;
        }
    }
}
