using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public List<GunPart> MyParts;
    public List<GameEventListener> MyBulletSpanwers;
    public UnityEvent FireMyGun;

    public Projectile ProjectileData;
    public Projectile ProjectileDataBonus;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            FindMyParts();
        }
    }

    void FindMyParts() 
    {
        MyParts.Clear();
        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).GetComponent<GunPart>() != null) 
            {
                MyParts.Add(transform.GetChild(i).GetComponent<GunPart>());
            }
        }

        FireMyGun.RemoveAllListeners();

        AssembleBullet();

        foreach (GunPart part in MyParts) 
        {   
            if (part.GetComponent<BulletSpawner>() != null) 
            {
                Debug.Log("hi");
                FireMyGun.AddListener(part.gameObject.GetComponent<BulletSpawner>().Fire);
                part.gameObject.GetComponent<BulletSpawner>().AssembledProjectile = new Projectile(ProjectileData, ProjectileDataBonus);
            }
        }
    }

    void AssembleBullet() 
    {
        ProjectileDataBonus = new Projectile();
        foreach (GunPart part in MyParts) 
        {
            ProjectileDataBonus = new Projectile(ProjectileDataBonus, part.ProjectileBonus);
        }
    }

    public void Fire() 
    {
        AssembleBullet();
        FireMyGun.Invoke();
    }

    public void FireMyBullet()
    {
    
    }
}
