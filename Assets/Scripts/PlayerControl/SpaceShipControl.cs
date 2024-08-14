using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    [SerializeField]
    Unit MyUnit;
    [SerializeField]
    Transform myJoystick;
    public Telemetry telemetry;
    public SimRacingStudio simRacingStudio;

    [SerializeField]
    public GameObject myCamera;
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    GameObject MyModel;
    [SerializeField]
    float Throttle = 1;

    [SerializeField]
    public bool MouseOverride = false;
    [SerializeField]
    public UnityEngine.InputSystem.XR.TrackedPoseDriver HeadInput;
    Vector3 turn;

    void Start()
    {
        MyUnit = GetComponent<Unit>();
        MyRB = GetComponent<Rigidbody>();
        HeadInput = myCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();

        telemetry = GetComponent<Telemetry>();
        simRacingStudio = GetComponent<SimRacingStudio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            MyUnit.myHP.TakeDamage(999999);
        }

        MyUnit.targetVelocity = transform.forward * MyUnit.maxSpeed * Throttle;

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
            //turn.z += Input.GetAxis("Horizontal"); //This is qe or steering wheel, check keyboard manager
            myCamera.transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            
        }
        else 
        {
            HeadInput.enabled = true;
        }

        myJoystick.localEulerAngles = myCamera.transform.localEulerAngles;
        myJoystick.localEulerAngles += Vector3.forward * ((Input.GetAxis("Horizontal") + (Input.GetAxis("SteeringWheelRoll"))) * -90f);

        //Passing the turn data to Unit
        MyUnit.JoystickRotate(myJoystick, gameObject.transform, MyModel.transform, new Vector3(0, 0, 0), 2, true);

        //Roll the player object back to reduce motion sickness
        myCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * MyUnit.Bound180(MyModel.transform.localEulerAngles.z) * (-0.2f);
    }
}
