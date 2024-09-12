using System;
using UnityEngine;

[Serializable]
public class EnemyWave : MonoBehaviour
{
    private void Update()
    {
        if(transform.childCount<=0)
            Destroy(gameObject);
    }
}