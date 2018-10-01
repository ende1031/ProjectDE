using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIcon : MonoBehaviour
{
    public enum Icon //icon
    {
        Interaction,
        OnOff,
        Portal
        //Gather,
        //Make,
        //Sleep,
        //Dump,
        //Research,
        //Remove,
        //Tumor
    };

    Inventory inventory;

    GameObject MenuIcon; //icon
    GameObject OnOffIcon;
    GameObject PortalIcon;
    //GameObject GatherIcon;
    //GameObject MakeIcon;
    //GameObject SleepIcon;
    //GameObject DumpIcon;
    //GameObject ResearchIcon;
    //GameObject RemoveIcon;
    //GameObject TumorIcon;

    List<Icon> displayedIconList = new List<Icon>();
    Dictionary<Icon, GameObject> iconDictionary = new Dictionary<Icon, GameObject>();
    
    GameObject IconBG;

    float iconSpace = 0.3f;

    bool isInventoryOpen = false;

    void Start ()
    {
        LoadObjects();
    }

    void LoadObjects()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        IconBG = transform.Find("IconBG").gameObject;

        MenuIcon = transform.Find("Interaction").gameObject; //icon
        PortalIcon = transform.Find("Portal").gameObject;
        OnOffIcon = transform.Find("OnOff").gameObject;
        //GatherIcon = transform.Find("Gather").gameObject;
        //MakeIcon = transform.Find("Make").gameObject;
        //SleepIcon = transform.Find("Sleep").gameObject;
        //DumpIcon = transform.Find("Dump").gameObject;
        //ResearchIcon = transform.Find("Research").gameObject;
        //RemoveIcon = transform.Find("Remove").gameObject;
        //TumorIcon = transform.Find("Tumor").gameObject;

        if (MenuIcon != null)
        {
            iconDictionary[Icon.Interaction] = MenuIcon; //icon
            iconDictionary[Icon.OnOff] = OnOffIcon;
            iconDictionary[Icon.Portal] = PortalIcon;
            //iconDictionary[Icon.Gather] = GatherIcon;
            //iconDictionary[Icon.Make] = MakeIcon;
            //iconDictionary[Icon.Sleep] = SleepIcon;
            //iconDictionary[Icon.Dump] = DumpIcon;
            //iconDictionary[Icon.Research] = ResearchIcon;
            //iconDictionary[Icon.Remove] = RemoveIcon;
            //iconDictionary[Icon.Tumor] = TumorIcon;
        }
    }

    void HideAllIcons()
    {
        MenuIcon.SetActive(false); //icon
        PortalIcon.SetActive(false);
        OnOffIcon.SetActive(false);
        //GatherIcon.SetActive(false);
        //MakeIcon.SetActive(false);
        //SleepIcon.SetActive(false);
        //DumpIcon.SetActive(false);
        //ResearchIcon.SetActive(false);
        //RemoveIcon.SetActive(false);
        //TumorIcon.SetActive(false);
    }

    void Update ()
    {
        if (inventory.isInventoryActive == true && isInventoryOpen == false)
        {
            isInventoryOpen = true;
            IconBG.SetActive(false);
            HideAllIcons();
            return;
        }
        if(isInventoryOpen == true)
        {
            if(inventory.isInventoryActive == false)
            {
                isInventoryOpen = false;
                IconBG.SetActive(true);
                RefreshIcons();
                return;
            }
        }

        IconBGAlpha();
    }

    public void DeleteAllIcons()
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
        if(isInventoryOpen == true)
        {
            HideAllIcons();
        }
    }

    public void DeleteIcon(Icon ico)
    {
        if (displayedIconList.Contains(ico) == true)
        {
            displayedIconList.Remove(ico);
            RefreshIcons();
        }
        if (isInventoryOpen == true)
        {
            HideAllIcons();
        }
    }

    void RefreshIcons()
    {
        HideAllIcons();


        if (displayedIconList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < displayedIconList.Count; i++)
        {
            iconDictionary[displayedIconList[i]].SetActive(true);
            Vector3 temp = transform.position;
            temp.x = temp.x + (-iconSpace * (displayedIconList.Count - 1) + iconSpace * 2 * i);
            iconDictionary[displayedIconList[i]].transform.position = temp;
        }

        //if (displayedIconList.Count > 0)
        //{
        //    for(int i = 0; i < displayedIconList.Count; i++ )
        //    {
        //        iconDictionary[displayedIconList[i]].SetActive(true);
        //        Vector3 temp = transform.position;
        //        temp.x = temp.x + (-iconSpace * (displayedIconList.Count - 1) + iconSpace * 2 * i);
        //        iconDictionary[displayedIconList[i]].transform.position = temp;
        //    }
        //}
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