using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {

    }
	
	void Update ()
    {
		
	}

    public int PlayerGrid()
    {
        return PosToGrid(GameObject.Find("Player").transform.position);
    }

    public int PosToGrid(float x)
    {
        if (x >= 0)
        {
            return (int)x;
        }
        else
        {
            return (int)(x - 1);
        }
    }

    public int PosToGrid(Vector3 pos)
    {
        if (pos.x >= 0)
        {
            return (int)pos.x;
        }
        else
        {
            return (int)(pos.x - 1);
        }
    }

    public Vector3 GridToPos(int grid)
    {
        Vector3 temp = Vector3.zero;
        temp.x = grid + 0.5f;
        return temp;
    }
}
