using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum Direction { Left, Right };

    public float speed = 5;

    Direction playerDir;
    SpriteRenderer spRenderer;

    void Start ()
    {
        playerDir = Direction.Left;
        spRenderer = GetComponent<SpriteRenderer>();
    }
	
	void Update ()
    {
		if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            playerDir = Direction.Left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerDir = Direction.Right;
        }

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