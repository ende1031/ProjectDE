﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIcon : MonoBehaviour
{
    public enum Icon //아이콘 추가시 수정할 부분
    {
        Gather,
        Input,
        Sleep,
        Portal,
    };

    GameObject GatherIcon; //아이콘 추가시 수정할 부분
    GameObject InputIcon;
    GameObject SleepIcon;
    GameObject PortalIcon;

    List<Icon> displayedIconList = new List<Icon>();
    Dictionary<Icon, GameObject> iconDictionary = new Dictionary<Icon, GameObject>();
    
    GameObject IconBG;
    GameObject Inventory;

    float iconSpace = 0.3f;

    bool isInventoryOpen = false;

    void Start ()
    {
        LoadObjects();
    }

    void LoadObjects()
    {
        Inventory = GameObject.Find("Inventory");
        IconBG = transform.Find("IconBG").gameObject;

        GatherIcon = transform.Find("Gather").gameObject; //아이콘 추가시 수정할 부분
        InputIcon = transform.Find("Input").gameObject;
        SleepIcon = transform.Find("Sleep").gameObject;
        PortalIcon = transform.Find("Portal").gameObject;

        if (GatherIcon != null)
        {
            iconDictionary[Icon.Gather] = GatherIcon; //아이콘 추가시 수정할 부분
            iconDictionary[Icon.Input] = InputIcon;
            iconDictionary[Icon.Sleep] = SleepIcon;
            iconDictionary[Icon.Portal] = PortalIcon;
        }
    }

    void HideAllIcons()
    {
        GatherIcon.SetActive(false); //아이콘 추가시 수정할 부분
        InputIcon.SetActive(false);
        SleepIcon.SetActive(false);
        PortalIcon.SetActive(false);
    }

    void Update ()
    {
        if (Inventory.GetComponent<Inventory>().isInventoryActive == true && isInventoryOpen == false)
        {
            isInventoryOpen = true;
            IconBG.SetActive(false);
            HideAllIcons();
            return;
        }
        if(isInventoryOpen == true)
        {
            if(Inventory.GetComponent<Inventory>().isInventoryActive == false)
            {
                isInventoryOpen = false;
                IconBG.SetActive(true);
                RefreshIcons();
                return;
            }
        }

        IconBGAlpha();
    }

    void DeleteAllIcons()
    {
        displayedIconList.Clear();
        RefreshIcons();
    }

    public void AddIcon(Icon ico)
    {
        if(displayedIconList.Contains(ico) == false)
        {
            displayedIconList.Add(ico);
            RefreshIcons();
        }
    }

    public void DeleteIcon(Icon ico)
    {
        if (displayedIconList.Contains(ico) == true)
        {
            displayedIconList.Remove(ico);
            RefreshIcons();
        }
    }

    void RefreshIcons()
    {
        HideAllIcons();

        if (displayedIconList.Count > 0)
        {
            for(int i = 0; i < displayedIconList.Count; i++ )
            {
                iconDictionary[displayedIconList[i]].SetActive(true);
                Vector3 temp = transform.position;
                temp.x = temp.x + (-iconSpace * (displayedIconList.Count - 1) + iconSpace * 2 * i);
                iconDictionary[displayedIconList[i]].transform.position = temp;
            }
        }
    }

    void IconBGAlpha()
    {
        Color temp = IconBG.GetComponent<SpriteRenderer>().color;
        if (displayedIconList.Count > 0)
        {
            if(temp.a < 1)
            {
                temp.a += 3.0f * Time.deltaTime;
            }
            if (temp.a >= 1)
            {
                temp.a = 1;
            }
        }
        else
        {
            if (temp.a > 0)
            {
                temp.a -= 3.0f * Time.deltaTime;
            }
            if (temp.a <= 0)
            {
                temp.a = 0;
            }
        }
        IconBG.GetComponent<SpriteRenderer>().color = temp;
    }
}