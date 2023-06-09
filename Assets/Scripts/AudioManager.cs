using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] _sounds;

    private static AudioManager instance;

    public static AudioManager Instance { 
        get => instance; 
        set => instance = value; 
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        foreach(Sound sound in _sounds) {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.Name == name);
        s.Source.Play();
    }
    public void Play(string name, AudioSource source)
    {
        Sound s = Array.Find(_sounds, sound => sound.Name == name);
        s.Source = source;
        source.clip = s.Clip;
        source.volume = s.Volume;
        source.pitch = s.Pitch;
    }
}
