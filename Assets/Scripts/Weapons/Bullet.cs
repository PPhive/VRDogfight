using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player Owner;
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
        tag = "Bullet";
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
        if (other.gameObject.tag == "Bullet")
        {
            //Debug.Log("hitting self");
        }
        else 
        {
            if (other.gameObject.tag != Owner.tag && other.gameObject.tag != "default")
            {
                //Debug.Log("Hit Landed with " + gameObject + ". Dealing " + MyProjectile.damage);
                Unit TargetUnit = FindParentUnit(other.transform);
                if (TargetUnit != null)
                {
                    if (TargetUnit.gameObject.GetComponent<LockOnAble>() != null && Owner.myShipUnit.myLockOnReciever != null && TargetUnit.gameObject.GetComponent<LockOnAble>().isActiveAndEnabled)
                    {
                        LockOnAble ThisTarget = TargetUnit.gameObject.GetComponent<LockOnAble>();
                        bool AlreadyLockedOn = false;
                        ThisTargetClass.Target = ThisTarget;
                        for (int i = 0; i < Owner.myShipUnit.myLockOnReciever.List.Count; i++)
                        {
                            if (Owner.myShipUnit.myLockOnReciever.List[i].Target == ThisTarget)
                            {
                                AlreadyLockedOn = true;
                                ThisTargetClass = Owner.myShipUnit.myLockOnReciever.List[i];
                                if (ThisTargetClass.LockOnProgress >= 100)
                                {
                                    MyProjectile.damage *= 1.5f;
                                }
                            }
                        }

                        if (AlreadyLockedOn)
                        {
                            Owner.myShipUnit.myLockOnReciever.AddLockOnProgress(ThisTargetClass, LockOnDamage);
                        }
                        else
                        {
                            ThisTarget.List.Add(Owner.myShipUnit.myLockOnReciever);
                            Owner.myShipUnit.myLockOnReciever.List.Add(ThisTargetClass);
                            Owner.myShipUnit.myLockOnReciever.CreateLockOnRing(ThisTargetClass);
                            Owner.myShipUnit.myLockOnReciever.AddLockOnProgress(ThisTargetClass, LockOnDamage);
                        }
                    }
                    TargetUnit.lasthitFrom = Owner;
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

            if (other.attachedRigidbody == null || other.attachedRigidbody.gameObject != Owner.myShipUnit.gameObject)
            {
                Disappear();
            }
        } 
    }

    public static Unit FindParentUnit(Transform other) 
    {
        if (other.GetComponent<Unit>() != null)
        {
            return other.GetComponent<Unit>();
        }
        else if (other.transform.parent != null)
        {
            return FindParentUnit(other.parent);
        }
        return null;
    }

    //To prevent overpenetration, bullet uses this method to ensure a hit.
    void SphereCastAdvance() 
    {
        RaycastHit[] HitsArray = Physics.SphereCastAll(transform.position, transform.lossyScale.magnitude, transform.TransformDirection(Vector3.forward), MyRb.velocity.magnitude * Time.fixedDeltaTime);
        if (HitsArray.Length > 0)
        {
            //covert raycast hit array into list to use the builtin sorting for lists.
            List<RaycastHit> Hits = new List<RaycastHit>();
            for (int i = 0; i < HitsArray.Length; i++)
            {
                Hits.Add(HitsArray[i]);
            }
            //because spherecast all returns a non-ordered array of hits we have to order them by their distance
            Hits.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            for (int i = 0; i < Hits.Count; i++) 
            {
                RaycastHit Hit = Hits[i];
                if (Hit.transform.gameObject.tag != Owner.tag && Hit.transform.gameObject.tag != "Bullet")
                {
                    MyRb.velocity = MyRb.velocity.normalized;

                    try
                    {
                        if (Hit.collider != null)
                        {
                            transform.position += Hit.distance * transform.TransformDirection(Vector3.forward);
                            OnTriggerEnter(Hit.collider);
                        }
                    }
                    catch 
                    {
                        Debug.Log("Bullet glitch ignored");
                    }
                    break;
                }
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
