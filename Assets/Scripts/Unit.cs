using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum state 
    {
        Controlled,
        Dead,
        Returning,
    }

    public state mystate;
    [SerializeField]
    public HP myHP;
    [SerializeField]
    public LockOnReciever myLockOnReciever;
    [SerializeField]
    public Rigidbody myRb;
    [SerializeField]
    private GameObject myShaker;

    //Speeds of all kind
    public float accel = 150;
    public float maxSpeed = 100;
    public float maxTurnspeed = 60;

    //Section for rotation control
    bool upsideDown;
    bool lastUpsideDown;
    public Vector3 targetVelocity; // has to be public

    //WeaponSystem
    public List<Gun> MyWeapons;

    void Start()
    {
        if (myRb == null)
        {
            myRb = GetComponent<Rigidbody>();
        }
        if (myHP == null)
        {
            myHP = GetComponent<HP>();
        }
        if (myLockOnReciever == null) 
        {
            myLockOnReciever = GetComponent<LockOnReciever>();
        }

        SetAllChildTag(transform);
        mystate = state.Controlled;
    }

    // Update is called once per frame
    void Update()
    {
        if (mystate == state.Controlled) 
        {
            if (Vector3.Distance(transform.position, new Vector3()) > 3000)
            {
                //mystate = state.Returning;
                //transform.LookAt(new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 1000f);
            }
            accelTowardVelocity();
        }
        if (mystate == state.Returning) 
        {
            accelTowardVelocity();

            if (Vector3.Distance((transform.forward * GameManager.instance.WorldRadius), new Vector3()) > 1000)
            {
                transform.RotateAround(transform.position, transform.up, maxTurnspeed/2 * Time.deltaTime);
                //transform.eulerAngles = Vector3.RotateTowards();
            }
            else 
            {
                mystate = state.Controlled;
            }
        }
    }
    private void accelTowardVelocity() 
    {
        if (myRb != null)
        {
            if (myRb.velocity != targetVelocity)
            {
                if ((targetVelocity - myRb.velocity).magnitude > accel * Time.deltaTime)
                {
                    myRb.velocity = myRb.velocity + (targetVelocity - myRb.velocity).normalized * accel * Time.deltaTime;
                }
                else
                {
                    myRb.velocity = targetVelocity;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        myRb.velocity = (transform.position - collision.GetContact(0).point).normalized * collision.relativeVelocity.magnitude;
    }

    public void Hit(float Damage)
    {
        myHP.TakeDamage(Damage);
    }

    public void SetAllChildTag(Transform thisTransform) 
    {
        thisTransform.tag = tag;
        if (thisTransform.childCount > 0)
        {
            foreach (Transform child in thisTransform)
            {
                //child.tag = tag;
                SetAllChildTag(child);
            }
        }
    }

    public void JoystickRotate(Transform Joystick, Transform GimbalOut, Transform GimbalIn, Vector3 JoystickOffset, float TurnSpeedMultiplier) 
    {
    //Pitch
        //Headset as Joystick
        float turnspd = Joystick.localEulerAngles.x + JoystickOffset.x;
        if (turnspd > 180)
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd * TurnSpeedMultiplier, -maxTurnspeed, maxTurnspeed);

        //Keyboard override
        if (Input.GetKey(KeyCode.W))
        {
            turnspd = maxTurnspeed / 2;
        }
        if (Input.GetKey(KeyCode.S))
        {
            turnspd = -maxTurnspeed / 2;
        }
        transform.Rotate(Vector3.right * Time.deltaTime * turnspd, Space.Self);

    //Yaw
        //Headset as Joystick
        turnspd = Bound180(Joystick.localEulerAngles.y);

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd * TurnSpeedMultiplier, -maxTurnspeed, maxTurnspeed);

        //Keyboard override
        if (Input.GetKey(KeyCode.D))
        {
            turnspd = maxTurnspeed / 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnspd = -maxTurnspeed / 2;
        }


        //When plane is facing directly up and down, reduce turn speed;
        float angle = Mathf.Asin(Mathf.Abs((transform.position + transform.up).y - transform.position.y)) / 3.1415f * 180f;
        if (angle < 10)
        {
            turnspd *= angle / 10;
        }


        //Roll the plane according to yaw
        float targetZ = turnspd;

        float MyModelZ = Bound180(GimbalIn.localEulerAngles.z);
        float RollSpeed = targetZ + MyModelZ;
        RollSpeed = Mathf.Clamp(RollSpeed, -30, 30);
        //Debug.Log(RollSpeed);

        float CurrentZ = GimbalIn.localEulerAngles.z - RollSpeed * Time.deltaTime;
        CurrentZ = Bound180(CurrentZ);
        GimbalIn.localEulerAngles = new Vector3(
                                                            0,
                                                            0,
                                                            Mathf.Clamp(CurrentZ, -30, 30)
                                                        );

        //Roll the player object back to reduce motion sickness
        if (tag != "Enemy") 
        {
            Joystick.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * Bound180(GimbalIn.localEulerAngles.z) * (-0.8f);
        }

        //If joystick is upside down, reverse turn direction;
        bool Upsidedown = (Joystick.position + Joystick.up).y < Joystick.position.y;
        if (Upsidedown)
        {
            turnspd *= -1;
        }

        //yall the plane according to turnspd
        transform.Rotate(Vector3.up * turnspd * Time.deltaTime, Space.World);

        //Reseting plane roll incase of crash
        Vector3 Tempforward = transform.position + transform.forward;
        upsideDown = (transform.position + transform.up).y < transform.position.y;
        if (upsideDown)
        {
            targetZ = 180;
        }
        else
        {
            targetZ = 0;
        }
        if (lastUpsideDown != upsideDown)
        {
            transform.LookAt(Tempforward);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetZ);
            transform.RotateAround(transform.position, Vector3.up, 180);
            transform.RotateAround(transform.position, transform.forward, 180);
        }

        if (angle > 30) 
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetZ);
        }

        lastUpsideDown = upsideDown;
    }

    public float Bound180(float eulerZ)
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

    public void AttemptToFireGuns(Gun.Slot FireSlot)
    {
        foreach (Gun myGun in MyWeapons)
        {
            if (myGun.MySlot == FireSlot) 
            {
                myGun.FireAttempted();
            }
        }
    }
}
