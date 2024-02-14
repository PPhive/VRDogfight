using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int Seed;

    public List<Unit> Players;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
        }
    }

    void Initialize() 
    {
        Seed = GenerateSeed();
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    private int GenerateSeed() 
    {
        int Seed = Random.Range(10000,99999);
        return Seed;
    }

    public float SeededRandomRange(float min, float max, float x, float y) 
    {
        return Mathf.Lerp(min,max, Mathf.PerlinNoise(x * Seed, y * Seed));
    }
}
