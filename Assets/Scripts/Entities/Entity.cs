using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected bool _isDead = false;
    protected int _health;
    public event Action<int> OnTakeDamage;
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public void Attack(Entity entityAttacked)
    {
        if (!_isDead)
        {
            entityAttacked.TakeDamage(_health);
        }
    }

    //If negative, heals the player
    public virtual void TakeDamage(int amount,Effect effect = Effect.NONE)
    {
        _health = Mathf.Max(_health - amount, 0);
        if (_health <= 0)
        {
            _isDead = true;
        }
        OnTakeDamage?.Invoke(amount);
    }
}
