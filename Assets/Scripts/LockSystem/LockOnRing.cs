using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnRing : MonoBehaviour
{
    public LockOnReciever MyReciever;
    public LockOnTargets MyTargetClass;

    void Update()
    {
        try 
        {
            transform.LookAt(MyTargetClass.Target.gameObject.transform); 
        }
        catch 
        {
            Destroy(gameObject);
        }

    }
}
