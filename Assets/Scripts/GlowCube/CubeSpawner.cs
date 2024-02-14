using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject GlowCube;
    [SerializeField]
    GameObject GlowCubeWindow;
    [SerializeField]
    GameObject CubeIsland;
    [SerializeField]
    GameObject EnemyBasic;
    public int ChunkSize = 20;
    public int RenderChunkDistance = 20;
    private Vector2 PlayerChunk;
    void Start()
    {
        SpawnCubes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCubes()
    {
        Vector3 SpawnerNode;
        for (int x = -50; x < 50; x++) 
        {
            for (int z = -50; z < 50; z++) 
            {
                if (!((x > -2 && x < 2) || (z > -2 && z < 2))) 
                {
                    SpawnerNode = new Vector3(x, Random.Range(-10f, 10f), z) * 50;
                    //what's in this chunk
                    if (Random.Range(0f, 1.0f) > 0.98f)
                    {
                        Instantiate(CubeIsland, SpawnerNode, transform.rotation, transform);
                    }
                    else if (Random.Range(0f, 1.0f) < 0.002f) 
                    {
                        GameObject ThisEnemy = Instantiate(EnemyBasic, SpawnerNode, transform.rotation);
                        ThisEnemy.transform.eulerAngles = new Vector3(Random.Range(-2f, 2f), Random.Range(0f,360f), Random.Range(-10f,10f));
                    }
                }
            }
        }
    }

    void SpawnCubeTree(int Length) 
    {
        //fuc
        float radius = 40;
        float Theta = 0;
        float Phi = 0;
        Vector3 SpawnerNode = new Vector3(Mathf.Sin(Random.Range(0f, 3.14f)), Mathf.Sin(Random.Range(0f, 3.14f)), Mathf.Sin(Random.Range(0f, 3.14f))) * radius;
        float Xspd = Random.Range(1f, 5f);
        float yspd = Random.Range(1f, 5f);
        float zspd = Random.Range(1f, 5f);
        float Size = Random.Range(1, 1);
        for (int i = 0; i < Length; i++) 
        {
            GameObject SpawnedCube = Instantiate(GlowCube, SpawnerNode, transform.rotation);
            SpawnedCube.transform.localScale = Vector3.one * Size;
            float Oldsize = Size;
            Size = Random.Range(5, 10);
            Theta += 6.28f / 10;
            Phi += 3.14f / 40;
            SpawnerNode = new Vector3(
                                        Mathf.Sin(Phi) * Mathf.Cos(Theta * 1.3f),
                                        Mathf.Sin(Phi) * Mathf.Sin(Theta),
                                        Mathf.Cos(Phi)
                                        ) * radius;
            SpawnedCube.transform.LookAt(SpawnerNode);
            radius += 2;
        }
    }
}
