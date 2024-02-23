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

    [SerializeField]
    public HP myHP;
    [SerializeField]
    public LockOnReciever myLockOnReciever;
    [SerializeField]
    Rigidbody myRb;
    [SerializeField]
    private GameObject myShaker;
    public float accel = 150;
    public Vector3 velocity;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (myRb != null) 
        {
            if (myRb.velocity != velocity) 
            {
                if ((velocity - myRb.velocity).magnitude > accel * Time.deltaTime)
                {
                    myRb.velocity = myRb.velocity + (velocity - myRb.velocity).normalized * accel * Time.deltaTime;
                    Debug.Log("accelerating");
                }
                else 
                {
                    myRb.velocity = velocity;
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
}
