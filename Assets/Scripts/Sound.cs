using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [SerializeField] string _name;
    [SerializeField] AudioClip _clip;
    [SerializeField, Range(0f, 1f)] float _volume = 1f;
    [SerializeField, Range(.1f, 3f)] float _pitch = 1f;
    AudioSource _source;

    public string Name { 
        get => _name; 
        set => _name = value; 
    }

    public AudioClip Clip {
        get => _clip; 
        private set => _clip = value; 
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
