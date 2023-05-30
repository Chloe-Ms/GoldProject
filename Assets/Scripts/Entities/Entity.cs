using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected bool _isDead = false;
    protected int _health;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public void Attack(Entity entityAttacked)
    {
        if (!_isDead)
        {
            entityAttacked.TakeDamage(_health);
        }
    }

    public virtual void TakeDamage(int amount,Effect effect = Effect.NONE)
    {
        _health = Mathf.Max(_health - amount, 0);
        if (_health <= 0)
        {
            _isDead = true;
        }
    }
}
