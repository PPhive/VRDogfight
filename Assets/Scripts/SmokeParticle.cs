using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticle : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
    private bool Exploded = false;
    [SerializeField]
    private float Timer = 1;
    private float Fall = 0;
    private float Burst = 40;
    void Start()
    {
        transform.rotation = Random.rotation;
        //transform.eulerAngles = new Vector3 (Mathf.Abs(transform.eulerAngles.x), transform.eulerAngles.y, 0);
        transform.eulerAngles = new Vector3(-Random.Range(-30f,90f), transform.eulerAngles.y, 0);
        Burst *= Random.Range(0.8f, 3f);
    }
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < Random.Range(1.5f, 2f) && !Exploded)
        {
            if (Random.Range(0f, 1f) > 0.9f) 
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            Exploded = true;
        }
        if (Timer < 0)
        {
            Destroy(gameObject);
        }

        Fall += 20f * Time.deltaTime;
        Burst -= 20f * Time.deltaTime;
        transform.position += transform.forward * Burst * Time.deltaTime;
        transform.position -= new Vector3(0, Time.deltaTime * Fall, 0);
    }
}
