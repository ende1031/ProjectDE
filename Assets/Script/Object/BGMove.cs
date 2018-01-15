using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    public GameObject mainCamera;
    public float speed = 0.5f; //0 : 일반 배경(움직이지 않음) / 0~1 : 원경(카메라보다 느림) / 1 : 카메라에 붙어다님 / 1~ : 근경(카메라보다 빠름)
    float distanceToCamera;
    
	void Start ()
    {
        distanceToCamera = transform.position.x - mainCamera.transform.position.x;
    }
	
	void Update ()
    {
        Vector3 TempPos = transform.position;

        TempPos.x = mainCamera.transform.position.x * speed + distanceToCamera;

        transform.position = TempPos;
    }
}
