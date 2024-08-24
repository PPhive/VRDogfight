using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Player myPlayer;
    public bool AI;
    public enum state
    {
        Controlled,
        Dead,
        Returning,
    }
    public state mystate;
    public Player lasthitFrom;

    [SerializeField]
    public HP myHP;
    [SerializeField]
    public LockOnReciever myLockOnReciever;
    [SerializeField]
    public Rigidbody myRb;
    [SerializeField]
    private GameObject myShaker;
    [SerializeField]
    public GameObject mount;//where the player object will be located
    [SerializeField]
    public GameObject scope;

    //Speeds of all kind
    public float accel = 150;
    public float maxSpeed = 100;
    public float maxTurnspeed = 60;
    public bool keyBoardControlOn = false;

    //Section for rotation control
    bool upsideDown;
    bool lastUpsideDown;
    public Vector3 targetVelocity; // has to be public

    //WeaponSystem
    public List<Gun> MyWeapons;

    void Awake()
    {

    }

    void Start()
    {
        if (myPlayer == null)
        {
            Debug.Log("No player found, setting team as Neutral");
            tag = "Neutral";
        }
        else
        {
            AI = myPlayer.AI;
        }
        SetAllChildTag(transform);

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
                transform.RotateAround(transform.position, transform.up, maxTurnspeed / 2 * Time.deltaTime);
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
        Vector3 JoystickAnglesClamped90 = Joystick.localEulerAngles;
        JoystickAnglesClamped90.x = Mathf.Clamp(Methods.i.Bound180(JoystickAnglesClamped90.x), -90, 90);
        JoystickAnglesClamped90.y = Mathf.Clamp(Methods.i.Bound180(JoystickAnglesClamped90.y), -90, 90); ;
        JoystickAnglesClamped90.z = Mathf.Clamp(Methods.i.Bound180(JoystickAnglesClamped90.z), -90, 90); ;

        Vector3 SeatMotion = new Vector3();

        //Pitch
        float turnspd = JoystickAnglesClamped90.x + JoystickOffset.x;
        if (turnspd > 180)
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd * TurnSpeedMultiplier, -maxTurnspeed, maxTurnspeed);

        //Keyboard override
        if (keyBoardControlOn)
        {
            if (Input.GetKey(KeyCode.W))
            {
                turnspd = maxTurnspeed / 2;
            }
            if (Input.GetKey(KeyCode.S))
            {
                turnspd = -maxTurnspeed / 2;
            }
        }

        SeatMotion.x = -turnspd;
        transform.Rotate(Vector3.right * Time.deltaTime * turnspd, Space.Self);

        //Yaw
        turnspd = JoystickAnglesClamped90.y + JoystickOffset.y;
        if (turnspd > 180)
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd * TurnSpeedMultiplier, -maxTurnspeed, maxTurnspeed);

        //Keyboard override
        if (keyBoardControlOn)
        {
            if (Input.GetKey(KeyCode.E))
            {
                turnspd = maxTurnspeed / 2;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                turnspd = -maxTurnspeed / 2;
            }
        }

        SeatMotion.y = -turnspd;
        transform.Rotate(Vector3.up * Time.deltaTime * turnspd, Space.Self);

        //Roll
        turnspd = JoystickAnglesClamped90.z + JoystickOffset.z;
        if (turnspd > 180)
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd * TurnSpeedMultiplier, -maxTurnspeed, maxTurnspeed);

        //Keyboard override
        if (keyBoardControlOn)
        {
            if (Input.GetKey(KeyCode.A))
            {
                turnspd = maxTurnspeed / 2;
            }
            if (Input.GetKey(KeyCode.D))
            {
                turnspd = -maxTurnspeed / 2;
            }
        }

        SeatMotion.z = -turnspd;
        transform.Rotate(Vector3.forward * Time.deltaTime * turnspd, Space.Self);

        

        //For motionseat telemetry
        if (myPlayer != null && !myPlayer.AI)
        {
            float PitchbySpeed = Vector3.Dot(myRb.velocity, transform.forward) * Mathf.Clamp(myRb.velocity.magnitude / maxSpeed / transform.localScale.magnitude, 0, 5);
            SeatMotion.x += PitchbySpeed;
            GetComponent<SpaceShipControl>().telemetry.PitchYawRoll = SeatMotion;
        }
    }

    public void AttemptToFireGuns(Gun.Slot FireSlot)
    {
        foreach (Gun myGun in MyWeapons)
        {
            if (myGun.isActiveAndEnabled) 
            {
                if (myGun.MySlot == FireSlot)
                {
                    myGun.FireAttempted();
                }
            }
        }
    }

    public bool IsPlayerControlled()//Is this unit controlled by an actual player?
    {
        if (myPlayer != null) 
        {
            if (!myPlayer.AI) 
            {
                return true;
            }
        }
        return false;
    }
}
