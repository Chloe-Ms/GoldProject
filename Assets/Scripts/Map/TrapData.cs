using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap Data", menuName = "GoldProject/Trap Data", order = 3)]
public class TrapData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _miniSprite;
    [SerializeField] private Effect _effect;

    public string Name
    {
        get { return _name; }
    }

    public Sprite Sprite
    {
        get { return _sprite; }
    }

    public Effect Effect { 
        get => _effect; 
    }
    public Sprite MiniSprite { 
        get => _miniSprite; 
    }
}
