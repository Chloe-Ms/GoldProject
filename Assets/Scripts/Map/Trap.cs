using UnityEngine;

public class Trap : Room
{
    Effect _effect = Effect.NONE;
    bool _isActive = true;
    int _nbOfUpgrades = 0;
    
    public Effect Effect { 
        get => _effect; 
        set => _effect = value; 
    }
    public bool IsActive { 
        get => _isActive; 
        set => _isActive = value; 
    }
}
