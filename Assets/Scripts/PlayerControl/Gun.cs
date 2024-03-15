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
    }
    [SerializeField]
    public Slot MySlot;

    [SerializeField]
    KeyCode FireButton = KeyCode.Space;
    [SerializeField]
    Unit Owner;
    [SerializeField]
    GameObject MyBullet;
    [SerializeField]
    GameObject MyMuzzle;
    [SerializeField]
    GameObject BarrelOffset;
    [SerializeField]
    ParticleSystem GunBlaze;
    [SerializeField]
    AudioSource MySound;
    [SerializeField]
    float Spread;
    [SerializeField]
    int Burst = 1;
    [SerializeField]
    float BurstCD = 0;

    [SerializeField]
    private float CooldownMax;
    [SerializeField]
    private float Cooldown;

    private IEnumerator Coroutine;

    void Start()
    {
        //Check which unit is my owner by looking into every parent
        Unit CheckingOwner = CheckOwner(gameObject).GetComponent<Unit>();
        Owner = CheckingOwner;

        //Checking if weapon is one left or right and determine control based on that
        if (Owner != null) 
        {
            Vector2 a = new Vector2(transform.position.x, transform.position.z);
            Vector2 b = new Vector2(Owner.transform.position.x, Owner.transform.position.z);
            Vector2 c = b + new Vector2(Owner.transform.forward.x, Owner.transform.forward.z);
            float CrossProduct = Vector3.Cross(a - b, c - b).z;
            if (CrossProduct > 0)
            {
                MySlot = Slot.Right;
            }
            else if (CrossProduct < 0)
            {
                MySlot = Slot.Left;
            }
            else
            {
                MySlot = Slot.Mid;
            }
        }

        //Add me to Owner WeaponList
        if (!Owner.MyWeapons.Contains(this)) 
        {
            Owner.MyWeapons.Add(this);
        }
    }

    void Update()
    {
        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        MuzzleRecoil();
    }

    public IEnumerator Fire(float delay) 
    {
        yield return new WaitForSeconds(delay);
        MyMuzzle.transform.localEulerAngles = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), 0);
        GameObject SpawnedBullet = Instantiate(MyBullet, MyMuzzle.transform.position, MyMuzzle.transform.rotation);
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
        BarrelOffset.transform.localPosition = -Vector3.forward * 1;
        GunBlaze.Play();
        MySound.PlayOneShot(MySound.clip);
    }

    public void MuzzleRecoil() 
    {
        if (BarrelOffset.transform.localPosition.z < 0) 
        {
            BarrelOffset.transform.localPosition += Vector3.forward * 10 * Time.deltaTime;
        }
    }

    GameObject CheckOwner(GameObject Checking) 
    {
        if (Checking.GetComponent<Unit>() != null)
        {
            return Checking.gameObject;
        }
        else if (Checking.transform.parent != null)
        {
            return CheckOwner(CheckOwner(Checking.transform.parent.gameObject));
        }
        return null;
    }

    public void FireAttempted() 
    {
        if (Cooldown <= 0)
        {
            for (int i = 0; i < Burst; i++)
            {
                StartCoroutine(Fire(i * BurstCD));
            }
            Cooldown = CooldownMax;
        }
    }
}
