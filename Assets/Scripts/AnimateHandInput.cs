using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator MyAnimator;

    void Start()
    {
        MyAnimator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        MyAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        MyAnimator.SetFloat("Grip", gripValue); 

    }
}
