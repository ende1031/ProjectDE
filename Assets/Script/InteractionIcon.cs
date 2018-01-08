using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIcon : MonoBehaviour
{
    //아이콘 이름
    public enum Icon //아이콘 추가시 수정할 부분
    {
        Gather,
        Input,
        Sleep,
        Temp
    };

    GameObject GatherIcon; //아이콘 추가시 수정할 부분
    GameObject InputIcon;
    GameObject SleepIcon;
    GameObject TempIcon;

    List<Icon> displayedIconList = new List<Icon>();
    Dictionary<Icon, GameObject> iconDictionary = new Dictionary<Icon, GameObject>();

    float iconSpace = 0.4f;

    void Start ()
    {
        GatherIcon = transform.Find("Gather").gameObject; //아이콘 추가시 수정할 부분
        InputIcon = transform.Find("Input").gameObject;
        SleepIcon = transform.Find("Sleep").gameObject;
        TempIcon = transform.Find("Suicide").gameObject;

        iconDictionary[Icon.Gather] = GatherIcon; //아이콘 추가시 수정할 부분
        iconDictionary[Icon.Input] = InputIcon;
        iconDictionary[Icon.Sleep] = SleepIcon;
        iconDictionary[Icon.Temp] = TempIcon;
    }

    void HideAllIcons()
    {
        GatherIcon.SetActive(false); //아이콘 추가시 수정할 부분
        InputIcon.SetActive(false);
        SleepIcon.SetActive(false);
        TempIcon.SetActive(false);
    }

    void Update ()
    {
        // 테스트용 코드
        
        /*
        if (Input.GetKeyUp(KeyCode.Q))
        {
            AddIcon(Icon.Gather);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            AddIcon(Icon.Input);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            DeleteIcon(Icon.Gather);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            DeleteIcon(Icon.Input);
        }
        */
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
                temp.x = temp.x + ( -iconSpace * (displayedIconList.Count - 1) + iconSpace * 2 * i);
                iconDictionary[displayedIconList[i]].transform.position = temp;
            }
        }
    }
}