using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Vignette : MonoBehaviour
{
    MeshRenderer myMesh;
    [SerializeField]
    bool Toggle;

    void Start()
    {
        myMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            myMesh.enabled = !myMesh.enabled;
        }
    }
}
