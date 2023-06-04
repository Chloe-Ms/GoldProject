using System.Collections.Generic;
using UnityEngine;

public class Trap : Room
{
    List<Effect> _listEffects = new List<Effect>();
    bool _isActive = true;
    int _nbOfUpgrades = 0;
    
    public bool IsActive { 
        get => _isActive; 
        set => _isActive = value; 
    }

    public List<Effect> Effects
    {
        get => _listEffects;
        private set => _listEffects = value;
    }
    public int NbOfUpgrades {
        get => _nbOfUpgrades; 
        set => _nbOfUpgrades = value; 
    }

    public bool IsElementary
    {
        get => _listEffects[0] == Effect.FOUDRE || _listEffects[0] == Effect.FEU || _listEffects[0] == Effect.GLACE;
    }
}
