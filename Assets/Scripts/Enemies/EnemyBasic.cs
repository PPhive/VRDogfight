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
    public float StateTimer;
    public Unit myTarget;

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

    [SerializeField]
    float fireTimer;

    [SerializeField]
    LaserFX myLaser;

    void Start()
    {
        DriftSpeed = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        LeftOrRight = (Random.Range(0,1) > 0);
        myState = state.Roaming;
        StateTimer = Random.Range(3f,5f);
    }

    void Update()
    {
        //refreshes Joystick
        myJoystick.transform.localEulerAngles = new Vector3();

        //Timer that switchs state when timer runs out;
        if (StateTimer > 0 && myState != state.Returning)
        {
            StateTimer -= Time.deltaTime;
        }
        else 
        {
            StateSelect();
        }

        if (myState == state.Returning)
        {
            if (transform.position.magnitude < GameManager.instance.WorldRadius * 0.9f)
            {
                myState = state.Roaming;
            }
        }
        else if (myState == state.Roaming)
        {
            Destination = transform.position + transform.forward * 100f;
        }
        else if (myState == state.Attacking)
        {
            if (myTarget == null)
            {
                StateTimer = 0;
            }
            else 
            {
                Destination = myTarget.transform.position + myTarget.myRb.velocity * 1f;

                if (Destination != null)
                {
                    laserToward(Destination);
                }

                //Destination = myTarget.transform.position + myTarget.myRb.velocity * Mathf.Min(Vector3.Distance(myTarget.transform.position,transform.position) / 10, 4f);
                //Debug.Log(Mathf.Min(Vector3.Distance(myTarget.transform.position, transform.position) / 40, 5f));

                //Tries to attack player
                RaycastHit Hit;
                bool sphereCast = Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward), 5, transform.TransformDirection(Vector3.forward), out Hit, MyUnit.maxSpeed * 4);
                if ((sphereCast && Hit.transform.gameObject.tag == myTarget.tag) || FindMinimumDistance(Destination, transform.position, transform.position + transform.forward * 10) < 5)
                {
                    //MyUnit.AttemptToFireGuns(Gun.Slot.Left);
                    fireTimer = 1;

                    //Add function that makes enemy chase target down after successful fire.

                    if (MyUnit.myLockOnReciever != null && MyUnit.myLockOnReciever.List.Count > 0)
                    {
                        if (MyUnit.myLockOnReciever.List[0].LockOnProgress >= 100)
                        {
                            MyUnit.AttemptToFireGuns(Gun.Slot.Right);
                        }
                    }
                }
            }
        }

        //if roamed out of play area, return to roughtly center
        if ((transform.position.magnitude > GameManager.instance.WorldRadius || Mathf.Abs(transform.position.y) > GameManager.instance.WorldHeight * 1.3f)&& myState != state.Returning)
        {
            Destination = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * GameManager.instance.WorldRadius * 0.3f;
            myState = state.Returning;
            StateTimer = 0;
        }




        myJoystick.transform.LookAt(Destination);
        OrganicDrift();
        Roomba();
        MyUnit.JoystickRotate(myJoystick.transform,transform, myModel.transform, new Vector3() ,1);
        MyUnit.targetVelocity = transform.forward * MyUnit.maxSpeed * 1f;


        //Fire gun if fire timer isn't 0
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            MyUnit.AttemptToFireGuns(Gun.Slot.Left);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void StateSelect() 
    {
        if (Random.Range(0f, 1f) < 0.3f)
        {
            //Roaming
            StateTimer = Random.Range(1f, 2f);
            myState = state.Roaming;
        }
        else
        {
            //Attacking
            float Randomizer = Random.Range(0f, 1f);
            if (Randomizer < 0.3f && myTarget != null)
            {
                //Attack SameTarget
            }
            else
            {
                //Switch Targets

                //make a list of possible targets;
                List<Unit> avaliableTargets = new List<Unit>();
                Unit closestTarget = null;

                //find all teams that aren't your own
                foreach (Team thisTeam in GameManager.instance.CurrentGame.teams)
                {
                    if (thisTeam.name != tag && thisTeam != GameManager.instance.CurrentGame.teams[0])
                    {
                        foreach (Player thisPlayer in thisTeam.myPlayers)
                        {
                            if (thisPlayer.myCurrentShip != null)
                            {
                                avaliableTargets.Add(thisPlayer.myShipUnit);
                                if (closestTarget == null || Vector3.Distance(thisPlayer.transform.position, transform.position) < Vector3.Distance(closestTarget.transform.position, transform.position))
                                {
                                    closestTarget = thisPlayer.myShipUnit;
                                }
                            }
                        }
                    }
                }

                if (avaliableTargets.Count > 0)
                {
                    if (Randomizer < 0.8f)
                    {
                        //Attack Closest Target
                        myTarget = closestTarget;
                    }
                    else
                    {
                        //Attack Random Target
                        myTarget = avaliableTargets[Random.Range(0, avaliableTargets.Count)];
                    }
                }
                else
                {
                    StateTimer = 0;
                }
                StateTimer = Random.Range(10f, 20f);
                myState = state.Attacking;
            }
        }
    }

    private void SimulateGunPress() 
    {
    
    }


    private void Roomba() 
    {
        RaycastHit Hit;
        if (Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward), 3, transform.TransformDirection(Vector3.forward), out Hit, MyUnit.maxSpeed * 4))
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

    public Vector3 GetTargetPosition(Vector3 targetPos, Vector3 chaserPos, Vector3 targetVelocity, float chaserSpeed, float range)
    {
        // Calculate the relative position and velocity
        Vector3 relativePos = targetPos - chaserPos;
        Vector3 relativeVelocity = targetVelocity;

        // Calculate the coefficients of the quadratic equation
        float a = relativeVelocity.sqrMagnitude - chaserSpeed * chaserSpeed;
        float b = 2 * Vector3.Dot(relativePos, relativeVelocity);
        float c = relativePos.sqrMagnitude - range * range;

        // Solve the quadratic equation for t
        float discriminant = b * b - 4 * a * c;

        // Check if the discriminant is non-negative (a solution exists)
        if (discriminant < 0)
        {
            // No real solution exists; just head towards the target's current position
            return targetPos;
        }

        // Calculate the two possible times
        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        // Choose the smallest positive time
        float t = Mathf.Min(t1, t2);

        if (t < 0)
        {
            // No valid intercept time; move towards current target position
            return targetPos;
        }

        // Calculate the intercept point
        Vector3 interceptPoint = targetPos + targetVelocity * t;

        return interceptPoint;
    }

    void laserToward(Vector3 laserTarget) 
    {
        if (myLaser != null && myLaser.gameObject.activeInHierarchy) 
        {
            myLaser.TargetPos = laserTarget;
        }
    }

    public float FindMinimumDistance(Vector3 P, Vector3 A, Vector3 B)
    {
        // Direction vector of the line AB
        Vector3 d = B - A;

        // Vector from A to the point P
        Vector3 AP = P - A;

        // Projection length of AP onto d
        float projectionLength = Vector3.Dot(AP, d.normalized);

        // Closest point on the line AB to the point P
        Vector3 closestPoint = A + projectionLength * d.normalized;

        // Minimum distance between the point P and the line AB
        float distance = Vector3.Distance(P, closestPoint);

        return distance;
    }
}
