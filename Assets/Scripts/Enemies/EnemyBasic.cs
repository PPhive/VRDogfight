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
    float ShakeTimer;

    [SerializeField]
    Vector3 DriftSpeed;

    void Start()
    {
        DriftSpeed = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }

    void Update()
    {
        //transform.Rotate(new Vector3(10, 10, 10) * Time.deltaTime);
        OrganicDrift();
        Roomba();
        Shake();
        if (Vector3.Distance(transform.position, new Vector3()) > 3000)
        {
            transform.LookAt(new Vector3(Random.Range(-1,1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 1000f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyRB.velocity = transform.forward * 90;
    }

    public void Hit(float Damage)
    {
        MyHP.TakeDamage(Damage);
        ShakeTimer = Random.Range(0.08f, 0.1f) * Damage;
    }

    private void Shake() 
    {
        if (ShakeTimer > 0) 
        {
            MyShaker.transform.localEulerAngles = new Vector3(ShakeTimer * 10 * Mathf.Sin(ShakeTimer * 7f), ShakeTimer * 20 * Mathf.Sin(ShakeTimer * 10f) * Time.deltaTime, 0);
            ShakeTimer -= Time.deltaTime;
        }
    }

    private void Roomba() 
    {
        RaycastHit Hit;
        if (Physics.SphereCast(transform.position + transform.TransformDirection(Vector3.forward) * 50, 50, transform.TransformDirection(Vector3.forward), out Hit, 300))
        {
            if (Hit.transform.gameObject.tag != "Bullet") 
            {
                transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime * (300 / Hit.distance));
            }
        }
    }

    private void OrganicDrift()
    {
        transform.Rotate(new Vector3(DriftSpeed.x * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.y * Mathf.Sin(Time.realtimeSinceStartup), DriftSpeed.z * Mathf.Sin(Time.realtimeSinceStartup)) * Time.deltaTime, Space.Self);
    }

}
