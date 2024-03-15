using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager.Team myTeam;
    public bool AI;
    public bool TeamPlaceHolder;

    [SerializeField]
    private GameObject myCamera;
    [SerializeField]
    private GameObject myRadar;
    public GameObject myShipPrefab;
    public GameObject myCurrentShip;
    public Unit myShipUnit;

    [SerializeField]
    UnityEngine.InputSystem.XR.TrackedPoseDriver HeadInput;

    void Start()
    {
        if (!GameManager.instance.Players.Contains(this))
        {
            GameManager.instance.Players.Add(this);
        }

        if (tag != "Neutral") 
        {
            Mount();
        }
    }

    void Update()
    {
        if (AI) //AI Player
        {

        }
        else //Real Player
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
        }
        else 
        {
            myCurrentShip = CheckTopParent(gameObject);
        }
        myCurrentShip.name = myCurrentShip.name + "-" + gameObject.name;
        myShipUnit = myCurrentShip.GetComponent<Unit>();
        myShipUnit.myPlayer = this;
        if (AI)
        {
            myCurrentShip.GetComponent<SpaceShipControl>().enabled = false;
            myCurrentShip.GetComponent<EnemyBasic>().enabled = true;
            myShipUnit.scope.SetActive(false);
            myShipUnit.GetComponent<Telemetry>().enabled = false;
            myShipUnit.GetComponent<SimRacingStudio>().enabled = false;
        }
        else
        {
            SpaceShipControl MyController = myCurrentShip.GetComponent<SpaceShipControl>();
            MyController.enabled = true;
            myCurrentShip.GetComponent<EnemyBasic>().enabled = false;
            myShipUnit.scope.SetActive(true);
            MyController.myCamera = myCamera;
            myCurrentShip.GetComponent<LockOnReciever>().Radar = myRadar;
            MyController.HeadInput = HeadInput;
            myShipUnit.GetComponent<Telemetry>().enabled = true;
            myShipUnit.GetComponent<SimRacingStudio>().enabled = true;
        }
        myCurrentShip.tag = tag;
        transform.parent = myCurrentShip.GetComponent<Unit>().mount.transform;
        transform.localPosition = new Vector3();
        transform.localRotation = new Quaternion();
    }

    GameObject CheckTopParent(GameObject Checking)
    {
        if (Checking.transform.parent != null)
        {
            return CheckTopParent(CheckTopParent(Checking.transform.parent.gameObject));
        }
        else 
        {
            return Checking;
        }
    }
}
