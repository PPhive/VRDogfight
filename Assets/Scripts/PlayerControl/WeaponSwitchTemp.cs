using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchTemp : MonoBehaviour
{
    Unit MyUnit;
    Player MyPlayer;

    public List<GameObject> LeftWeapons;
    public int CurrentLeftWeapon;
    public List<GameObject> RightWeapons;
    public int CurrentRightWeapon;

    void Start()
    {
        try
        {
            MyUnit = GetComponent<Unit>();
            MyPlayer = MyUnit.myPlayer;
        }
        catch 
        {
            Debug.Log("Failed to capture player");
            Destroy(this);
        }

        if (MyPlayer.AI) 
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("L2") || Input.GetKeyDown(KeyCode.Z))
        {
            LeftWeapons[CurrentLeftWeapon].SetActive(false);
            CurrentLeftWeapon++;
            if (CurrentLeftWeapon == LeftWeapons.Count) 
            {
                CurrentLeftWeapon = 0;
            }
            LeftWeapons[CurrentLeftWeapon].SetActive(true);
        }
        if (Input.GetButtonDown("R2") || Input.GetKeyDown(KeyCode.C)) 
        {
            RightWeapons[CurrentRightWeapon].SetActive(false);
            CurrentRightWeapon++;
            if (CurrentRightWeapon == RightWeapons.Count)
            {
                CurrentRightWeapon = 0;
            }
            RightWeapons[CurrentRightWeapon].SetActive(true);
        }
    }
}
