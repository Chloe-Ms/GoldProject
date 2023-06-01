using UnityEngine;

public class Trap : Room
{
    Effect[] _listEffects;
    bool _isActive = true;
    int _nbOfUpgrades = 0;
    
    public bool IsActive { 
        get => _isActive; 
        set => _isActive = value; 
    }

    public Effect[] Effects
    {
        get => _listEffects;
        private set => _listEffects = value;
    }
    public int NbOfUpgrades {
        get => _nbOfUpgrades; 
        set => _nbOfUpgrades = value; 
    }
}
