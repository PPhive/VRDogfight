using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipColorAutoPicker : MonoBehaviour
{
    Unit myUnit;
    [SerializeField]
    List<Mesh> AllMeshes;

    void Start()
    {
        myUnit = GameManager.instance.CheckMyUnit(gameObject).GetComponent<Unit>();
        if (myUnit.myPlayer != null && AllMeshes.Count > 0) 
        {
            MeshFilter MyFilter = GetComponent<MeshFilter>();
            if (AllMeshes.Count >= myUnit.myPlayer.myTeam.index) 
            {
                MyFilter.mesh = AllMeshes[myUnit.myPlayer.myTeam.index];
            }
        }
    }
}
