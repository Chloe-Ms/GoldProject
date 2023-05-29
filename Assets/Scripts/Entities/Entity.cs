using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected bool _isDead = false;
    protected int _health;
    public void Attack(Entity entityAttacked)
    {
        entityAttacked.TakeDamage(_health);
    }

    public void TakeDamage(int amount)
    {
        _health = Mathf.Max(_health - amount, 0);
        if (_health <= 0)
        {
            _isDead = true;
        }
    }
}
