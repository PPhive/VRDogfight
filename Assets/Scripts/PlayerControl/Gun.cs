using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum Slot //Weapons are split into three groups, left right and mid, each has a different firing condition.
    {
        Left,
        Right,
        Mid,
        Sub,
    }
    [SerializeField]
    public Slot MySlot;

    [SerializeField]
    KeyCode FireButton = KeyCode.Space;
    [SerializeField]
    protected Unit Owner;
    [SerializeField]
    protected GameObject MyBullet;
    [SerializeField]
    protected GameObject MyMuzzle;
    [SerializeField]
    protected GameObject BarrelOffset;
    [SerializeField]
    protected float BarrelRecoilDist = 1;
    [SerializeField]
    protected float BarrelRetract = 0;
    [SerializeField]
    protected ParticleSystem GunBlaze;
    [SerializeField]
    protected AudioSource MySound;
    [SerializeField]
    protected float Spread;
    [SerializeField]
    protected int Burst = 1;
    [SerializeField]
    protected float BurstCD = 0;

    [SerializeField]
    protected float CooldownMax;
    [SerializeField]
    protected float Cooldown;

    void Start()
    {
        //Check which unit is my owner by looking into every parent
        Unit CheckingOwner = GameManager.instance.CheckMyUnit(gameObject).GetComponent<Unit>();
        Owner = CheckingOwner;

        DetermineSlot();

        //Add me to Owner WeaponList
        if (!Owner.MyWeapons.Contains(this)) 
        {
            Owner.MyWeapons.Add(this);
        }
    }

    void Update()
    {
        GunUpdate();
    }

    public virtual void GunUpdate() //Call this method per frame in update
    {
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        MuzzleRecoil();
    }

    public IEnumerator Fire(float delay, GameObject bullet, GameObject muzzle,GameObject barrelOffset, ParticleSystem blaze
        ) 
    {
        yield return new WaitForSeconds(delay);
        muzzle.transform.localEulerAngles = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), 0);
        GameObject SpawnedBullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
        SpawnedBullet.tag = tag;
        if (SpawnedBullet.GetComponent<Bullet>() != null)
        {
            if (Owner.myPlayer != null)
            {
                SpawnedBullet.GetComponent<Bullet>().Owner = Owner.myPlayer;
            }
            else 
            {
                SpawnedBullet.GetComponent<Bullet>().Owner = GameManager.instance.CurrentGame.teams[0].myPlayers[0];//This is the neutral player
            }
        }
        barrelOffset.transform.localPosition -= Vector3.forward * BarrelRecoilDist;
        Debug.Log(Vector3.forward * BarrelRecoilDist);
        blaze.Play();
        MySound.PlayOneShot(MySound.clip);
        yield return null;
    }

    public void MuzzleRecoil() 
    {
        if (BarrelOffset.transform.localPosition.z < 0) 
        {
            BarrelOffset.transform.localPosition += Vector3.forward * BarrelRetract * Time.deltaTime;
            if (false && BarrelOffset.transform.localPosition.z > 0)
            {
                Debug.Log(BarrelOffset.transform.localPosition + " " + Vector3.forward * BarrelOffset.transform.localPosition.z);
                BarrelOffset.transform.localPosition -= Vector3.forward * BarrelOffset.transform.localPosition.z;
                Debug.Log(BarrelOffset.transform.localPosition);
            }
        }
    }

    public virtual void FireAttempted() 
    {
        if (Cooldown <= 0)
        {
            for (int i = 0; i < Burst; i++)
            {
                StartCoroutine(Fire(i * BurstCD,MyBullet,MyMuzzle,BarrelOffset,GunBlaze));
            }
            Cooldown = CooldownMax;
        }
    }

    public void DetermineSlot() 
    {
        //Checking if weapon is one left or right and determine control based on that
        if (Owner != null)
        {
            if (transform.parent.name == "WeaponMountRight")
            {
                MySlot = Slot.Right;
                Debug.Log(transform.parent.name);
            }
            else if (transform.parent.name == "WeaponMountLeft")
            {
                MySlot = Slot.Left;
            }
            else if (transform.parent.name == "WeaponMountMid")
            {
                MySlot = Slot.Mid;
            }
            else
            {
                MySlot = Slot.Sub;
            }
        }
    }
}
