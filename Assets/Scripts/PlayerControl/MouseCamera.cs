using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class MouseCamera : MonoBehaviour
{
    [SerializeField]
    bool MouseOverride = false;
    [SerializeField]
    UnityEngine.InputSystem.XR.TrackedPoseDriver HeadInput;
    Vector2 turn;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            if (MouseOverride) 
            {
                MouseOverride = false;
            }
            else 
            {
                MouseOverride = true;
            }
        }

        if (MouseOverride)
        {
            Cursor.lockState = CursorLockMode.Locked;
            HeadInput.enabled = false;
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler(-turn.y,turn.x,0);
        }
        else 
        {
            
        }
    }
}
