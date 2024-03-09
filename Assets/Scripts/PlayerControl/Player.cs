using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool AI;

    [SerializeField]
    private GameObject myCamera;
    [SerializeField]
    private GameObject myRadar;
    public GameObject myShipPrefab;
    public GameObject myCurrentShip;

    void Start()
    {
        if (!GameManager.instance.Players.Contains(this))
        {
            GameManager.instance.Players.Add(this);
        }

        Mount();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            Mount();
        }


        if (myCurrentShip != null) 
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                myCurrentShip.GetComponent<Unit>().Hit(99999);
            }

            Unit myShipUnit = myCurrentShip.GetComponent<Unit>();
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Mouse0)) 
            {
                myShipUnit.AttemptToFireGuns(Gun.Slot.Left);
            }
            if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Mouse1)) 
            {
                myShipUnit.AttemptToFireGuns(Gun.Slot.Right);
            }
            if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Mouse2))
            {
                myShipUnit.AttemptToFireGuns(Gun.Slot.Mid);
            }
        }
    }
    public void UnMount() 
    {
        transform.parent = null;
        transform.eulerAngles = transform.eulerAngles.y * Vector3.up;
    }

    public void Mount() 
    {
        if (transform.parent == null) 
        {
            myCurrentShip = Instantiate(myShipPrefab, transform.position, transform.rotation, transform.parent);
            myCurrentShip.tag = tag;
            myCurrentShip.GetComponent<LockOnReciever>().Radar = myRadar;
            transform.parent = myCurrentShip.GetComponent<SpaceShipControl>().playerMount.transform;
            myCurrentShip.GetComponent<SpaceShipControl>().myCamera = myCamera;
            transform.localPosition = new Vector3();
            transform.localRotation = new Quaternion();
        }
    }
}
