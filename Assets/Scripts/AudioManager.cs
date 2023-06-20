using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] _sounds;
    [SerializeField] BackgroundSound[] _bgSources;
    [SerializeField] string _startBackgroundMusic;
    BackgroundSound _currentBackgroundMusic = null;

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

    private void Start()
    {
        BackgroundSound s = Array.Find(_bgSources, sound => sound.Name == _startBackgroundMusic);
        s?.Source.Play();
        _currentBackgroundMusic = s;
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
                if (s.Source.isPlaying)
                {
                    s.Source.Stop();
                }
                s.Source.Play();
            }
        }
    }

    public void MuteAllSounds()
    {
        foreach (Sound sound in _sounds)
        {
            sound.Source.Stop();
        }

        /*foreach (KeyValuePair<string, AudioSource> source in _bgSources)
        {
            source.Value.mute = true;
        }*/
        foreach (BackgroundSound source in _bgSources)
        {
            source.Source.mute = true;
        }
        _isMuted = true;
    }
    [SerializeField] private string _name;

    public void DemuteAllSounds(){
        /*foreach (KeyValuePair<string, AudioSource> source in _bgSources)
        {
            source.Value.mute = false;
        }*/
        foreach (BackgroundSound source in _bgSources)
        {
            source.Source.mute = false;
        }
        _isMuted = false;
    }
    [Button]
    public void PlayMusic()
    {
        PlayBackgroundMusic(_name);
    }
    [Button]
    public void StopMusic()
    {
        StopBackgroundMusic(_name);
    }

    public void PlayBackgroundMusic(string name)
    {
        BackgroundSound s = Array.Find(_bgSources, sound => sound.Name == name);
        _currentBackgroundMusic?.Source.Stop();
        _currentBackgroundMusic = s;
        s.Source.Play();
    }
    public void StopBackgroundMusic(string name)
    {
        BackgroundSound s = Array.Find(_bgSources, sound => sound.Name == name);
        s.Source.Stop();
    }
    [Button]
    public void StopCurrentBackgroundMusic()
    {
        _currentBackgroundMusic.Source.Stop();
        _currentBackgroundMusic = null;
    }
}