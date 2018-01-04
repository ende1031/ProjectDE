using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum Direction { Left, Right };

    public float speed = 5;

    Direction playerDir;
    SpriteRenderer spRenderer;
    Vector3 oldPos;

    void Start ()
    {
        playerDir = Direction.Left;
        spRenderer = GetComponent<SpriteRenderer>();
        oldPos = transform.position;
    }
	
	void Update ()
    {
        InputAndMove();
        SetDirection();
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
}