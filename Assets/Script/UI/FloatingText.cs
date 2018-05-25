using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    float speed = 0.3f;

	void Start ()
    {
    }
	
	void Update ()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
