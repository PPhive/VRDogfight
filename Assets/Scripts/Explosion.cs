using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private bool Small;
    [SerializeField]
    private float timer = 0;
    [SerializeField]
    private GameObject Particle;
    public float MaxTime;

    //has an explosion shrunk to 0 yet
    private bool Finished = false;

    private float TimeScale = 1;

    void Start()
    {
        if (!Small)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject ThisParticle = Instantiate(Particle, transform.position, transform.rotation);
                TimeScale = Random.Range(0.8f, 1.2f);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject ThisParticle = Instantiate(Particle, transform.position, Random.rotation, transform);
        }
    }
    void Update()
    {
        if (!Finished) 
        {
            float SizeMultiplier = 3 * Mathf.Sin((0.3f + timer) * 3.1415f);
            if (SizeMultiplier <= 0)
            {
                transform.localScale *= 0;
                Finished = true;
            }
            transform.localScale = new Vector3(1, 1, 1) * SizeMultiplier;
        }
        timer += Time.deltaTime * TimeScale;
        if (timer > MaxTime) 
        {
            Destroy(gameObject);
        }
    }
}
