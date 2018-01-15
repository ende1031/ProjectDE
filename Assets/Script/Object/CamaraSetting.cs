using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSetting : MonoBehaviour
{
    public Vector2 ScreenSize;

    void Awake()
    {
        Screen.SetResolution((int)ScreenSize.x, (int)ScreenSize.y, false);
    }
}
