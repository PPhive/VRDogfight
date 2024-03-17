using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;

public class TitleScene : MonoBehaviour
{

    public Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            XROrigin xrorigin = GetComponentInChildren<XROrigin>();
            //xrorigin.MoveCameraToWorldLocation(myShipUnit.mount.transform.position);
            xrorigin.transform.localPosition = new Vector3();
            myCamera.transform.parent.localPosition = myCamera.transform.localPosition * (-1);
            xrorigin.MatchOriginUpCameraForward(Vector3.up, Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) 
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
