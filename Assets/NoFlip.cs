using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFlip : MonoBehaviour
{
    void Update()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
