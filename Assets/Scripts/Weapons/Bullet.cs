using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Unit Owner;
    public GameObject Explosion;
    public GameObject ExplosionCrit;
    public bool ExplodeOnTimeOut = false;
    public Projectile MyProjectile;
    [SerializeField]
    private float LockOnDamage = 34;
    public int TargetIndex;
    public Rigidbody MyRb;
    private float Timer;
    [SerializeField]
    LockOnTargets ThisTargetClass;

    void Start()
    {
        transform.localScale = new Vector3(1,1,1) * MyProjectile.size;
        MyRb = GetComponent<Rigidbody>();

        MyRb.mass = MyProjectile.mass;
        MyRb.AddForce(transform.forward * MyProjectile.initalforce, ForceMode.Impulse);
        Timer = MyProjectile.lifeTime;
        SphereCastAdvance();
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
            Destroy(gameObject);
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
        if (other.gameObject.tag != tag && other. gameObject.tag != "default")
        {
            //Debug.Log("Hit Landed with " + gameObject + ". Dealing " + MyProjectile.damage);
            Unit TargetUnit = FindParentScript(other.transform);
            if (TargetUnit != null)
            {
                //This should be transported to Unit system soon
                if (TargetUnit.gameObject.GetComponent<LockOnAble>() != null && Owner.myLockOnReciever != null && TargetUnit.gameObject.GetComponent<LockOnAble>().isActiveAndEnabled) 
                {
                    LockOnAble ThisTarget = TargetUnit.gameObject.GetComponent<LockOnAble>();
                    bool AlreadyLockedOn = false;
                    ThisTargetClass.Target = ThisTarget;
                    for (int i = 0; i < Owner.myLockOnReciever.List.Count; i++) 
                    {
                        if (Owner.myLockOnReciever.List[i].Target == ThisTarget)
                        {
                            AlreadyLockedOn = true;
                            ThisTargetClass = Owner.myLockOnReciever.List[i];
                            if (ThisTargetClass.LockOnProgress >= 100) 
                            {
                                MyProjectile.damage *= 1.5f;
                            }
                        }
                    }

                    if (AlreadyLockedOn)
                    {
                        Owner.myLockOnReciever.AddLockOnProgress(ThisTargetClass, LockOnDamage);
                    }
                    else
                    {
                        ThisTarget.List.Add(Owner.myLockOnReciever);
                        Owner.myLockOnReciever.List.Add(ThisTargetClass);
                        Owner.myLockOnReciever.CreateLockOnRing(ThisTargetClass);
                        Owner.myLockOnReciever.AddLockOnProgress(ThisTargetClass, LockOnDamage);
                    }
                }
                TargetUnit.Hit(MyProjectile.damage);
                if (TargetUnit.GetComponent<ModelShaker>() != null)
                {
                    TargetUnit.GetComponent<ModelShaker>().AddInertia(transform.position, MyRb.velocity.normalized * MyProjectile.damage * 1.5f);
                    Debug.Log("tried to shake!");
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

    Unit FindParentScript(Transform other) 
    {
        if (other.GetComponent<Unit>() != null)
        {
            return other.GetComponent<Unit>();
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
            if (Hit.transform.gameObject.tag != "Bullet" && Hit.transform.gameObject.tag != Owner.gameObject.tag)
            {
                MyRb.velocity = MyRb.velocity.normalized;
                transform.position += Hit.distance * transform.TransformDirection(Vector3.forward);
                Debug.Log("Hitting " + Hit.transform.gameObject);
                OnTriggerEnter(Hit.collider);
            }
        }
    }

    public void Explode() 
    {
        GameObject ThisExplosion;
        if (ThisTargetClass.LockOnProgress >= 100)
        {
            ThisExplosion = Instantiate(ExplosionCrit, transform.position, transform.rotation);
        }
        else 
        {
            ThisExplosion = Instantiate(Explosion, transform.position, transform.rotation);
        }
        if (transform.parent != null) 
        {
            Transform Parent = transform.parent;
            Vector3 MyLocalScale = ThisExplosion.transform.localScale;
            Vector3 ParentScale = Parent.transform.lossyScale;
            MyLocalScale = new Vector3(MyLocalScale.x / ParentScale.x, MyLocalScale.y / ParentScale.y, MyLocalScale.z / ParentScale.z);
        }
    }
}
