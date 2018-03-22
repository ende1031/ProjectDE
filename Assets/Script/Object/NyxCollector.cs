﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyxCollector : MonoBehaviour
{
    public string ObjectName;
    [TextArea]
    public string ObjectExplanation;

    InteractionIcon interactionIcon;
    InteractionMenu interactionMenu;
    Inventory inventory;
    int sceneNum;
    Monologue monologue;
    EnergyGauge energyGauge;
    NyxUI nyxUI;
    Animator animaitor;
    GameObject particle;

    public float collectTimer = 0;

    public bool isLoadByManager = false;
    public int state = 1; // 0:꺼짐, 1:평상시, 4:죽음
    float offTimer = 0;

    void Start ()
    {
        interactionIcon = GameObject.Find("InteractionIcon").GetComponent<InteractionIcon>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        interactionMenu = GameObject.Find("InteractionMenu").GetComponent<InteractionMenu>();
        sceneNum = GameObject.Find("SceneSettingObject").GetComponent<SceneSetting>().sceneNum;
        monologue = GameObject.Find("Player").transform.Find("Monologue").gameObject.GetComponent<Monologue>();
        energyGauge = GameObject.Find("EnergyUI").GetComponent<EnergyGauge>();
        nyxUI = GameObject.Find("NyxUI").GetComponent<NyxUI>();
        particle = transform.Find("Particle").gameObject;

        animaitor = GetComponent<Animator>();
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
    }
	
	void Update ()
    {
        if (isLoadByManager == true)
        {
            if (state != 0)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
                {
                    if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                    {
                        Ruin();
                    }
                }
            }
            if(state == 0 || state == 4)
            {
                particle.SetActive(false);
            }
            if (animaitor != null)
            {
                animaitor.SetInteger("State", state);
            }
            isLoadByManager = false;
        }

        if (RuinCheck() == true)
        {
            Ruin();
        }

        if (offTimer < 0.3f)
        {
            offTimer += Time.deltaTime;
        }

        if(state == 1)
        {
            CollectNyx();
        }
    }

    void CollectNyx()
    {
        collectTimer += Time.deltaTime;

        if(collectTimer >= 1.0f)
        {
            nyxUI.SetAmount(5);
            collectTimer = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            DisplayIcon();
        }
    }

    public void DisplayIcon()
    {
        if (state != 0)
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.Interaction);
        }
        else
        {
            interactionIcon.AddIcon(InteractionIcon.Icon.OnOff);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && inventory.isInventoryActive == false)
        {
            interactionIcon.DeleteIcon(InteractionIcon.Icon.Interaction);
            interactionIcon.DeleteIcon(InteractionIcon.Icon.OnOff);
        }
    }

    bool RuinCheck()
    {
        if (state == 0)
        {
            return false;
        }

        if (Mathf.Abs(Grid.instance.PlayerGrid() - Grid.instance.PosToGrid(transform.position.x)) > 4)
        {
            if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Bulb", "Bulb01") == false)
            {
                if (SceneObjectManager.instance.RangeSearch(sceneNum, Grid.instance.PosToGrid(transform.position.x), 2, "Facility", "EscapePod") == false)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void Ruin()
    {
        state = 4;
        if (animaitor != null)
        {
            animaitor.SetInteger("State", state);
        }
        particle.SetActive(false);
    }

    void Repair()
    {
        if (state == 4)
        {
            if (energyGauge.GetAmount() < 5)
            {
                monologue.DisplayLog("에너지가 부족해서 수리할 수 없어.");
                return;
            }
            energyGauge.SetAmount(-5);
            state = 1;
            if (animaitor != null)
            {
                animaitor.SetInteger("State", state);
            }
            particle.SetActive(true);
        }
    }

    public void OnOff()
    {
        if (offTimer < 0.3f)
        {
            return;
        }

        if (state == 0)
        {
            state = 1;
            particle.SetActive(true);
        }
        else
        {
            state = 0;
            particle.SetActive(false);
        }
        animaitor.SetInteger("State", state);
        interactionIcon.DeleteAllIcons();
        DisplayIcon();
        offTimer = 0;
    }

    public void RemoveObject()
    {
        if (energyGauge.GetAmount() < 5)
        {
            monologue.DisplayLog("에너지가 부족해서 제거할 수 없어.");
            return;
        }
        energyGauge.SetAmount(-5);
        interactionIcon.DeleteAllIcons();
        SceneObjectManager.instance.DeleteObject(sceneNum, Grid.instance.PosToGrid(transform.position.x));
    }

    void Examine()
    {
        monologue.DisplayLog("닉스입자를 수집하는 시설이다.\n검은 입자들이 빨려들어가는게 보인다.");
    }

    public void OpenMenu()
    {
        interactionMenu.ClearMenu();
        interactionMenu.SetNameAndExp(ObjectName, ObjectExplanation);

        switch (state)
        {
            case 1:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Off);
                break;
            case 4:
                interactionMenu.AddMenu(InteractionMenu.MenuItem.Repair);
                break;
        }
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Examine);
        interactionMenu.AddMenu(InteractionMenu.MenuItem.Remove);

        float w = GetComponent<SpriteRenderer>().sprite.rect.width;
        float h = GetComponent<SpriteRenderer>().sprite.rect.height;
        interactionMenu.OpenMenu(this.gameObject, "NyxCollector", GetComponent<SpriteRenderer>().sprite, w, h);
    }

    public void SelectMenu(InteractionMenu.MenuItem m)
    {
        switch (m)
        {
            case InteractionMenu.MenuItem.Off:
                OnOff();
                break;
            case InteractionMenu.MenuItem.Remove:
                RemoveObject();
                break;
            case InteractionMenu.MenuItem.Repair:
                Repair();
                break;
            case InteractionMenu.MenuItem.Examine:
                Examine();
                break;
        }
    }
}