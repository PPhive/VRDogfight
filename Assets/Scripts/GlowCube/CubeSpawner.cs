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
    public int ChunkSize = 30;
    private Vector2 PlayerChunk;
    void Start()
    {
        SpawnCubes(GameManager.instance.WorldRadius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCubes(float radius)
    {
        int RadiusbyChunk =  (int)(radius / ChunkSize);
        Vector3 SpawnerNode;
        for (int x = -RadiusbyChunk; x < RadiusbyChunk; x++) 
        {
            for (int z = -RadiusbyChunk; z < RadiusbyChunk; z++) 
            {
                if (!(new Vector2(x * ChunkSize, z * ChunkSize).magnitude < radius * 0.3f || new Vector2(x * ChunkSize, z * ChunkSize).magnitude > radius)) 
                {
                    SpawnerNode = new Vector3(x, Random.Range(-20f, 20f), z) * ChunkSize;
                    //what's in this chunk
                    if (Random.Range(0f, 1.0f) > 0.97f)
                    {
                        Instantiate(CubeIsland, SpawnerNode, transform.rotation, transform);
                    }
                    else if (Random.Range(0f, 1.0f) < 0.002f && false) //added false to pause enemyspawn
                    {
                        GameObject ThisEnemy = Instantiate(EnemyBasic, SpawnerNode, transform.rotation);
                        ThisEnemy.transform.eulerAngles = new Vector3(Random.Range(-2f, 2f), Random.Range(0f,360f), Random.Range(-10f,10f));
                    }
                }
            }
        }
    }
}
