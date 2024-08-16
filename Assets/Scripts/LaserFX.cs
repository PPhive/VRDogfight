using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFX : MonoBehaviour
{
    public Vector3 TargetPos;
    float Length;
    void Update()
    {
        if (TargetPos != new Vector3()) 
        {
            transform.LookAt(TargetPos);
            Length = Vector3.Distance(TargetPos, transform.position);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Length);
        }
    }
}
