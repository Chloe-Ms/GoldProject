using System.Collections.Generic;
using UnityEngine;

public class Group
{
    private List<Hero> _heroes = new List<Hero>();
    private static List<EffectEvent> _effectsEvent = new List<EffectEvent>();
    private int _nbKeysTaken = 0;
    private bool _isPoisoned = false;
    private bool _affectedByPlants = false;
    private bool _isInvulnerable = false;
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
    public bool IsInvulnerable { 
        get => _isInvulnerable; 
        set => _isInvulnerable = value; 
    }

    public void Init()
    {
        _isPoisoned = false;
        _affectedByPlants = false;
        _isInvulnerable = false;
    }

    public Hero GetHeroWithRole(Role role)
    {
        int i = 0;
        Hero hero = null;
        while (i < Heroes.Count && hero == null)
        {
            if (Heroes[i].Role == role)
            {
                hero = Heroes[i];
            }
            i++;
        }
        return hero;
    }

    public int GetHeroIndexWithRole(Role role)
    {
        int i = Heroes.Count - 1;
        bool found = false;
        while (i >= 0 && !found)
        {
            if (Heroes[i].Role == role)
            {
                found = true;
            }
            i--;
        }
        return i + 1;
    }
}
