using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum Direction
    {
        Left,
        Right
    };

    public float speed = 5;

    Direction playerDir;
    SpriteRenderer spRenderer;
    Vector3 oldPos;
    bool isMove = false;
    Animator animaitor;
    bool isMovePossible = true;

    GameObject LeftWall;
    GameObject RightWall;

    void Start ()
    {
        playerDir = Direction.Left;
        spRenderer = GetComponent<SpriteRenderer>();
        animaitor = GetComponent<Animator>();
        oldPos = transform.position;

        LeftWall = GameObject.Find("LeftWall");
        RightWall = GameObject.Find("RightWall");
    }
	
	void Update ()
    {
        if (isMovePossible == true)
        {
            InputAndMove();
            SetDirection();
        }
    }

    void InputAndMove()
    {
        Vector3 moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVec.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVec.x += 1;
        }

        if (moveVec.x == 0)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
        animaitor.SetBool("isMove", isMove);

        transform.Translate(moveVec * speed * Time.deltaTime);

        RangeLimit();
    }

    void RangeLimit()
    {
        Vector3 temp = transform.position;
        if (transform.position.x < LeftWall.transform.position.x + 0.5f)
        {
            temp.x = LeftWall.transform.position.x + 0.5f;
            transform.position = temp;
        }
        else if (transform.position.x > RightWall.transform.position.x - 0.5f)
        {
            temp.x = RightWall.transform.position.x - 0.5f;
            transform.position = temp;
        }
    }

    void SetDirection()
    {
        if (transform.position.x > oldPos.x)
        {
            playerDir = Direction.Right;
        }
        if (transform.position.x < oldPos.x)
        {
            playerDir = Direction.Left;
        }
        oldPos = transform.position;


        if (playerDir == Direction.Left)
        {
            spRenderer.flipX = false;
        }
        else
        {
            spRenderer.flipX = true;
        }
    }

    // 0:왼쪽, 1:오른쪽
    public void SetDirection(int dir)
    {
        oldPos = transform.position;
        if (dir == 0)
        {
            playerDir = Direction.Left;
            spRenderer.flipX = false;
        }
        else if (dir == 1)
        {
            playerDir = Direction.Right;
            spRenderer.flipX = true;
        }
    }

    public void SetMovePossible(bool possibility)
    {
        isMovePossible = possibility;

        if (possibility == false)
        {
            animaitor.SetBool("isMove", false);
        }
    }

    public bool GetMovePossible()
    {
        return isMovePossible;
    }
}