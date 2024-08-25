using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    Player myPlayer;
    [SerializeField]
    Transform myCross;
    [SerializeField]
    Transform myRing;
    [SerializeField]
    Transform myWire;

    [SerializeField]
    Camera myCamera;

    void Start()
    {
    }
    void Update()
    {
        if (myPlayer.myShipUnit != null) 
        {
            myCross = myPlayer.myShipUnit.scope.transform;
            float dotProduct = Vector3.Dot(myPlayer.myShipUnit.transform.forward, myCamera.transform.forward);
            if (dotProduct > 0)
            {
                myWire.gameObject.SetActive(true);

                /*
                myWire.position = Vector3.Lerp(myRing.position, myCross.position, 0.5f);
                myWire.localEulerAngles = -Vector3.forward * Mathf.Atan2
                    (
                    myCross.position.x - myRing.position.x,
                    myCross.position.y - myRing.position.y
                    ) / 3.14f * 180f;
                myWire.localScale = 3 * new Vector3(0.02f, Vector2.Distance(myCross.position, myRing.position) / 100 * 5 / myCanvas.localScale.x,1);
                */
                myWire.LookAt(myCross);
                myWire.transform.localScale = new Vector3(1f,1f,Vector3.Distance(myRing.position, myCross.position) / transform.lossyScale.z);
            }
            else 
            {
                myWire.gameObject.SetActive(false);
            }
        }
    }
}
