using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody MyRigidBody;
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
        if (MyBullet.Owner != null && MyBullet.Owner.myShipUnit.myLockOnReciever.List.Count > 0) 
        {
            foreach (LockOnTargets TargetClass in MyBullet.Owner.myShipUnit.myLockOnReciever.List)
            {
                if (TargetClass.LockOnProgress >= 100) 
                {
                    PossibleTargets.Add(TargetClass.Target.gameObject);
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
        if (Target != null)
        {
            //Find the spot to hit by estimate target's velocity;
            Vector3 TargetVelocity = (Target.transform.position - TargetPastPos) / Time.deltaTime;

            float Distance = Vector3.Distance(Target.transform.position, transform.position);
            TargetDirection = Target.transform.position - transform.position + TargetVelocity * 0f;
            TurnTimer -= Time.deltaTime;
            if (TurnTimer <= 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, TargetDirection, 3.1415f * 0.2f, 0));

                //RandomizeTurn
                Vector3 TurnRandomizer = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                TurnRandomizer *= Mathf.Clamp(Distance / 200, 5, 30);
                transform.Rotate(TurnRandomizer, Space.Self);

                TurnTimer += Random.Range(0.1f, 0.6f);
            }
            TargetPastPos = Target.transform.position;
        }
        else
        {
            //Proxmity detection?
        }
        MyRigidBody.velocity = transform.forward * Speed;
        FlightTimer += Time.fixedDeltaTime;
    }
}
