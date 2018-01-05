﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum Direction { Left, Right };

    public float speed = 5;

    Direction playerDir;
    SpriteRenderer spRenderer;
    Vector3 oldPos;
    bool isMove = false;
    Animator animaitor;
    bool isMovePossible = true;

    void Start ()
    {
        playerDir = Direction.Left;
        spRenderer = GetComponent<SpriteRenderer>();
        animaitor = GetComponent<Animator>();
        oldPos = transform.position;
    }
	
	void Update ()
    {
        if (isMovePossible == true)
        {
            InputAndMove();
            SetDirection();
        }

        //테스트용 코드
        if (Input.GetKeyUp(KeyCode.A))
        {
            print(Grid.instance.PlayerGrid());
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

    public void SetMovePossible(bool possibility)
    {
        isMovePossible = possibility;
    }
}