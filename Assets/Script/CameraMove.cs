using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 100;
    GameObject player;

	void Start ()
    {
        player = GameObject.Find("Player");
        //Scene.GetRootGameObjects().
        //SceneManager.GetActiveScene().GetRootGameObjects().

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

        //테스트용 코드
        if (Input.GetKeyUp(KeyCode.Z))
        {
            FadeManager.instance.FadeAndLoadScene("Stage01");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            FadeManager.instance.FadeAndLoadScene("Stage02");
        }
    }
}
