using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowCube : MonoBehaviour
{
    //This script is applied to glowcubes to adjust their internal cube scales to make the edges have constant size;
    [SerializeField]
    GameObject CubeGlow;
    [SerializeField]
    GameObject CubeX;
    [SerializeField]
    GameObject CubeY;
    [SerializeField]
    GameObject CubeZ;
    [SerializeField]
    float EdgeWidth = 0.1f;
    void Start()
    {
        Rescale();
    }

    public void Rescale() 
    {
        CubeGlow.transform.parent = null;
        CubeGlow.transform.localScale = transform.lossyScale - new Vector3(1, 1, 1) * EdgeWidth / 4;
        CubeGlow.transform.parent = transform;

        CubeX.transform.parent = null;
        CubeX.transform.localScale = transform.lossyScale - new Vector3(0, 1, 1) * EdgeWidth;
        CubeX.transform.parent = transform;

        CubeY.transform.parent = null;
        CubeY.transform.localScale = transform.lossyScale - new Vector3(1, 0, 1) * EdgeWidth;
        CubeY.transform.parent = transform;

        CubeZ.transform.parent = null;
        CubeZ.transform.localScale = transform.lossyScale - new Vector3(1, 1, 0) * EdgeWidth;
        CubeZ.transform.parent = transform;
    }
}
