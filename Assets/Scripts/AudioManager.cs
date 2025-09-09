using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get { return instance; }
    }
    
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource musicSource;
    public AudioSource bgm;

    private void Awake()
    {
        instance = this;
    }

    public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, clips.Length );
        AudioClip clip = clips[index];
        musicSource.clip = clip;
        musicSource.pitch = pitch;
        musicSource.Play();

    }

    public void stopMusic()
    {
        bgm.Stop();
    }
}
