using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Unit Owner;
    public GameObject Explosion;
    public Projectile MyProjectile;
    public Rigidbody MyRb;
    private float Timer;

    void Start()
    {
        transform.localScale = new Vector3(1,1,1) * MyProjectile.size;
        MyRb = GetComponent<Rigidbody>();

        MyRb.mass = MyProjectile.mass;
        MyRb.AddForce(transform.forward * MyProjectile.initalforce, ForceMode.Impulse);
        Timer = MyProjectile.lifeTime;
    }

    void Update()
    {
        transform.LookAt(MyRb.velocity + transform.position);
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else 
        {
            Disappear();
        }
    }

    void Disappear() 
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit Landed with " + gameObject + ". Dealing " + MyProjectile.damage);
            EnemyBasic MyScript = FindParentScript(other.transform);
            if (MyScript != null)
            {
                MyScript.Hit(MyProjectile.damage);
                Instantiate(Explosion, transform.position, transform.rotation, other.transform);

                //This should be transported to Unit system soon
                if (MyScript.gameObject.GetComponent<LockOnAble>() != null && Owner.MyLockOnReciever != null && MyScript.gameObject.GetComponent<LockOnAble>().isActiveAndEnabled) 
                {
                    LockOnAble ThisTarget = MyScript.gameObject.GetComponent<LockOnAble>();

                    bool AlreadyLockedOn = false;
                    LockOnTargets ThisTargetClass = new LockOnTargets();
                    ThisTargetClass.Target = ThisTarget;
                    foreach (LockOnTargets ExistingTargets in Owner.MyLockOnReciever.List) 
                    {
                        if (ExistingTargets.Target == ThisTarget)
                        {
                            AlreadyLockedOn = true;
                            ThisTargetClass = ExistingTargets;
                        }
                    }

                    if (AlreadyLockedOn)
                    {
                        Owner.MyLockOnReciever.AddLockOnProgress(ThisTargetClass, 34f);
                    }
                    else
                    {
                        ThisTarget.List.Add(Owner.MyLockOnReciever);
                        Owner.MyLockOnReciever.List.Add(ThisTargetClass);
                        Owner.MyLockOnReciever.AddLockOnProgress(ThisTargetClass, 34f);
                        Owner.MyLockOnReciever.CreateLockOnRing(ThisTargetClass);
                    }
                }

            }
            if (GetComponent<Collider>() != null) 
            {
                GetComponent<Collider>().enabled = false;
            }
            Disappear();
        }

        if (other.gameObject.tag != Owner.gameObject.tag)
        {
            Debug.Log("Hitting target with same tag!");
        }
        else if (other.gameObject.tag != "Bullet") 
        {
            Disappear();
        }
    }

    EnemyBasic FindParentScript(Transform other) 
    {
        if (other.GetComponent<EnemyBasic>() != null)
        {
            return other.GetComponent<EnemyBasic>();
        }
        else if (other.transform.parent != null)
        {
            return FindParentScript(other.parent);
        }
        return null;
    }
}
