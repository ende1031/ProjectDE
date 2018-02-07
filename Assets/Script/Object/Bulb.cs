using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    InteractionIcon interactionIcon;
    Inventory inventory;

    GameObject BulbLight;
    GameObject Balloon;
    GameObject TimeText;
    Animator animaitor;
    int sceneNum;

    public bool isOn = true;
    public bool isLoadByManager;
    public bool isAlive = true;

    public float LifeTime = 20;
    public float LifeTimer = 0;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        BulbLight = transform.Find("Light").gameObject;
        Balloon = transform.Find("Balloon").gameObject;
        TimeText = Balloon.transform.Find("TimeText").gameObject;
        Balloon.GetComponent<Animator>().SetBool("BalloonOff", false);
        Balloon.SetActive(false);

        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetBool("isOn", isOn);
            animaitor.SetBool("isAlive", isAlive);
        }
    }
	
	void Update ()
    {
        if (isLoadByManager == true)
        {
            animaitor.SetBool("isOn", isOn);
            BulbLight.SetActive(isOn);

            if (LifeTimer >= LifeTime)
            {
                isAlive = false;
                OnOff(false);
                animaitor.SetBool("isDie", true);
            }
            isLoadByManager = false;
        }

        if (isAlive == true)
        {
            DisplayText();

            if (isOn == true)
            {
                if (LifeTimer < LifeTime)
                {
                    LifeTimer += Time.deltaTime;
                }
            }
            if (LifeTimer > LifeTime)
            {
                LifeTimer = LifeTime;
                OnOff(false);
                isAlive = false;
            }
        }
        else
        {
            BulbLight.SetActive(false);
            animaitor.SetBool("isAlive", isAlive);
            TimeText.SetActive(false);
            Balloon.SetActive(false);
            if(isOn == true)
            {
                OnOff(false);
            }
            if (Grid.instance.PosToGrid(transform.position.x) == Grid.instance.PlayerGrid())
            {
                interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
            }
        }
    }

    void DisplayText()
    {
        float timeLeft = LifeTime - LifeTimer;
        float temp = (int)(timeLeft % 60);
        if (temp < 10)
        {
            TimeText.GetComponent<TextMesh>().text = (int)(timeLeft / 60.0f) + ":0" + temp;
        }
        else
        {
            TimeText.GetComponent<TextMesh>().text = (int)(timeLeft / 60.0f) + ":" + temp;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isAlive == true)
        {
            TimeText.SetActive(true);
            Balloon.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false && isAlive == true)
        {
            DisplayIcon();
        }
    }

    public void DisplayIcon()
    {
        interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
        interactionIcon.AddIcon(InteractionIcon.Icon.Remove);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isAlive == true)
        {
            TimeText.SetActive(false);
            Balloon.GetComponent<Animator>().SetBool("BalloonOff", true);
        }

        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false && isAlive == true)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Remove);
        }
    }

    public void OnOff()
    {
        if (isAlive == true)
        {
            isOn = !isOn;
            animaitor.SetBool("isOn", isOn);
            BulbLight.SetActive(isOn);
            interactionIcon.DeleteAllIcons();
            DisplayIcon();
            SceneObjectManager.instance.SaveObject();
        }
    }

    public void OnOff(bool onOff)
    {
        if (isAlive == true)
        {
            isOn = false;
            animaitor.SetBool("isOn", false);
            BulbLight.SetActive(false);
            interactionIcon.DeleteAllIcons();
            DisplayIcon();
            SceneObjectManager.instance.SaveObject();
        }
    }

    public void RemoveObject()
    {
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }
}
