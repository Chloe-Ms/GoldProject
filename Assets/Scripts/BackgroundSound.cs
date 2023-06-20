using System;
using UnityEngine;

[Serializable]
public class BackgroundSound
{
    [SerializeField] string _name;
    [SerializeField] AudioSource _source;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public AudioSource Source
    {
        get => _source;
        set => _source = value;
    }
}
