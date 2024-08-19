using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleProximity : MonoBehaviour
{
    [SerializeField]
    Missle myMissle;

    //SET THE TRIGGER OFF IN INSPECTOR TO PREVENT MISSLE SCRIPT USING THIS AS COLLIDER FOR HIT
    void Start()
    {
        if (myMissle == null) 
        {
            myMissle = transform.parent.GetComponent<Missle>();
        }

        if (myMissle != null && myMissle.Target != null)
        {
            Destroy(gameObject);
        }

        transform.parent = null;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    void FixedUpdate()
    {
        if (myMissle == null)
        {
            Destroy(gameObject);
        }
        else 
        {
            transform.position = myMissle.transform.position;
            transform.rotation = myMissle.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myMissle == null)
        {
            Destroy(gameObject);
        }
        else 
        {
            if (other.tag != myMissle.GetComponent<Bullet>().Owner.tag && other.tag != "Terrain" && other.tag != "Bullet")
            {
                if (myMissle != null && myMissle.Target == null)
                {
                    Unit TargetUnit = Bullet.FindParentUnit(other.transform);
                    if (TargetUnit != null)
                    {
                        myMissle.Target = TargetUnit.gameObject;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
