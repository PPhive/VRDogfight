using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnAble : MonoBehaviour
{
    public List<LockOnReciever> List;

    public void RemoveMyself() 
    {
        foreach (LockOnReciever ThisReciever in List) 
        {
            foreach (LockOnTargets TargetClass in ThisReciever.List) 
            {
                if (TargetClass.Target == this) 
                {
                    ThisReciever.List.Remove(TargetClass);
                    Debug.Log("Remove Attempted");
                    break;
                }
            }
        }
        this.enabled = false;
    }
}
