using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public Unit Owner;
    public GameObject Explosion;

    public float HitPoints = 1;
    public void TakeDamage(float Dmg) 
    {
        HitPoints -= Dmg;
        if (HitPoints <= 0) 
        {
            for (int i = 0; i < 4; i++) 
            {
                GameObject MyExplosion = Instantiate(Explosion, transform.position, Random.rotation);
                MyExplosion.transform.position += transform.forward * 1.5f;
            }

            BroadcastMessage("RemoveMyself");

            if (transform.GetComponent<SpaceShipControl>() != null) 
            {
                transform.BroadcastMessage("UnMount");
            }

            Destroy(gameObject);
        }
    }
}
