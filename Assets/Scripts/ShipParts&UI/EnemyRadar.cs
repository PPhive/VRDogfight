using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    public Unit myUnit;
    [SerializeField]
    GameObject MyCone;
    [SerializeField]
    GameObject MyText;

    void Start()
    {
        //should probably add a way to find unit;
        if (myUnit.AI) 
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Unit closestEnemyUnit = null;

        if (GameManager.instance.CurrentGame != null) 
        {
            foreach (Team thisTeam in GameManager.instance.CurrentGame.teams)
            {
                if (thisTeam != GameManager.instance.CurrentGame.teams[0] && !thisTeam.myPlayers.Contains(myUnit.myPlayer)) 
                {
                    foreach (Player thisPlayer in thisTeam.myPlayers) 
                    {
                        if (thisPlayer.myCurrentShip != null) 
                        {
                            if (closestEnemyUnit == null || Vector3.Distance(thisPlayer.transform.position,transform.position) < Vector3.Distance(closestEnemyUnit.transform.position, transform.position)) 
                            {
                                closestEnemyUnit = thisPlayer.myShipUnit;
                            }
                        }
                    }
                }
            }
        }

        if (closestEnemyUnit != null)
        {
            MyCone.SetActive(true);
            MyCone.transform.LookAt(closestEnemyUnit.transform);
            Transform myConeTip = MyCone.transform.GetChild(0);
            myConeTip.localPosition = (Mathf.Sin(Time.timeSinceLevelLoad * 6.28f) * 0.05f - 0.2f) * Vector3.forward;

            MyText.SetActive(true);
            if (myUnit.myPlayer != null) 
            {
                GameObject mainCamera = myUnit.myPlayer.transform.GetChild(0).GetChild(0).gameObject;
                Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.forward;
                Vector3 upDirection = mainCamera.transform.rotation * Vector3.up;
                MyText.transform.LookAt(targetPosition, upDirection);
            }
        }
        else 
        {
            MyCone.SetActive(false);
            MyText.SetActive(false);
        }
    }
}
