using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelShaker : MonoBehaviour
{
    [SerializeField]
    GameObject MyModel;
    [SerializeField]
    Vector3 MyEulers;
    [SerializeField]
    Vector3 MyInertia;
    [SerializeField]
    float Spring = 100;
    [SerializeField]
    float Decay = 10;


    void Start()
    {
        if (MyModel == null) 
        {
            MyModel = gameObject;
        }
    }

    void Update()
    {
        if (MyEulers.magnitude != 0 || MyInertia.magnitude != 0) 
        {
            MyEulers += MyInertia * Time.deltaTime;
            MyInertia += (-MyEulers) * Time.deltaTime * Spring;
            MyInertia -= (MyInertia * Time.deltaTime * Decay);
            MyModel.transform.localEulerAngles = MyEulers;
            if (Mathf.Abs(MyInertia.magnitude) + Mathf.Abs(MyEulers.magnitude) < 0.1)
            {
                MyInertia *= 0;
                MyEulers *= 0;
            }
        }
    }

    public void AddInertia(Vector3 Position, Vector3 Strength) 
    {
        Vector3 CrossProduct = Vector3.Cross(Position - transform.position, Strength);
        CrossProduct = Mathf.Clamp(CrossProduct.magnitude, 0, 300) * CrossProduct.normalized;
        MyInertia += CrossProduct;
    }
}
