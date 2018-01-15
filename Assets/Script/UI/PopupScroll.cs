using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupScroll : MonoBehaviour
{
    public GameObject content; //스크롤할 내용
    public GameObject cursor; //커서

    float cursorDistance; //커서와 스크롤할 내용의 간격
    int index = 0; //현재 선택된 번호

    public int maxIndex = 4; //스크롤할 내용의 갯수
    public float spaceOfIndex = 160.0f; //스크롤할 내용들 사이의 간격

    public float bottomPos = 265; //커서가 가장 아래로 내려갔을 때의 포지션값
    float topPos; //커서의 초기 위치

    void Start()
    {
        cursorDistance = content.transform.position.y - cursor.transform.position.y;
        topPos = cursor.transform.position.y;
    }

    void Update ()
    {
        InputAndMove();
    }

    void InputAndMove()
    {
        Vector3 cursorPos = cursor.transform.position;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
                cursorPos.y += spaceOfIndex;
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (index < maxIndex)
            {
                index++;
                cursorPos.y -= spaceOfIndex;
            }
        }

        if (cursorPos.y < bottomPos)
        {
            cursorPos.y = bottomPos;
        }
        else if(cursorPos.y > topPos)
        {
            cursorPos.y = topPos;
        }

        cursor.transform.position = cursorPos;

        Vector3 contentpos = content.transform.position;
        contentpos.y = cursorDistance + cursor.transform.position.y + 160 * index;
        content.transform.position = contentpos;
    }

    int GetIndex()
    {
        return index;
    }
}