using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Team myTeam;
    public bool AI;
    public bool TeamPlaceHolder;

    public float RespawnTimer = 0;

    [SerializeField]
    private GameObject myCamera;
    [SerializeField]
    private GameObject myRadar;
    public GameObject myShipPrefab;
    public GameObject myCurrentShip;
    public Unit myShipUnit;

    [SerializeField]
    GameObject recenterProbe;

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
            if (!AI)
            {
                StartCoroutine(RecenterAfterOneFrame());
                Debug.Log("recentered");
            }
        }
    }

    void Update()
    {
        if (tag != "Neutral")
        {
            if (myShipUnit == null)
            {
                if (RespawnTimer > 0)
                {
                    RespawnTimer -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Mount Attempted");
                    float randomAngle = Random.Range(0f, 6.28f);
                    Vector3 UnitCircle = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle));
                    transform.position = GameManager.instance.WorldRadius * 1.2f * UnitCircle;
                    transform.LookAt(new Vector3());
                    Mount();
                }
            }

            if (AI) //AI Player
            {

            }
            else //Real Player
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Mount();
                    myShipUnit.keyBoardControlOn = true;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine(RecenterAfterOneFrame());
                }

                if (myCurrentShip != null)
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        myShipUnit.lasthitFrom = this;
                        myShipUnit.Hit(99999);
                    }

                    myShipUnit = myCurrentShip.GetComponent<Unit>();
                    if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Mouse0) || Input.GetButton("Fire1"))
                    {
                        myShipUnit.AttemptToFireGuns(Gun.Slot.Left);
                    }
                    if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Mouse1) || Input.GetButton("Fire2"))
                    {
                        myShipUnit.AttemptToFireGuns(Gun.Slot.Right);
                    }
                    if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Mouse2) || Input.GetButton("Fire3"))
                    {
                        myShipUnit.AttemptToFireGuns(Gun.Slot.Mid);
                    }
                }
            }
        }
    }
    public void UnMount()
    {
        if (tag != "Neutral")
        {
            transform.parent = null;
            transform.eulerAngles = transform.eulerAngles.y * Vector3.up;
            RespawnTimer = 5;
        }
    }

    public void Mount()
    {
        if (tag != "Neutral" && myCurrentShip == null)
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
    }

    public void Recenter()
    {
        Transform cameraOffset = myCamera.transform.parent;
        cameraOffset.localRotation = Quaternion.Inverse(myCamera.transform.localRotation);
        recenterProbe.transform.localPosition = new Vector3();
        recenterProbe.transform.parent = cameraOffset.parent;
        cameraOffset.localPosition = -recenterProbe.transform.localPosition;
        recenterProbe.transform.parent = myCamera.transform;
        Debug.Log("attempted");
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

    IEnumerator RecenterAfterOneFrame()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        Recenter();
        yield break;
    }
}
