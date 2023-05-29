using System;
using UnityEngine;

public class Hero : Entity
{
    [SerializeField] private string heroName;
    [SerializeField] private Role role;
    [SerializeField] SerializedDictionary<Effect, int> _sensibilityPerEffectType;

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
}
