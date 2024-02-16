using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnRing : MonoBehaviour
{
    public LockOnReciever MyReciever;
    public LockOnTargets MyTargetClass;


    [SerializeField]
    float TargetProgress = 0;
    [SerializeField]
    float CurrentProgress;
    [SerializeField]
    float ProgressDifference = 0;
    [SerializeField]
    float Timer = 0;
    [SerializeField]
    float TimerMax = 0.5f;

    [SerializeField]
    private GameObject BaseRing;

    [SerializeField]
    private GameObject Spinner;
    [SerializeField]
    private GameObject LeftMask;
    [SerializeField]
    private GameObject RightMask;
    [SerializeField]
    private GameObject LeftFill;
    [SerializeField]
    private GameObject RightFill;
    [SerializeField]
    private GameObject LeftFinished;

    [SerializeField]
    private GameObject Spinner2;
    [SerializeField]
    private GameObject LeftMask2;
    [SerializeField]
    private GameObject RightMask2;
    [SerializeField]
    private GameObject LeftFill2;
    [SerializeField]
    private GameObject RightFill2;
    [SerializeField]
    private GameObject LeftFinished2;

    [SerializeField]
    private GameObject LockedOn;

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

        if (Timer > 0) 
        {
            Timer -= Time.deltaTime;
            if (TargetProgress >= 100)
            {
                transform.localScale = Vector3.forward + new Vector3(1,1,0) * (1 - Timer/2);
            }
            CurrentProgress = Mathf.Lerp(TargetProgress - ProgressDifference, TargetProgress, ((TimerMax - Timer)/TimerMax) * ((TimerMax - Timer) / TimerMax));
            RingUpdate();
        }
    }

    void RingUpdate() 
    {
        Spinner.transform.localEulerAngles = new Vector3(0, 0, 360 - CurrentProgress / 100 * 360);
        if (CurrentProgress < 50)
        {
            BaseRing.SetActive(true);
            LeftMask.SetActive(false);
            RightMask.SetActive(true);
            LeftFill.SetActive(false);
            RightFill.SetActive(true);
            LeftFinished.SetActive(false);
            LockedOn.SetActive(false);
        }
        else if (CurrentProgress >= 50 && CurrentProgress < 100)
        {
            BaseRing.SetActive(true);
            LeftMask.SetActive(true);
            RightMask.SetActive(false);
            LeftFill.SetActive(true);
            RightFill.SetActive(false);
            LeftFinished.SetActive(true);
            LockedOn.SetActive(false);
        }
        else
        {
            BaseRing.SetActive(false);
            LeftMask.SetActive(false);
            RightMask.SetActive(false);
            LeftFill.SetActive(false);
            RightFill.SetActive(false);
            LeftFinished.SetActive(false);
            LockedOn.SetActive(true);
        }

        Spinner2.transform.localEulerAngles = new Vector3(0, 0, 360 - TargetProgress / 100 * 360);
        if (TargetProgress < 50)
        {
            LeftMask2.SetActive(false);
            RightMask2.SetActive(true);
            LeftFill2.SetActive(false);
            RightFill2.SetActive(true);
            LeftFinished2.SetActive(false);
        }
        else if (CurrentProgress < 100)
        {
            LeftMask2.SetActive(true);
            RightMask2.SetActive(false);
            LeftFill2.SetActive(true);
            RightFill2.SetActive(false);
            LeftFinished2.SetActive(true);
        }
        else
        {
            LeftMask2.SetActive(false);
            RightMask2.SetActive(false);
            LeftFill2.SetActive(false);
            RightFill2.SetActive(false);
            LeftFinished2.SetActive(false);
        }
    }

    public void AnimateProgressRing(float Progress) 
    {
        CurrentProgress = TargetProgress;
        Timer = TimerMax;
        TargetProgress = Progress;
        ProgressDifference = TargetProgress - CurrentProgress;
    }
}
