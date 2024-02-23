using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    [SerializeField]
    Unit MyUnit;

    [SerializeField]
    Transform MyCamera;
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    GameObject MyModel;

    [SerializeField]
    float Speed;
    [SerializeField]
    float TurnSpeedMultiplier = 0.6f;
    [SerializeField]
    float MaxTurnSpeed;
    [SerializeField]
    float HeadAngleOffset;
    bool Upsidedown;
    bool LastUpsideDown;
    float RollSpeed;

    void Start()
    {
        MyRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    //Pitch
        //Headset as Joystick
        float turnspd = MyCamera.localEulerAngles.x + HeadAngleOffset;
        if (turnspd > 180) 
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd, -MaxTurnSpeed, MaxTurnSpeed) / TurnSpeedMultiplier;

        //Keyboard override
        if (Input.GetKey(KeyCode.W))
        {
            turnspd = MaxTurnSpeed/2;
        }
        if (Input.GetKey(KeyCode.S))
        {
            turnspd = -MaxTurnSpeed/2;
        }
        transform.Rotate(Vector3.right * Time.deltaTime * turnspd, Space.Self);

    //Yaw
        //Headset as Joystick
        turnspd = Bound180(MyCamera.localEulerAngles.y);

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd, -MaxTurnSpeed, MaxTurnSpeed) * TurnSpeedMultiplier;

        //Keyboard override
        if (Input.GetKey(KeyCode.D))
        {
            turnspd = MaxTurnSpeed/2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnspd = -MaxTurnSpeed/2;
        }


        //When plane is facing directly up and down, reduce turn speed;
        float angle = Mathf.Asin(Mathf.Abs((transform.position + transform.up).y - transform.position.y)) / 3.1415f * 180f;
        if (angle < 10)
        {
            //turnspd *= angle / 10;
        }

        
        //Roll the plane according to yaw
        float targetZ = turnspd;

        
        //RollSpeed = Mathf.Clamp(RollSpeed + Mathf.Clamp(targetZ - MyModel.transform.localEulerAngles.z, -2, 2) * Time.deltaTime, -10,10);

        float MyModelZ = Bound180(MyModel.transform.localEulerAngles.z);
        RollSpeed = targetZ + MyModelZ;
        RollSpeed = Mathf.Clamp(RollSpeed,-60, 60);
        //Debug.Log(RollSpeed);

        float CurrentZ = MyModel.transform.localEulerAngles.z - RollSpeed * Time.deltaTime;
        CurrentZ = Bound180(CurrentZ);
        MyModel.transform.localEulerAngles = new Vector3(
                                                            0,
                                                            0,
                                                            Mathf.Clamp(CurrentZ, -20, 20)
                                                        );
        //Roll the player object back to reduce motion sickness
        MyCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * Bound180(MyModel.transform.localEulerAngles.z) * (-0.5f);

        

        //If plane is upside down, reverse turn direction;
        Upsidedown = (MyCamera.position + MyCamera.up).y < MyCamera.position.y;
        if (Upsidedown)
        {
            turnspd *= -1;
        }

        //yall the plane according to turnspd
        transform.Rotate(Vector3.up * turnspd * Time.deltaTime, Space.World);

        //Reseting plane roll incase of crash
        Vector3 Tempforward = transform.position + transform.forward;
        bool ShipUpsidedown = (transform.position + transform.up).y < transform.position.y;
        if (ShipUpsidedown)
        {
            targetZ = 180;
        }
        else
        {
            targetZ = 0;
        }
        if (LastUpsideDown != ShipUpsidedown)
        {
            transform.LookAt(Tempforward);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetZ);
            transform.RotateAround(transform.position, Vector3.up, 180);
            transform.RotateAround(transform.position, transform.forward, 180);
        }
        LastUpsideDown = ShipUpsidedown;

        //Roll
        /*
        turnspd = MyCamera.transform.localPosition.x;
        if (Input.GetKey(KeyCode.A))
        {
            turnspd = MaxTurnSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnspd = -MaxTurnSpeed;
        }
        */
        MyUnit.velocity = transform.forward * Speed;
    }

    float Bound180 (float eulerZ) 
    {
        if (eulerZ < -180)
        {
            eulerZ += 360;
        }
        else if (eulerZ > 180)
        {
            eulerZ -= 360;
        }
        return eulerZ;
    }
}
