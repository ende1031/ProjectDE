using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NyxUI : MonoBehaviour
{
    public int amountOfNyx = 300;

    Text UI_Text;

    void Start ()
    {
        UI_Text = transform.Find("Nyx_Amount").gameObject.GetComponent<Text>();

    }
	
	void Update ()
    {
        DisplayText();

    }

    void DisplayText()
    {
        UI_Text.text = amountOfNyx.ToString();
    }

    public void SetAmount(int amount)
    {
        amountOfNyx += amount;

        if(amountOfNyx < 0)
        {
            amountOfNyx = 0;
        }
    }

    public int GetAmount()
    {
        return amountOfNyx;
    }
}
