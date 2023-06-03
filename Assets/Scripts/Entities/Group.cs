using System.Collections.Generic;
using UnityEngine;

public class Group
{
    private List<Hero> _heroes = new List<Hero>();
    private static List<EffectEvent> _effectsEvent = new List<EffectEvent>();
    private int _nbKeysTaken = 0;
    private bool _isPoisoned = false;
    private bool _affectedByPlants = false;
    public List<Hero> Heroes {
        get => _heroes;
        set => _heroes = value; 
    }
    public bool IsPoisoned {
        get => _isPoisoned;
        set => _isPoisoned = value; 
    }
    public static List<EffectEvent> EffectsEvent { 
        get => _effectsEvent; 
        set => _effectsEvent = value; 
    }
    public bool AffectedByPlants { 
        get => _affectedByPlants; 
        set => _affectedByPlants = value; 
    }
    public int NbKeysTaken { 
        get => _nbKeysTaken; 
        set => _nbKeysTaken = value; 
    }

    public void Init()
    {
        _isPoisoned = false;
    }
}
