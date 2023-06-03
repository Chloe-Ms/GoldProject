using NaughtyAttributes;
using System;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    [SerializeField] SpriteRenderer _renderer;
    private HeroData _heroData;
    private int _health;
    private bool _isDead = false;

    public event Action OnHeroDeath;
    public event Action<int> OnDamageTaken;
    public string HeroName
    {
        get => _heroData.heroName;
        set => _heroData.heroName = value;
    }
    public int MaxHealth
    {
        get => _heroData.maxHealth;
        set => _heroData.maxHealth = value;
    }

    public Role Role 
    {
        get => _heroData.role;
        set => _heroData.role = Role;
    }
    public bool IsDead { 
        get => _isDead; 
        set => _isDead = value; 
    }
    public int Health {
        get => _health; 
        set => _health = value; 
    }

    public void TestDamage()
    {
        UpdateHealth(1);
    }
    public void UpdateHealth(int pv)
    {
        if (IsDead) return;

        int realPV; ;
        if (pv < 0)
        {
            realPV = Mathf.Min(_health, pv);
        } else {
            realPV = Mathf.Min(MaxHealth - _health, pv);
        }

        _health = Mathf.Clamp(_health + realPV, 0,MaxHealth);
        if (_health <= 0)
        {
            _isDead = true;
            OnHeroDeath?.Invoke();
        } else
        {
            OnDamageTaken?.Invoke(realPV);
        }
    }

    public void LoadHeroData(HeroData data)
    {
        _heroData = data;
        _renderer.color = _heroData.color;
        _health = _heroData.maxHealth;
    }
}
