using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Inventory inventory;
    Monologue monologue;
    EnergyGauge energyGauge;

    //GameObject BulbLight;
    //GameObject Balloon;
    //GameObject TimeText;
    //Animator animaitor;
    int sceneNum;

    //public bool isOn = true;
    //public bool isLoadByManager;
    //public bool isAlive = true;

    //public float LifeTime = 20;
    //public float LifeTimer = 0;

    //float offTimer = 0;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        monologue = GameObject.Find("Player").transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        energyGauge = GameObject.Find("LeftUI").GetComponent<EnergyGauge>();

        //BulbLight = transform.Find("Light").gameObject;
        //Balloon = transform.Find("Balloon").gameObject;
        //TimeText = Balloon.transform.Find("TimeText").gameObject;
        //Balloon.GetComponent<Animator>().SetBool("BalloonOff", false);
        //Balloon.SetActive(false);

        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;

        //animaitor = GetComponent<Animator>();
        //if (animaitor != null)
        //{
        //    animaitor.SetBool("isOn", isOn);
        //    animaitor.SetBool("isAlive", isAlive);
        //}
    }
	
	void Update ()
    {
        //if (isLoadByManager == true)
        //{
        //    animaitor.SetBool("isOn", isOn);
        //    BulbLight.SetActive(isOn);

        //    if (LifeTimer >= LifeTime)
        //    {
        //        isAlive = false;
        //        OnOff(false);
        //        animaitor.SetBool("isDie", true);
        //    }
        //    isLoadByManager = false;
        //}

        //if (isAlive == true)
        //{
        //    DisplayText();

        //    if (isOn == true)
        //    {
        //        if (LifeTimer < LifeTime)
        //        {
        //            LifeTimer += Time.deltaTime;
        //        }
        //    }
        //    if (LifeTimer > LifeTime)
        //    {
        //        LifeTimer = LifeTime;
        //        OnOff(false);
        //        isAlive = false;
        //    }
        //}
        //else
        //{
        //    BulbLight.SetActive(false);
        //    animaitor.SetBool("isAlive", isAlive);
        //    TimeText.SetActive(false);
        //    Balloon.SetActive(false);
        //    if(isOn == true)
        //    {
        //        OnOff(false);
        //    }
        //}

        //if (offTimer < 0.3f)
        //{
        //    offTimer += Time.deltaTime;
        //}
    }

    void DisplayText()
    {
        //float timeLeft = LifeTime - LifeTimer;
        //float temp = (int)(timeLeft % 60);
        //if (temp < 10)
        //{
        //    TimeText.GetComponent<TextMesh>().text = (int)(timeLeft / 60.0f) + ":0" + temp;
        //}
        //else
        //{
        //    TimeText.GetComponent<TextMesh>().text = (int)(timeLeft / 60.0f) + ":" + temp;
        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Player" && isAlive == true)
        //{
        //    TimeText.SetActive(true);
        //    Balloon.SetActive(true);
        //}
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            //if(isAlive == true && isOn == false)
            //{
            //    interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
            //}
            //else
            //{
                interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
            //}
        }
    }

    public void DisplayIcon()
    {
        //if (isAlive == true && isOn == false)
        //{
        //    interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
        //}
        //else
        //{
            interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Player" && isAlive == true)
        //{
        //    TimeText.SetActive(false);
        //    Balloon.GetComponent<Animator>().SetBool("BalloonOff", true);
        //}

        //if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        //{
        //    interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
        //    interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
        //}
        if (other.gameObject.tag == "Player")
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
        }
    }

    public void OnOff()
    {
        //if (offTimer < 0.3f)
        //{
        //    return;
        //}

        //if (isAlive == true)
        //{
        //    isOn = !isOn;
        //    animaitor.SetBool("isOn", isOn);
        //    BulbLight.SetActive(isOn);
        //    interactionIcon.DeleteAllIcons();
        //    DisplayIcon();
        //    offTimer = 0;
        //    SceneObjectManager.instance.SaveObject();
        //}
    }

    public void OnOff(bool onOff)
    {
        //if (offTimer < 0.3f)
        //{
        //    return;
        //}

        //if (isAlive == true)
        //{
        //    isOn = false;
        //    animaitor.SetBool("isOn", false);
        //    BulbLight.SetActive(false);
        //    interactionIcon.DeleteAllIcons();
        //    DisplayIcon();
        //    offTimer = 0;
        //    SceneObjectManager.instance.SaveObject();
        //}
    }

    void Examine()
    {
        monologue.DisplayLog("괴물은 빛을 싫어한다.\n이 전구 근처에는 다른 시설을 설치해도 안전할 것이다.");
    }

    public void RemoveObject()
    {
        if (energyGauge.GetAmount() < 10)
        {
            monologue.DisplayLog("에너지가 부족해서 철거할 수 없어.");
            return;
        }
        energyGauge.SetAmount(-10);
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        //if (isAlive == true)
        //{
        //    interactionMenu.AddMenu(InteractionMenu.MenuItem.Off);
        //}
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, "Bulb", GetComponent<SpriteRenderer>().sprite, w, h);
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
    {
        switch (m)
        {
            //case InteractionMenu.MenuItem.Off:
            //    OnOff(false);
            //    break;

            case InteractionMenu.MenuItem.Remove:
                RemoveObject();
                break;
            case InteractionMenu.MenuItem.Examine:
                Examine();
                break;
        }
    }
}
