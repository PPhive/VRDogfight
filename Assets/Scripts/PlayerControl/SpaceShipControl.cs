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

    [SerializeField]
    float turnMultiplier;
    [SerializeField]
    bool simple;

    void Start()
    {
        MyUnit = GetComponent<Unit>();
        MyRB = GetComponent<Rigidbody>();
        HeadInput = myCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();

        telemetry = GetComponent<Telemetry>();
        simRacingStudio = GetComponent<SimRacingStudio>();

        simple = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            MyUnit.myHP.TakeDamage(999999);
        }
        */

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (GetComponent<SpaceShipControl>() != null)
            {
                SpaceShipControl mySpaceShipControl = GetComponent<SpaceShipControl>();
                if (mySpaceShipControl.simple)
                {
                    mySpaceShipControl.simple = false;
                }
                else 
                {
                    mySpaceShipControl.simple = true;
                }
            }
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
            myCamera.transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);       
        }
        else 
        {
            HeadInput.enabled = true;
        }

        Quaternion relativeRotation = Quaternion.Inverse(myCamera.transform.rotation) * MyUnit.transform.rotation;
        Vector3 relativeEulerAngles = relativeRotation.eulerAngles;
        myJoystick.localEulerAngles = -relativeEulerAngles;

        if (!simple)
        {
            
            myJoystick.localEulerAngles += Vector3.forward * ((Input.GetAxis("Horizontal") + (Input.GetAxis("SteeringWheelRoll"))) * -90f);
        }
        else 
        {        
            float globalpitch = 0;
            Vector3 myUnitfront = MyUnit.transform.position + MyUnit.transform.forward;
            globalpitch = -Mathf.Atan2
                (
                myUnitfront.y - MyUnit.transform.position.y,
                Vector2.Distance(new Vector2(myUnitfront.x, myUnitfront.z), new Vector2(MyUnit.transform.position.x, MyUnit.transform.position.z))
                ) / -3.14f * 180;

            /*
            //if the pitch is too high or low, slowly disable pitching
            if (Mathf.Abs(globalpitch) >= 30f)
            {
                //Debug.Log("global pitch is " + globalpitch + " joystick euler x is " + myJoystick.localEulerAngles.x);
                if (globalpitch * Methods.i.Bound180(myJoystick.localEulerAngles.x) < 0) 
                {
                    myJoystick.localEulerAngles -= Vector3.right * Methods.i.Bound180(myJoystick.localEulerAngles.x) * ((Mathf.Abs(globalpitch) - 30) / 20);
                }     
            }
            */

            //Automatically rolling the ship back upright
            if (Mathf.Abs(globalpitch) <= 80f) 
            {
                if (Methods.i.Bound180(MyUnit.transform.localEulerAngles.z) != 0)
                {
                    float unitZ = Methods.i.Bound180(MyUnit.transform.localEulerAngles.z);
                    float unitZSign = 0;
                    if (unitZ != 0)
                    {
                        unitZSign = unitZ / Mathf.Abs(unitZ);
                    }

                    myJoystick.localEulerAngles = new Vector3(
                        myJoystick.localEulerAngles.x,
                        myJoystick.localEulerAngles.y,
                        -unitZ * 0.4f
                        );
                }
            }
        }

        //Passing the turn data to Unit
        MyUnit.JoystickRotate(myJoystick, gameObject.transform, MyModel.transform, new Vector3(0, 0, 0), turnMultiplier);

        /*
        //Roll the player object back to reduce motion sickness
        myCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * Methods.i.Bound180(MyModel.transform.localEulerAngles.z) * (-0.2f);
        */
        }
}
