using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    Player myPlayer;
    [SerializeField]
    RectTransform myCross;
    [SerializeField]
    RectTransform myRing;
    [SerializeField]
    RectTransform myWire;
    [SerializeField]
    RectTransform myCanvas;

    [SerializeField]
    Camera myCamera;

    void Start()
    {
        if (myPlayer.myShipUnit != null)
        {
            myCamera = myPlayer.myShipUnit.GetComponent<SpaceShipControl>().myCamera.GetComponent<Camera>();
        }
    }
    void Update()
    {
        if (myPlayer.myShipUnit != null) 
        {
            float dotProduct = Vector3.Dot(myPlayer.myShipUnit.transform.forward, myCamera.transform.forward);
            if (dotProduct > 0)
            {
                myCross.gameObject.SetActive(true);
                myWire.gameObject.SetActive(true);
                myWire.position = Vector3.Lerp(myRing.position, myCross.position, 0.5f);
                myWire.localEulerAngles = -Vector3.forward * Mathf.Atan2
                    (
                    myCross.position.x - myRing.position.x,
                    myCross.position.y - myRing.position.y
                    ) / 3.14f * 180f;
                myWire.localScale = 3 * new Vector3(0.02f, Vector2.Distance(myCross.position, myRing.position) / 100 * 5 / myCanvas.localScale.x,1);
            }
            else 
            {
                myCross.gameObject.SetActive(false);
                myWire.gameObject.SetActive(false);
            }

            myCross.position = myCamera.WorldToScreenPoint(myPlayer.myShipUnit.scope.transform.position);
        }
    }
}
