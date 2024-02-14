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

    private void FixedUpdate()
    {
        SphereCastAdvance();
    }

    void Disappear() 
    {
        Explode();
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
        }

        if (other.gameObject.tag == Owner.gameObject.tag)
        {
            Debug.Log("Hitting target with same tag!");
            transform.parent = other.transform;
        }
        else
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

    //To prevent overpen, bullet uses this method to ensure a hit.
    void SphereCastAdvance() 
    {
        RaycastHit Hit;
        if (Physics.SphereCast(transform.position, 0.3f, transform.TransformDirection(Vector3.forward), out Hit, MyRb.velocity.magnitude * Time.fixedDeltaTime))
        {
            if (Hit.transform.gameObject.tag != "Bullet")
            {
                MyRb.velocity = MyRb.velocity.normalized;
                transform.position += Hit.distance * transform.TransformDirection(Vector3.forward);
                OnTriggerEnter(Hit.collider);
                Debug.Log("Hitting " + Hit.transform.gameObject);
            }
        }
    }

    public void Explode() 
    {
        GameObject ThisExplosion = Instantiate(Explosion, transform.position, transform.rotation);
        if (transform.parent != null) 
        {
            Transform Parent = transform.parent;
            Vector3 MyLocalScale = ThisExplosion.transform.localScale;
            Vector3 ParentScale = Parent.transform.lossyScale;
            MyLocalScale = new Vector3(MyLocalScale.x / ParentScale.x, MyLocalScale.y / ParentScale.y, MyLocalScale.z / ParentScale.z);
        }


    }
}
