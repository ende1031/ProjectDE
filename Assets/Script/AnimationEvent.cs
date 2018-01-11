using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
	void ActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
