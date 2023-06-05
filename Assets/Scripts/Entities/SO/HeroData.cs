using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "GoldProject/Create Hero", order = 1)]
public class HeroData : ScriptableObject
{
    public int maxHealth;
    public string heroName;
    public Role role;
    public RuntimeAnimatorController _animator;
    public Sprite _sprite;
    public Color color;
}
