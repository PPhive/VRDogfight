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

    private float TimeScale;

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
    }
    void Update()
    {
        transform.localScale = new Vector3(1, 1, 1) * 3 * Mathf.Sin(timer * 3.1415f);
        timer += Time.deltaTime * TimeScale;
        if (timer > 0.8) 
        {
            Destroy(gameObject);
        }
    }
}
