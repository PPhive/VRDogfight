using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowCubeUpdate : MonoBehaviour
{
    public GlowCube MyGlowCube;
    void Start()
    {
        MyGlowCube = transform.GetComponent<GlowCube>();
    }

    void Update()
    {
        MyGlowCube.Rescale();
    }
}
