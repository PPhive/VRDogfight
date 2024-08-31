using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_MultiBarrel : Gun
{
    [System.Serializable]
    public class BarrelClass 
    {
        public GameObject muzzle;
        public GameObject barrelOffset;
        public ParticleSystem gunBlaze;
    }

    [SerializeField]
    List<BarrelClass> barrels;
    [SerializeField]
    int barrelIndex;

    public override void GunUpdate() 
    {
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        foreach (BarrelClass barrel in barrels) 
        {
            BarrelOffset = barrel.barrelOffset;
            MuzzleRecoil();
        }
    }

    public override void FireAttempted()
    {
        if (Cooldown <= 0)
        {
            for (int i = 0; i < Burst; i++)
            {
                if (barrelIndex >= barrels.Count - 1)//roll through every barrels;
                {
                    barrelIndex = 0;
                }
                else 
                {
                    barrelIndex++;
                }
                StartCoroutine(Fire(i * BurstCD, MyBullet, barrels[barrelIndex].muzzle, barrels[barrelIndex].barrelOffset, barrels[barrelIndex].gunBlaze));
            }
            Cooldown = CooldownMax;
        }
    }
}
