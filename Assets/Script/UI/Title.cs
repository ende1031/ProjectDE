using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Title : MonoBehaviour
{
    GameObject Cursor;
    GameObject[] titleMenu;
    Animator animaitor;
    GameObject BlackScreen;
    AudioSource audio;

    GameObject mainCamera;

    int selectIndex = 0;

    bool isCursorActive = false;
    float Timer = 0;
    bool isSelect = false;

    void Start ()
    {
        Cursor = GameObject.Find("Cursor");
        titleMenu = new GameObject[5] { GameObject.Find("NewStart"), GameObject.Find("Load"), GameObject.Find("Quit"), GameObject.Find("Video"), GameObject.Find("People") };
        BlackScreen = GameObject.Find("Canvas").transform.Find("BlackScreen").gameObject;
        animaitor = GetComponent<Animator>();
        audio = GameObject.Find("TitleAudio").GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera");

        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x = 0;
        mainCamera.transform.position = cameraPos;

        RefreshMenu();
    }
	
	void Update ()
    {
        if (isSelect == false)
        {
            if (isCursorActive == true)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectIndex == 0)
                    {
                        selectIndex = 4;
                    }
                    else
                    {
                        selectIndex--;
                    }
                    RefreshMenu();
                    SoundManager.instance.PlaySE(3);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectIndex == 4)
                    {
                        selectIndex = 0;
                    }
                    else
                    {
                        selectIndex++;
                    }
                    RefreshMenu();
                    SoundManager.instance.PlaySE(3);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && selectIndex <= 2)
                {
                    selectIndex = 3;
                    RefreshMenu();
                    SoundManager.instance.PlaySE(3);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && selectIndex > 2)
                {
                    selectIndex = 0;
                    RefreshMenu();
                    SoundManager.instance.PlaySE(3);
                }

                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return))
                {
                    SelectMenu();
                    SoundManager.instance.PlaySE(4);
                }
            }
            else
            {
                Timer += Time.deltaTime;
                if (Timer > 0.5f)
                {
                    isCursorActive = true;
                }
            }
        }
        else
        {
            Color tempColor;
            for (int i = 0; i < 5; i++)
            {
                tempColor = titleMenu[i].GetComponent<TextMesh>().color;
                if (tempColor.a > 0.1f)
                {
                    tempColor.a -= Time.deltaTime * 3;
                }
                else
                {
                    tempColor.a = 0;
                }
                titleMenu[i].GetComponent<TextMesh>().color = tempColor;
            }

            tempColor = Cursor.GetComponent<SpriteRenderer>().color;
            if (tempColor.a > 0.1f)
            {
                tempColor.a -= Time.deltaTime * 5;
            }
            else
            {
                tempColor.a = 0;
            }
            Cursor.GetComponent<SpriteRenderer>().color = tempColor;

            if(audio.volume > 0)
            {
                audio.volume -= 0.2f * Time.deltaTime;
            }
        }
    }

    void SelectMenu()
    {
        switch(selectIndex)
        {
            case 0:
                if (File.Exists(Application.dataPath + "/Save/NewGameData.json") == false)
                {
                    return;
                }
                animaitor.SetBool("isStart", true);
                BlackScreen.SetActive(true);
                isSelect = true;
                break;
            case 1:
                if (File.Exists(Application.dataPath + "/Save/SaveData.json") == false)
                {
                    return;
                }
                animaitor.SetBool("isStart", true);
                BlackScreen.SetActive(true);
                isSelect = true;
                break;

            case 2:
                isSelect = true;
                Application.Quit();
                break;
        }
    }

    void RefreshMenu()
    {
        Vector3 tempPos = titleMenu[selectIndex].transform.position;
        tempPos.z = -1.2f;
        Cursor.transform.position = tempPos;
        Cursor.transform.rotation = titleMenu[selectIndex].transform.rotation;

        Color tempColor = titleMenu[0].GetComponent<TextMesh>().color;
        for(int i =0; i<5; i++)
        {
            if(i == selectIndex)
            {
                tempColor.a = 1;
                titleMenu[i].GetComponent<TextMesh>().color = tempColor;
            }
            else
            {
                tempColor.a = 0.5f;
                titleMenu[i].GetComponent<TextMesh>().color = tempColor;
            }
        }
    }

    public void GameStart()
    {
        //SceneObjectManager.instance.SetUIActive(true);
        switch (selectIndex)
        {
            case 0:
                SceneObjectManager.instance.isNewGame = true;
                SceneChanger.instance.FadeAndLoadScene("OpeningMovie");
                break;
            case 1:
                SceneObjectManager.instance.isNewGame = false;
                SceneObjectManager.instance.SetUIActive(true);
                SceneChanger.instance.FadeAndLoadScene("Stage01", 5);
                break;
        }
    }
}
