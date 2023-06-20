using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    [SerializeField] string _name;
    [SerializeField] List<AudioClip> _clips;
    [SerializeField, Range(0f, 1f)] float _volume = 1f;
    [SerializeField, Range(.1f, 3f)] float _pitch = 1f;
    AudioSource _source;

    public string Name { 
        get => _name; 
        set => _name = value; 
    }

    public List<AudioClip> Clips {
        get => _clips; 
        private set => _clips = value; 
    }

    public float Volume { 
        get => _volume; 
        set => _volume = value; 
    }

    public float Pitch { 
        get => _pitch; 
        set => _pitch = value; 
    }

    public AudioSource Source { 
        get => _source; 
        set => _source = value; 
    }
}
