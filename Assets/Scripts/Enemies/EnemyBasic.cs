using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    public enum state
    {
        Roaming,
        Returning,
        Dodging,
        Attacking,
    }

    public state myState;

    [SerializeField]
    private Unit MyUnit;
    [SerializeField]
    private Rigidbody MyRB;
    [SerializeField]
    private HP MyHP;
    [SerializeField]
    private GameObject myModel;
    [SerializeField]
    private GameObject MyShaker;
    [SerializeField]
    private GameObject myJoystick;

    [SerializeField]
    bool LeftOrRight;
    [SerializeField]
    Vector3 DriftSpeed;

    [SerializeField]
    Vector3 Destination;

    void Start()
    {
        DriftSpeed = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        LeftOrRight = (Random.Range(0,1) > 0);
        myState = state.Roaming;
    }

    void Update()
    {
        //refreshes Joystick
        myJoystick.transform.localEulerAngles = new Vector3();

        RaycastHit Hit;
        if (Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward), 3, transform.TransformDirection(Vector3.forward), out Hit, MyUnit.maxSpeed * 2))
        {
            if (Hit.transform.gameObject.tag == "TeamA")
            {
                MyUnit.AttemptToFireGuns(Gun.Slot.Left);
            }
        }

        if (myState == state.Roaming)
        {
            Destination = transform.position + transform.forward * 100f;

            if (transform.position.magnitude > GameManager.instance.WorldRadius && myState != state.Returning)
            {
                Destination = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * GameManager.instance.WorldRadius * 0.3f;
                myState = state.Returning;
            }
            else if (true) 
            {
            
            }
        }
        else if (myState == state.Returning)
        {
            if (transform.position.magnitude < GameManager.instance.WorldRadius * 0.9f) 
            {
                myState = state.Roaming;
            }
        }
        else if (myState == state.Dodging)
        {
        }

        //if roamed out of play area, return to roughtly center


        myJoystick.transform.LookAt(Destination);
        OrganicDrift();
        Roomba();
        MyUnit.JoystickRotate(myJoystick.transform,transform, myModel.transform, new Vector3() ,1);
        MyUnit.targetVelocity = transform.forward * MyUnit.maxSpeed * 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Roomba() 
    {
        RaycastHit Hit;
        if (Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward), 3, transform.TransformDirection(Vector3.forward), out Hit, MyUnit.maxSpeed * 2))
        {
            if (Hit.transform.gameObject.tag == "Terrain") 
            {
                float direction = 1f;
                if (LeftOrRight) 
                {
                    direction = -1f;
                }
                myJoystick.transform.Rotate(new Vector3(0, 20, 0) * (MyUnit.maxSpeed * 2 / Hit.distance) * direction);
            }
        }
    }

    private void OrganicDrift()
    {
        myJoystick.transform.Rotate(new Vector3(DriftSpeed.x * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.y * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.z * Mathf.Sin(Time.realtimeSinceStartup)), Space.Self);
    }
}
