using NaughtyAttributes;
using System;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    [SerializeField] SpriteRenderer _renderer;
    private HeroData _heroData;
    private int _health;
    private bool _isDead = false;
    private bool _isinvulnerable = false;
    private int _nbDamageOnElementaryRoom = 0; //Pas de meilleur endroit où le mettre :/

    public event Action<Hero> OnHeroDeath;
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
    public bool Isinvulnerable {
        get => _isinvulnerable; 
        set => _isinvulnerable = value; 
    }
    public int NbDamageOnElementaryRoom { 
        get => _nbDamageOnElementaryRoom; 
        set => _nbDamageOnElementaryRoom = value; 
    }

    public void TestDamage()
    {
        UpdateHealth(1);
    }
    public void UpdateHealth(int pv)
    {
        if (IsDead || Isinvulnerable)
        {
            return;
        }

        int realPV; ;
        if (pv < 0) //DAMAGE
        {
            realPV = Mathf.Min(_health, pv);
            if (Role == Role.MAGE && GameManager.Instance.IsCurrentRoomElementary)
                _nbDamageOnElementaryRoom++;

        } else { //HEAL
            realPV = Mathf.Min(MaxHealth - _health, pv);
        }

        _health = Mathf.Clamp(_health + realPV, 0,MaxHealth);
        if (_health <= 0)
        {
            _isDead = true;
            OnHeroDeath?.Invoke(this);
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
