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
    [SerializeField]
    public List<LockOnTargets> List;

    [SerializeField]
    GameObject LockOnRing;

    [SerializeField]
    GameObject Radar;

    [SerializeField]
    AudioSource MySound;
    [SerializeField]
    AudioClip SoundBeep;

    void Start()
    {
        
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
            Destroy(RemovingTarget.MyRing.gameObject);
            List.Remove(RemovingTarget);
        }
    }

    public void AddLockOnProgress(LockOnTargets TargetClass, float Progress) 
    {
        TargetClass.LockOnProgress = Mathf.Clamp(TargetClass.LockOnProgress + Progress, 0, 100);
        if (TargetClass.LockOnProgress < 100)
        {
            MySound.pitch = 1f;
            MySound.volume = 0.1f;
            MySound.PlayOneShot(SoundBeep);
        }
        else 
        {
            MySound.pitch = 1.7f;
            MySound.volume = 0.2f;
            MySound.PlayOneShot(SoundBeep);
        }
        TargetClass.Timer = 10;
        TargetClass.MyRing.AnimateProgressRing(TargetClass.LockOnProgress);

    }

    public void CreateLockOnRing(LockOnTargets TargetClass) 
    {
        GameObject NewRing = Instantiate(LockOnRing, Radar.transform);
        TargetClass.MyRing = NewRing.GetComponent<LockOnRing>();
        TargetClass.MyRing.MyTargetClass = TargetClass;
    }
}
