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

        if (!simple)
        {
            myJoystick.localEulerAngles = myCamera.transform.localEulerAngles;
            myJoystick.localEulerAngles += Vector3.forward * ((Input.GetAxis("Horizontal") + (Input.GetAxis("SteeringWheelRoll"))) * -90f);
        }
        else 
        {
            myJoystick.localEulerAngles = myCamera.transform.localEulerAngles;
            float globalpitch = 0;
            Vector3 myUnitfront = MyUnit.transform.position + MyUnit.transform.forward;
            globalpitch = -Mathf.Atan2
                (
                myUnitfront.y - MyUnit.transform.position.y,
                Vector2.Distance(new Vector2(myUnitfront.x, myUnitfront.z), new Vector2(MyUnit.transform.position.x, MyUnit.transform.position.z))
                ) / -3.14f * 180;


            if (Mathf.Abs(globalpitch) >= 30f) // if the pitch is too high or low
            {
                Debug.Log("global pitch is " + globalpitch + " joystick euler x is " + myJoystick.localEulerAngles.x);
                if (globalpitch * Methods.i.Bound180(myJoystick.localEulerAngles.x) < 0) 
                {
                    myJoystick.localEulerAngles -= Vector3.right * Methods.i.Bound180(myJoystick.localEulerAngles.x) * ((Mathf.Abs(globalpitch) - 30) / 15);
                }     
            }

            float simpleRoll = Mathf.Clamp(Methods.i.Bound180(myJoystick.transform.localEulerAngles.y) * turnMultiplier / 4, -MyUnit.maxTurnspeed / 4, MyUnit.maxTurnspeed / 4);
            MyUnit.transform.localEulerAngles -= MyUnit.transform.localEulerAngles.z * Vector3.forward;
            MyUnit.transform.localEulerAngles -= simpleRoll * Vector3.forward;
        }

        if (!MouseOverride) 
        {
            myJoystick.localEulerAngles += new Vector3(10, 0, 0);
        }

        //Passing the turn data to Unit
        MyUnit.JoystickRotate(myJoystick, gameObject.transform, MyModel.transform, new Vector3(0, 0, 0), turnMultiplier);

        //Roll the player object back to reduce motion sickness
        myCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * Methods.i.Bound180(MyModel.transform.localEulerAngles.z) * (-0.2f);
    }
}
