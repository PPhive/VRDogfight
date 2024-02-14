using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class Slicer : MonoBehaviour
{
    public Transform PlaneDebug;
    public GameObject Target;
    public Material CrossSectionMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            Slice(Target);
        }
    }

    public void Slice(GameObject target) 
    {
        SlicedHull hull = target.Slice(PlaneDebug.position, PlaneDebug.up);

        if (hull != null) 
        {
            GameObject upperhull = hull.CreateUpperHull(target, CrossSectionMaterial);
            GameObject lowerhull = hull.CreateLowerHull(target, CrossSectionMaterial);
            Destroy(target);
        }
    }
}
