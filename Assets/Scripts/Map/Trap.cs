using UnityEngine;

public class Trap : Room
{
    [SerializeField] Effect _effect = Effect.NONE;

    public Effect Effect { 
        get => _effect; 
        set => _effect = value; 
    }
}
