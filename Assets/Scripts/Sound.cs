using UnityEngine;

public class Sound
{
    [SerializeField] string _name;

    public string Name { 
        get => _name; 
        set => _name = value; 
    }

    [SerializeField] AudioClip _clip;

    public AudioClip Clip {
        get => _clip; 
        private set => _clip = value; 
    }

    [SerializeField, Range(0f, 1f)] float _volume;

    public float Volume { 
        get => _volume; 
        set => _volume = value; 
    }

    [SerializeField, Range(.1f, .3f)] float _pitch;

    public float Pitch { 
        get => _pitch; 
        set => _pitch = value; 
    }

    [SerializeField] AudioSource _source;

    public AudioSource Source { 
        get => _source; 
        set => _source = value; 
    }
}
