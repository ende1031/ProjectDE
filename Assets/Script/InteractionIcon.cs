using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIcon : MonoBehaviour
{
    //아이콘 이름
    public enum Icon
    {
        Gather,
        Temp
    };

    GameObject GatherIcon;
    GameObject TempIcon;

    List<Icon> displayedIconList = new List<Icon>();
    Dictionary<Icon, GameObject> iconDictionary = new Dictionary<Icon, GameObject>();

    float iconSpace = 0.4f;

    void Start ()
    {
        GatherIcon = transform.Find("Gather").gameObject;
        TempIcon = transform.Find("Suicide").gameObject;

        iconDictionary[Icon.Gather] = GatherIcon;
        iconDictionary[Icon.Temp] = TempIcon;
    }
	
	void Update ()
    {
        // 테스트용 코드
        if (Input.GetKeyUp(KeyCode.Q))
        {
            AddIcon(Icon.Gather);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            AddIcon(Icon.Temp);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            DeleteIcon(Icon.Gather);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            DeleteIcon(Icon.Temp);
        }
    }

    void HideAllIcons()
    {
        GatherIcon.SetActive(false);
        TempIcon.SetActive(false);
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
                Vector3 temp = GetComponent<RectTransform>().position;
                temp.x = temp.x + ( -iconSpace * (displayedIconList.Count - 1) + iconSpace * 2 * i);
                iconDictionary[displayedIconList[i]].GetComponent<RectTransform>().position = temp;
            }
        }
    }
}