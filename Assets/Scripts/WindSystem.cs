using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSystem : MonoBehaviour
{
    [SerializeField]
    Player player;
    public Rigidbody MyRB;
    [SerializeField]
    ParticleSystem MyEmitter;

    [SerializeField]
    Vector3 LastVelocity;
    [SerializeField]
    Vector3 Velocity;

    [SerializeField]
    Vector3 EulerOld;
    [SerializeField]
    Vector3 EulerNew;

    private void Start()
    {
        
    }

    private void Update()
    {
        float Speed = MyRB.velocity.magnitude;
        Transform Parent = MyRB.transform;
        Vector3 Difference = EulerNew - EulerOld;
        transform.localEulerAngles = Difference * 45 / Speed / 0.01f;
        MyEmitter.transform.localEulerAngles = Difference * 45 / Speed / 0.01f;
        //MyEmitter.transform.localPosition = new Vector3 (Difference.y, Difference.x, 0)  * 45 / Speed / 0.05f + Vector3.forward * 45;
    }

    IEnumerator UpdateAngle() 
    {
        while (true)
        {
            Transform Parent = MyRB.transform;
            EulerOld = EulerNew;
            EulerNew = Parent.eulerAngles;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
