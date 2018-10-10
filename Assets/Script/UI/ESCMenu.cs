using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    GameObject Menu;
    GameObject player;
    GameObject Cursor;
    GameObject[] MenuText = new GameObject[3];
    Animator menuAnimator;
    float timer = 0;
    int selectedIndex = 0;
    bool isMenuOpen = false;

    void Start ()
    {
        Menu = transform.Find("Menu").gameObject;
        Cursor = Menu.transform.Find("Board").transform.Find("Cursor").gameObject;

        for(int i = 0; i < 3; i++)
        {
            MenuText[i] = Menu.transform.Find("Board").transform.Find("Menu" + (i + 1)).gameObject;
        }

        menuAnimator = Menu.GetComponent<Animator>();

    }
	
	void Update ()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            return;
        }

        timer += Time.deltaTime;
        if (timer < 0.1f)
        {
            return;
        }

        if (isMenuOpen == false)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OpenWindow();
            }
        }
        else
        {
            MoveCursor();
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.X))
            {
                CloseWindow();
            }
            if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Return))
            {
                SelectMenu();
            }
        }
    }

    void OpenWindow()
    {
        if (player.GetComponent<PlayerMove>().GetMovePossible() == false)
        {
            return;
        }
        player.GetComponent<PlayerMove>().SetMovePossible(false);
        Menu.SetActive(true);
        isMenuOpen = true;
        timer = 0;
        SoundManager.instance.PlaySE(13);
        RefreshMenu();
    }

    void CloseWindow()
    {
        player.GetComponent<PlayerMove>().SetMovePossible(true);
        menuAnimator.SetBool("isOff", true);
        isMenuOpen = false;
        timer = 0;
        SoundManager.instance.PlaySE(15);
    }

    void RefreshMenu()
    {
        Cursor.transform.position = MenuText[selectedIndex].transform.position;

        Color tempColor = MenuText[0].GetComponent<Text>().color;

        for (int i = 0; i < 3; i++)
        {
            if (i == selectedIndex)
            {
                tempColor.a = 1;
                MenuText[i].GetComponent<Text>().color = tempColor;
            }
            else
            {
                tempColor.a = 0.5f;
                MenuText[i].GetComponent<Text>().color = tempColor;
            }
        }
    }

    void MoveCursor()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) && selectedIndex > 0)
        {
            selectedIndex--;
            RefreshMenu();
            SoundManager.instance.PlaySE(14);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && selectedIndex < 2)
        {
            selectedIndex++;
            RefreshMenu();
            SoundManager.instance.PlaySE(14);
        }
    }

    void SelectMenu()
    {
        switch(selectedIndex)
        {
            case 0:
                CloseWindow();
                break;

            case 1:
                SceneObjectManager.instance.ResetGame();
                break;

            case 2:
                Application.Quit();
                break;
        }
    }
}
