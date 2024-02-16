using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody MyRigidBody;
    private Transform Probe;

    public GameObject Target;
    private Vector3 TargetPastPos;
    private Vector3 TargetDirection;

    private float TurnTimer = 0.5f;
    private float FlightTimer = 0f;
    [SerializeField]
    private float Speed;


    void Start()
    {
        MyRigidBody = GetComponent<Rigidbody>();
        Bullet MyBullet = GetComponent<Bullet>();
        List<GameObject> PossibleTargets = new List<GameObject>();
        if (MyBullet.Owner != null && MyBullet.Owner.MyLockOnReciever.List.Count > 0) 
        {
            foreach (LockOnTargets TargetClass in MyBullet.Owner.MyLockOnReciever.List)
            {
                if (TargetClass.LockOnProgress >= 100) 
                {
                    PossibleTargets.Add(TargetClass.Target.gameObject);
                    Debug.Log("adding " + TargetClass.Target.gameObject + "to potential target. It's lock on progress is " + TargetClass.LockOnProgress);
                }
            }
            if(PossibleTargets.Count > 0)
            {
                Target = PossibleTargets[Random.Range(0,PossibleTargets.Count)];
            } 
        }
        if (Target != null) 
        {
            TargetPastPos = Target.transform.position;
        }
    }

    void Update()
    {
        if (FlightTimer <= 1000f)
        {
            if (Target != null)
            {
                //Find the spot to hit by estimate target's velocity;
                Vector3 TargetVelocity = (Target.transform.position - TargetPastPos) / Time.deltaTime;

                float Distance = Vector3.Distance(Target.transform.position, transform.position);
                TargetDirection = Target.transform.position - transform.position + TargetVelocity * 0.6f; //Make this multiplier the mean of the randomizer at the bottom
                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, TargetDirection, 3.1415f * 0.15f * Time.fixedDeltaTime, 0));

                TurnTimer -= Time.deltaTime;
                if (TurnTimer <= 0)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, TargetDirection, 3.1415f * 0.3f, 0));

                    //RandomizeTurn
                    Vector3 TurnRandomizer = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    TurnRandomizer *= Mathf.Clamp(Distance / 200, 0, 90);
                    transform.Rotate(TurnRandomizer, Space.Self);

                    TurnTimer += Random.Range(0.2f, 1f);
                }
                TargetPastPos = Target.transform.position;
            }
        }
        else
        {
            //Destroy(gameObject);
        }
        MyRigidBody.velocity = transform.forward * Speed;
        FlightTimer += Time.fixedDeltaTime;
    }
}
