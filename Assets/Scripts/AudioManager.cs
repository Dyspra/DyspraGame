using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicV, sfxV;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PlayMusic("1");
    }

    public void PlayMaster(string name)
    {
        PlayMusic(name);
        PlaySFX(name);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicV, x => x.name == name);

        if (s == null) {
            Debug.Log("No Sound");
        }
        else {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxV, x => x.name == name);

        if (s == null)
            Debug.Log("No Sound");
        else
            sfxSource.PlayOneShot(s.clip);
    }

    public void MasterVolume(float v)
    {
        MusicVolume(v);
        SFXVolume(v);
    }

    public void MusicVolume(float v)
    {
        musicSource.volume = v;
    }

    public void SFXVolume(float v)
    {
        sfxSource.volume = v;
    }
}
