using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] _sounds;
    [SerializeField] AudioSource[] _bgSources;

    private static AudioManager instance;

    public static AudioManager Instance { 
        get => instance; 
        set => instance = value; 
    }

    private bool _isMuted = false;
    
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
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
        }
    }

    public void Play(string name)
    {
        if (!_isMuted){
            Sound s = Array.Find(_sounds, sound => sound.Name == name);
            if (s.Clips.Count > 0) 
            {
                if (s.Clips.Count == 1)
                {
                    s.Source.clip = s.Clips[0];
                } else
                {
                    int index = UnityEngine.Random.Range(0, s.Clips.Count);
                    s.Source.clip = s.Clips[index];
                }
                if (s.Source.isPlaying)
                {
                    s.Source.Stop();
                }
                s.Source.Play();
            }
        }
    }
    public void Play(string name, AudioSource source)
    {
        if (!_isMuted){
            Sound s = Array.Find(_sounds, sound => sound.Name == name);
            if (s.Clips.Count > 0)
            {
                s.Source = source;
                if (s.Clips.Count == 1)
                {
                    s.Source.clip = s.Clips[0];
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, s.Clips.Count);
                    s.Source.clip = s.Clips[index];
                }
                source.volume = s.Volume;
                source.pitch = s.Pitch;
            }
        }
    }

    public void MuteAllSounds()
    {
        foreach (Sound sound in _sounds)
        {
            sound.Source.Stop();
        }

        foreach (AudioSource source in _bgSources)
        {
            source.mute = true;
        }
        _isMuted = true;
    }

    public void DemuteAllSounds(){
        foreach (AudioSource source in _bgSources)
        {
            source.mute = false;
        }
        _isMuted = false;
    }

    public void StopBackgroundMusic()
    {
        
    }
}