using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methods : MonoBehaviour
{
    public static Methods i;
    void Awake()
    {
        if (i != null) 
        {
            Destroy(this);
        }
        else 
        {
            i = this;
        }
    }

    public GameObject CheckTopParent(GameObject Checking)
    {
        if (Checking.transform.parent != null)
        {
            return CheckTopParent(CheckTopParent(Checking.transform.parent.gameObject));
        }
        else
        {
            return Checking;
        }
    }

    public float Bound180(float eulerZ)
    {
        if (eulerZ < -180)
        {
            eulerZ += 360;
        }
        else if (eulerZ > 180)
        {
            eulerZ -= 360;
        }
        return eulerZ;
    }
}
