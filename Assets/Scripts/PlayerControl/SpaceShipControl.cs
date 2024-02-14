using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    [SerializeField]
    Transform MyCamera;
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    GameObject MyModel;



    [SerializeField]
    float Speed;
    [SerializeField]
    float MaxTurnSpeed;
    [SerializeField]
    float HeadAngleOffset;
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
        turnspd = Mathf.Clamp(turnspd, -MaxTurnSpeed, MaxTurnSpeed) / 2;

        //Keyboard override
        if (Input.GetKey(KeyCode.W))
        {
            turnspd = MaxTurnSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            turnspd = -MaxTurnSpeed;
        }
        transform.Rotate(Vector3.right * Time.deltaTime * turnspd, Space.Self);

    //Yaw
        //Headset as Joystick
        turnspd = MyCamera.localEulerAngles.y;
        if (turnspd > 180)
        {
            turnspd -= 360;
        }

        //Clamping to max speed
        turnspd = Mathf.Clamp(turnspd, -MaxTurnSpeed, MaxTurnSpeed) / 2;

        //Keyboard override
        if (Input.GetKey(KeyCode.D))
        {
            turnspd = MaxTurnSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnspd = -MaxTurnSpeed;
        }

        //If plane is upside down, reverse turn direction;
        if ((transform.position + transform.up).y < transform.position.y) 
        {
            turnspd *= -1;
        }

        //When plane is facing directly up and down, reduce turn speed;
        float angle = Mathf.Asin(Mathf.Abs((transform.position + transform.up).y - transform.position.y)) / 3.1415f * 180f;
        if (angle < 20)
        {
            turnspd *= angle / 20;
        }

        MyModel.transform.localEulerAngles = -Vector3.forward * turnspd / 2;
        MyCamera.transform.parent.transform.parent.transform.localEulerAngles = Vector3.forward * turnspd;

        transform.Rotate(Vector3.up * turnspd * Time.deltaTime, Space.World);

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
    }

    private void FixedUpdate()
    {
        MyRB.velocity = transform.forward * Speed;
    }

    void JoyStick() 
    {
        
    }
}
