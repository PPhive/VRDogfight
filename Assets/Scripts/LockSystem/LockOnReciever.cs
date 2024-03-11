using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LockOnTargets
{
    public LockOnAble Target;
    public float LockOnProgress;
    public float Timer;
    public LockOnRing MyRing;
}

public class LockOnReciever : MonoBehaviour
{
    bool isPlayerControlled = false;

    [SerializeField]
    public List<LockOnTargets> List;

    [SerializeField]
    GameObject LockOnRing;
    public GameObject Radar;

    [SerializeField]
    AudioSource MySound;
    [SerializeField]
    AudioClip SoundBeep;

    void Start()
    {
        isPlayerControlled = GetComponent<Unit>().IsPlayerControlled();
    }

    void Update()
    {
        List<LockOnTargets> ToBeRemoved = new List<LockOnTargets>();
        foreach (LockOnTargets Locked in List) 
        {     
            if (Locked.Timer <= 0)
            {
                ToBeRemoved.Add(Locked);
            }
            else 
            {
                Locked.Timer -= Time.deltaTime;
            }
        }
        foreach (LockOnTargets RemovingTarget in ToBeRemoved)
        {
            if (RemovingTarget.MyRing != null) 
            {
                Destroy(RemovingTarget.MyRing.gameObject);
            }
            List.Remove(RemovingTarget);
        }
    }

    public void AddLockOnProgress(LockOnTargets TargetClass, float Progress) 
    {
        TargetClass.LockOnProgress = Mathf.Clamp(TargetClass.LockOnProgress + Progress, 0, 100);
        if (isPlayerControlled) 
        {
            if (TargetClass.LockOnProgress < 100)
            {
                MySound.pitch = 1f;
                MySound.volume = 0.07f;
            }
            else
            {
                MySound.pitch = 1.7f;
                MySound.volume = 0.1f;
            }
            MySound.PlayOneShot(SoundBeep);
            TargetClass.MyRing.AnimateProgressRing(TargetClass.LockOnProgress);
        }
        TargetClass.Timer = 10;
    }

    public void CreateLockOnRing(LockOnTargets TargetClass) 
    {
        if (isPlayerControlled) 
        {
        GameObject NewRing = Instantiate(LockOnRing, Radar.transform);
        TargetClass.MyRing = NewRing.GetComponent<LockOnRing>();
        TargetClass.MyRing.MyTargetClass = TargetClass;
        }
    }
}
