using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField]
    private Unit MyUnit;
    [SerializeField]
    private Rigidbody MyRB;
    [SerializeField]
    private HP MyHP;
    [SerializeField]
    private GameObject MyShaker;

    [SerializeField]
    bool LeftOrRight;
    [SerializeField]
    Vector3 DriftSpeed;

    void Start()
    {
        DriftSpeed = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        LeftOrRight = (Random.Range(0,1) > 0);
    }

    void Update()
    {
        //transform.Rotate(new Vector3(10, 10, 10) * Time.deltaTime);
        OrganicDrift();
        Roomba();
        if (Vector3.Distance(transform.position, new Vector3()) > 3000)
        {
            transform.LookAt(new Vector3(Random.Range(-1,1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 1000f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyUnit.velocity = transform.forward * 140;
    }

    private void Roomba() 
    {
        RaycastHit Hit;
        if (Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward) * 50, 50, transform.TransformDirection(Vector3.forward), out Hit, 300))
        {
            if (Hit.transform.gameObject.tag != "Bullet") 
            {
                float direction = 1f;
                if (LeftOrRight) 
                {
                    direction = -1f;
                }
                transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime * (300 / Hit.distance) * direction);
            }
        }
    }

    private void OrganicDrift()
    {
        transform.Rotate(new Vector3(DriftSpeed.x * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.y * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.z * Mathf.Sin(Time.realtimeSinceStartup)) * Time.deltaTime, Space.Self);
    }
}
