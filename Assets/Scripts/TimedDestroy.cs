using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float timer;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
