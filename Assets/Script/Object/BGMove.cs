using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    SceneSetting sceneSetting;
    GameObject mainCamera;
    public float MoveSpeed = 0.8f;

    float startPos;
    float startCameraPos;
    bool isSetPos = false;
    
	void Start ()
    {
        sceneSetting = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>();
        mainCamera = GameObject.Find("Main Camera");
    }
	
	void Update ()
    {
        if (sceneSetting.GetIsSet() == true)
        {
            if(isSetPos == false)
            {
                startCameraPos = mainCamera.transform.position.x;
                startPos = transform.position.x - (startCameraPos * MoveSpeed);
                isSetPos = true;
            }
        }

        if(isSetPos == true)
        {
            Vector3 TempPos = transform.position;
            TempPos.x = startPos - ((mainCamera.transform.position.x - startCameraPos) * MoveSpeed);
            transform.position = TempPos;
        }
    }
}
