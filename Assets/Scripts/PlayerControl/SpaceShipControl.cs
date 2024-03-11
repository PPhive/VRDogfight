using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    [SerializeField]
    Unit MyUnit;

    [SerializeField]
    public GameObject myCamera;
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    GameObject MyModel;
    [SerializeField]
    float Speed;

    [SerializeField]
    public bool MouseOverride = false;
    [SerializeField]
    UnityEngine.InputSystem.XR.TrackedPoseDriver HeadInput;
    Vector2 turn;

    void Start()
    {
        MyUnit = GetComponent<Unit>();
        MyRB = GetComponent<Rigidbody>();
        HeadInput = myCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
        Speed = Mathf.Clamp(Speed, 0, MyUnit.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            MyUnit.myHP.TakeDamage(114514);
        }

        MyUnit.targetVelocity = transform.forward * Speed;

        //Mouse override
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
            myCamera.transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            MyUnit.JoystickRotate(myCamera.transform, gameObject.transform, MyModel.transform, new Vector3(0, 0, 0), 2);
        }
        else 
        {
            HeadInput.enabled = true;
            MyUnit.JoystickRotate(myCamera.transform, gameObject.transform, MyModel.transform, new Vector3(0, 0, 0), 2);
        }

        //Roll the player object back to reduce motion sickness
        myCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * MyUnit.Bound180(MyModel.transform.localEulerAngles.z) * (-0.2f);
    }
}
