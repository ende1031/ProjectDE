using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    GameObject SelectCursor;

    GameObject Logo;

    int selectIndex = 0;

    bool isCursorActive = false;

    void Start ()
    {
        SelectCursor = GameObject.Find("Title_Select");
        Logo = GameObject.Find("LogoCanvas");
    }
	
	void Update ()
    {
        if (isCursorActive == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && selectIndex > 0)
            {
                selectIndex--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && selectIndex < 1)
            {
                selectIndex++;
            }

            Vector3 tempPos = SelectCursor.transform.position;

            switch (selectIndex)
            {
                case 0:
                    tempPos.x = 981.0f;
                    tempPos.y = 290.0f;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C))
                    {
                        GameStart();
                    }
                    break;
                case 1:
                    tempPos.x = 1114.0f;
                    tempPos.y = 190.0f;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C))
                    {
                        Application.Quit();
                    }
                    break;
            }

            SelectCursor.transform.position = tempPos;
        }
        else
        {
            if(Logo == null)
            {
                isCursorActive = true;
            }
        }
    }

    public void GameStart()
    {
        SceneObjectManager.instance.SetUIActive(true);
        SceneChanger.instance.FadeAndLoadScene("Stage01", 6);
    }
}
