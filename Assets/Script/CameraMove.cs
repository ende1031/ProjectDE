using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    GameObject player;

	void Start ()
    {
        player = GameObject.Find("Player");
	}
	
	void Update ()
    {
        Vector3 TempPos = transform.position;
        //타겟을 향해 움직임
        TempPos.x += (TempPos.x - player.transform.position.x) / -15 * speed * Time.deltaTime;
        //위치 적용
        transform.position = TempPos;
    }
}
