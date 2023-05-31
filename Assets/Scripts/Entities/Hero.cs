using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    [SerializeField] private string heroName;
    [SerializeField] private Role role;
    [SerializeField] SerializedDictionary<Effect, int> _sensibilityPerEffectType;

    public string HeroName { get => heroName; set => heroName = value; }
    public Role Role { get => role; set => role = value; }

    private void OnValidate()
    { //Check if the list is complete (editor file not complete yet)
        if (_sensibilityPerEffectType.dictionary.Count < Enum.GetValues(typeof(Effect)).Length)
        {
            foreach (Effect effectType in Enum.GetValues(typeof(Effect)))
            {
                if (!_sensibilityPerEffectType.Contains(effectType))
                {
                    _sensibilityPerEffectType.Add(effectType, 0);
                }
            }
        }
    }
    [Button]
    public void TestDamage()
    {
        TakeDamage(0);
    }
    public override void TakeDamage(int pv, Effect effect = Effect.NONE)
    {
        int amount = pv;
        amount += _sensibilityPerEffectType.Get(effect);
        base.TakeDamage(amount);
    }
}
