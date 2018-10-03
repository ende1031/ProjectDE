using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject FT;

    Inventory inventory;
    Animator animaitor;
    bool isGather = false;

    bool isTrigger = false;
    GameObject target;

    bool isInteractionPossible = true;

    void Start ()
    {
        animaitor = GetComponent<Animator>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
	
	void Update ()
    {
        if(isTrigger)
        {
            if (inventory.isInventoryActive == false && isInteractionPossible == true)
            {
                Interaction();
            }
        }

        //테스트용 치트키
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.GetItem("Item_Mass", 5);
            inventory.GetItem("Item_Stick", 5);
            inventory.GetItem("Item_Board", 5);
            inventory.GetItem("Item_Thorn", 5);
            inventory.GetItem("Item_Hose", 5);
            inventory.GetItem("Item_TumorSeed", 5);
            inventory.GetItem("Item_Heart", 5);
            inventory.GetItem("Item_Sawtooth", 5);
            inventory.GetItem("Item_Fruit", 5);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            inventory.GetItem("Item_Facility01", 1);
            inventory.GetItem("Item_Bulb", 1);
            inventory.GetItem("Item_NyxCollector01", 1);
            inventory.GetItem("Item_Grinder01", 1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<PlayerMove>().GetMovePossible() == true)
            {
                SceneObjectManager.instance.ResetGame();
            }
        }
    }

    void PlayerDirection()
    {
        if (target.transform.position.x > transform.position.x)
        {
            GetComponent<PlayerMove>().SetDirection(1);
        }
        else if (target.transform.position.x < transform.position.x)
        {
            GetComponent<PlayerMove>().SetDirection(0);
        }
    }

    void Interaction()
    {
        if (target == null)
            return;

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (target.gameObject.tag == "Portal")
            {
                if (target.GetComponent<Portal>().isPortalReady() == true)
                {
                    GetComponent<PlayerMove>().SetMovePossible(false);
                    SceneObjectManager.instance.SaveObject();
                    SceneChanger.instance.FadeAndLoadScene(target.GetComponent<Portal>().TargetSceneName, target.GetComponent<Portal>().state);
                    SoundManager.instance.PlaySE(37);
                }
                return;
            }

            if (target.gameObject.tag == "Facility" || target.gameObject.tag == "NyxCollector")
            {
                if (target.GetComponent<SceneObject>().state == 0)
                {
                    target.GetComponent<SceneObject>().OnOff();
                    return;
                }
            }

            target.GetComponent<SceneObject>().OpenMenu();

            //switch (target.gameObject.tag)
            //{
            //    case "Plant":
            //        if (isGather == false)
            //        {
            //            target.GetComponent<Plant>().OpenMenu();
            //        }
            //        break;

            //    case "Facility":
            //        if (target.GetComponent<Facility>().state == 0)
            //        {
            //            target.GetComponent<Facility>().OnOff();
            //        }
            //        else
            //        {
            //            target.GetComponent<Facility>().OpenMenu();
            //        }
            //        break;

            //    case "Portal":
            //        if (target.GetComponent<Portal>().isPortalReady() == true)
            //        {
            //            GetComponent<PlayerMove>().SetMovePossible(false);
            //            SceneObjectManager.instance.SaveObject();
            //            SceneChanger.instance.FadeAndLoadScene(target.GetComponent<Portal>().sceneName, target.GetComponent<Portal>().AfterMoveGrid);
            //            SoundManager.instance.PlaySE(37);
            //        }
            //        break;

            //    case "Bulb":
            //        target.GetComponent<Bulb>().OpenMenu();
            //        break;

            //    case "Nest":
            //        target.GetComponent<Nest>().OpenMenu();
            //        break;

            //    case "NyxCollector":
            //        if (target.GetComponent<NyxCollector>().state == 0)
            //        {
            //            target.GetComponent<NyxCollector>().OnOff();
            //        }
            //        else
            //        {
            //            target.GetComponent<NyxCollector>().OpenMenu();
            //        }
            //        break;
            //}
        }
    }

    public void GatherPlant(int GatherAnimationType)
    {
        PlayerDirection();
        GatherAnimation(GatherAnimationType, true);
        GetComponent<PlayerMove>().SetMovePossible(false);
    }

    void GatherAnimation(int num, bool onOff)
    {
        isGather = onOff;
        animaitor.SetInteger("GatherType", num);
        animaitor.SetBool("isGather", isGather);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        target = other.gameObject;
        isTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        target = null;
        isTrigger = false;
    }

    //애니메이션 이벤트에서 불러오는 메소드
    void GatherEnd()
    {
        if (target == null)
            return;
        GatherAnimation(0, false);
        target.GetComponent<Plant>().GetItem();
        GetComponent<PlayerMove>().SetMovePossible(true);
    }

    public void SetInteractionPossible(bool possibility)
    {
        isInteractionPossible = possibility;
    }

    public bool GetInteractionPossible()
    {
        return isInteractionPossible;
    }

    public void DisplayFT(string s, Color c, bool displayItem = false, Sprite itemSP = null)
    {
        Vector3 tempPos = transform.position;
        tempPos.x += 0.3f;

        tempPos.y += (1.8f - GameObject.FindGameObjectsWithTag("FloatingText").Length * 0.25f);
        tempPos.z -= 0.5f;
        GameObject ft = Instantiate(FT, tempPos, Quaternion.identity);

        ft.GetComponent<TextMesh>().color = c;

        if (displayItem == false)
        {
            ft.GetComponent<TextMesh>().text = s;
        }
        else
        {
            ft.GetComponent<TextMesh>().text = "　　" + s;
            GameObject itemSp = ft.transform.Find("Item").gameObject;
            itemSp.SetActive(true);
            itemSp.GetComponent<SpriteRenderer>().sprite = itemSP;
        }
    }
}