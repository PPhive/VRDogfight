using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    public Unit myUnit;
    [SerializeField]
    GameObject MyCone;

    void Start()
    {
        //should probably add a way to find unit;
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
            myConeTip.localPosition = (Mathf.Sin(Time.timeSinceLevelLoad * 6.28f) * 0.05f + 0.25f) * Vector3.forward;
        }
        else 
        {
            MyCone.SetActive(false);
        }
    }
}
