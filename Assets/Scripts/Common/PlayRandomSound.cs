using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField]
    AudioSource MyAudioSource;
    [SerializeField]
    bool PlayOnAwake = true;
    [SerializeField]
    bool PlayRandomOnAwake = true;
    [SerializeField]
    List<AudioClip> Clips;

    float Delay = 0;
    AudioClip SoundInQueue;
    IEnumerator Coroutine;


    void Start()
    {
        if (MyAudioSource == null && GetComponent<AudioSource>() != null)
        {
            MyAudioSource = GetComponent<AudioSource>();
        }
        else if (MyAudioSource == null) 
        {
            Debug.Log("No Audio source found on " + gameObject);
        }

        if (PlayOnAwake)
        {
            if (Delay > 0) 
            {
                WaitAndPlay(Delay);
            }
            if (PlayRandomOnAwake)
            {
                PlayRandomClip();
            }
            else
            {
                PlayThisClip(0);
            }
        }
    }

    public void PlayThisClip(int ClipNumber)
    {
        if (Clips.Count >= 1)
        {
            if (ClipNumber < Clips.Count)
            {
                MyAudioSource.PlayOneShot(Clips[ClipNumber]);
            }
            else 
            {
                Debug.Log("You tried to play clip " + ClipNumber + " But there's only " + Clips.Count);
            }
        }
        else
        {
            Debug.Log("No Sound in list");
        }
    }

    public void PlayRandomClip()
    {
        PlayThisClip(Random.Range(0, Clips.Count));
    }

    public void PlayAfterDelay(float delay) 
    {
        WaitAndPlay(delay);
    }

    private IEnumerator WaitAndPlay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayRandomClip();
    }

}
