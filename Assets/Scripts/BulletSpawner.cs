using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public Projectile AssembledProjectile;
    public GameObject bullet;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire() 
    {
        GameObject Mybullet = Instantiate(bullet, transform.position + transform.forward * AssembledProjectile.size/2, transform.rotation);
        Mybullet.GetComponent<Bullet>().MyProjectile = AssembledProjectile;
    }
}
