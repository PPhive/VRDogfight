using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public Unit myUnit;
    public GameObject Explosion;
    public bool destroyed;
    public float HitPoints = 1;

    public void TakeDamage(float Dmg) 
    {
        HitPoints -= Dmg;
        if (!destroyed) 
        {
            if (HitPoints <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject MyExplosion = Instantiate(Explosion, transform.position, Random.rotation);
                    MyExplosion.transform.position += transform.forward * 1.5f;
                }

                BroadcastMessage("RemoveMyself");

                if (myUnit.myPlayer != null)
                {
                    transform.BroadcastMessage("UnMount");
                    myUnit.myPlayer = null;
                }

                GameManager.instance.UnitDestroyed(myUnit.lasthitFrom, myUnit);
                destroyed = true;
                Destroy(gameObject);
                Debug.Log("I shouldve been destroyed");
            }
        }
    }
}
