using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeIsland : MonoBehaviour
{
    [SerializeField]
    GameObject GlowCube;
    [SerializeField]
    GameObject GlowCubeWindow;
    public float Length = 5;
    public float Size = 70;
    void Start()
    {
        SpawnCubeSpike();
    }

    void SpawnCubeSpike()
    {
        Vector3 SpawnNode = transform.position;
        float Theta = Random.Range(0, 6.28f);
        float HorizontalShift = 0;

        //Spawning the initial cube
        GameObject SpawnedCube = Instantiate(GlowCube, SpawnNode, transform.rotation, transform);
        SpawnedCube.transform.localScale = Size * Vector3.one;

        //Spawning other cubes
        for (int i = 0; i < Length; i++)
        {
            SpawnedCube = Instantiate(GlowCube, transform);
            HorizontalShift = Random.Range(0.4f, 0.6f) * Size;
            Theta = Random.Range(0, 6.28f);
            SpawnNode.x += Mathf.Sin(Theta) * HorizontalShift;
            SpawnNode.z += Mathf.Cos(Theta) * HorizontalShift;
            if (Mathf.Cos(i * 3.14f) > 0)
            {
                SpawnNode.y += Random.Range(0.4f, 0.6f) * Size;
                SpawnedCube.transform.position = SpawnNode;
            }
            else
            {
                SpawnedCube.transform.position = new Vector3 (SpawnNode.x, transform.position.y - (SpawnNode.y - transform.position.y), SpawnNode.z);
            }
            SpawnedCube.transform.localScale = Size * Vector3.one * Random.Range(0.7f, 1.5f);
            if (SpawnedCube.transform.localScale.x > 1.2 && Random.Range(0f, 1f) > 0.5f) 
            {
                GameObject MyWindows = Instantiate(GlowCubeWindow, SpawnedCube.transform);
            }
        }

        //Spawning long cubes
        SpawnNode = transform.position + Vector3.up * 0.9f * Size;
        for (int i = 0; i < Length; i++)
        {
            SpawnedCube = Instantiate(GlowCube, transform);
            HorizontalShift = Random.Range(0.2f, 0.3f) * Size;
            Theta = Random.Range(0, 6.28f);
            SpawnNode.x += Mathf.Sin(Theta) * HorizontalShift;
            SpawnNode.z += Mathf.Cos(Theta) * HorizontalShift;
            if (Mathf.Cos(i * 3.14f) > 0)
            {
                SpawnNode.y += Random.Range(0.7f, 1f) * Size;
                SpawnedCube.transform.position = SpawnNode;
            }
            else
            {
                SpawnedCube.transform.position = new Vector3(SpawnNode.x, transform.position.y - (SpawnNode.y - transform.position.y), SpawnNode.z);
            }

            SpawnedCube.transform.localScale = Size * new Vector3(0.75f,2f,0.75f) * Random.Range(0.7f, 1f);
        }
    }
}
