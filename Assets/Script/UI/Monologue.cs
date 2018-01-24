using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monologue : MonoBehaviour
{
    GameObject MonologueText;
    Animator animaitor;

    bool isLogueOn = false;
    string LogueText;
    string DisplayText;
    int stringCount = 0;

    float displayTimer = 0;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        MonologueText = transform.Find("Text").gameObject;
        MonologueText.GetComponent<TextMesh>().text = string.Empty;

        DisplayText = string.Empty;
    }
	
	void Update ()
    {
        if(isLogueOn == true)
        {
            InsertString();
            MonologueText.GetComponent<TextMesh>().text = DisplayText;
        }
    }

    public void DisplayLog(string logText, bool quotes = true)
    {
        if (isLogueOn == false)
        {
            animaitor.SetBool("isLogueOn", true);
            isLogueOn = true;
        }
        DisplayText = string.Empty;

        if (quotes == true)
        {
            LogueText = "\"" + logText + "\"";
        }
        else
        {
            LogueText = logText;
        }

        stringCount = 0;
        displayTimer = 0;
    }

    void InsertString()
    {
        displayTimer += Time.deltaTime;
        if (stringCount < LogueText.Length && displayTimer >= 0.05f)
        {
            DisplayText = DisplayText.Insert(DisplayText.Length, LogueText[stringCount] + "");
            stringCount++;
            displayTimer = 0;
        }
        else if(stringCount >= LogueText.Length && displayTimer >= 1.5f)
        {
            DisplayText = string.Empty;
            isLogueOn = false;
            animaitor.SetBool("isLogueOn", false);
        }
    }
}
