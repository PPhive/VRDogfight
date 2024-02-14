using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

}

[System.Serializable]
public class Projectile
{
    public float mass;
    public float size;
    public float damage;
    public float blastRadius;
    public float lifeTime;
    public float initalforce;
    public float recoilX;
    public float recoilY;

    public Projectile(float Mass, float Size, float Damage, float BlastRadius, float LifeTime, float InitalForce, float RecoilX, float RecoilY)
    {
        mass = Mass;
        size = Size;
        damage = Damage;
        blastRadius = BlastRadius;
        lifeTime = LifeTime;
        initalforce = InitalForce;
        recoilX = RecoilX;
        recoilY = RecoilY;
    }

    public Projectile()
    {
        mass = 1;
        size = 1;
        damage = 10;
        blastRadius = 0;
        lifeTime = 0;
        initalforce = 100;
        recoilX = 0;
        recoilY = 0;
    }

    public Projectile(Projectile a, Projectile b)
    {
        damage = a.damage + b.damage;
        blastRadius = a.blastRadius + b.blastRadius;
        size = a.size + b.size;
        mass = a.mass + b.mass;
        lifeTime = a.lifeTime + b.lifeTime;
        initalforce = a.initalforce + b.initalforce;
        recoilX = a.recoilX + b.recoilX;
        recoilY = a.recoilY + b.recoilY;
    }
}

