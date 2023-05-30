using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "GoldProject/Create Hero", order = 1)]
public class HeroData : EntityData
{
    public string heroName;
    public Role role;
    [SerializeField] SerializedDictionary<Effect,int> _effects;

    public SerializedDictionary<Effect, int> Effects { 
        get => _effects; 
        set => _effects = value; 
    }

    private void OnEnable()
    {
        if (_effects == null)
        {
            _effects = new SerializedDictionary<Effect,int>();
            foreach (Effect effectType in Enum.GetValues(typeof(Effect)))
            {
                _effects.Add(effectType, 0);
            }
        }
    }
}
