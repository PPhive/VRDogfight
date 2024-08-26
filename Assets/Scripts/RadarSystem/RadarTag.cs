using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadarTag : MonoBehaviour
{
    [SerializeField]
    Player myPlayer;
    [SerializeField]
    Unit myUnit;
    [SerializeField]
    TextMeshPro distanceText;
    [SerializeField]
    GameObject mySprite;
    [SerializeField]
    GameObject myArrow;
    [SerializeField]
    SpriteRenderer myBox;
    [SerializeField]
    float blinkTimer;

    void Update()
    {
        Unit closestEnemyUnit = null;

        if (GameManager.instance.CurrentGame != null)
        {
            foreach (Team thisTeam in GameManager.instance.CurrentGame.teams)
            {
                if (thisTeam != GameManager.instance.CurrentGame.teams[0] && !thisTeam.myPlayers.Contains(myPlayer))
                {
                    foreach (Player thisPlayer in thisTeam.myPlayers)
                    {
                        if (thisPlayer.myCurrentShip != null)
                        {
                            if (closestEnemyUnit == null || Vector3.Distance(thisPlayer.transform.position, transform.position) < Vector3.Distance(closestEnemyUnit.transform.position, transform.position))
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
            //use dot product to determine if the angle is too big, if so, use arrow on screen
            transform.LookAt(closestEnemyUnit.transform);
            float dot = Vector3.Dot(transform.forward, transform.parent.forward);
            if (dot > 0.94f)
            {
                myBox.enabled = true;
                myArrow.SetActive(false);

                //Always point up
                transform.localEulerAngles -= new Vector3(0, 0, transform.localEulerAngles.z);
            }
            else 
            {
                myBox.enabled = false;
                myArrow.SetActive(true);

                // Make arrow move toward the enemy
                transform.localEulerAngles = new Vector3();
                Transform myParent = transform.parent;
                transform.parent = null;
                Vector3 directionToEnemy = closestEnemyUnit.transform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToEnemy.normalized, 0.35f, 0); // 0.524 radians is approximately 30 degrees
                transform.rotation = Quaternion.LookRotation(newDirection);
                transform.parent = myParent;

                //Always point up
                transform.localEulerAngles -= new Vector3(0, 0, transform.localEulerAngles.z);

                myArrow.transform.LookAt(myParent.transform.position + myParent.transform.forward * 12);
            }
            
            float distance = Mathf.Round(Vector3.Distance(closestEnemyUnit.transform.position,transform.position));
            distanceText.text = distance + "";




            
            if (blinkTimer > 0f)
            {
                blinkTimer -= Time.deltaTime;
            }
            else 
            {
                if (mySprite.activeInHierarchy)
                {
                    mySprite.SetActive(false);
                }
                else 
                {
                    mySprite.SetActive(true);
                }
                blinkTimer += 0.5f;
            }
            
        }
        else 
        {
            distanceText.text = null;
            mySprite.SetActive(false);
        }
    }
}
