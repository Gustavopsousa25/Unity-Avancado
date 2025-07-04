using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthComponent : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    public UnityEvent OnDamaged, OnDeath;
    public Action OnDeathAction, OnDamagedAction;
    private int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        OnDeathAction?.Invoke();
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int dmgValue)
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            currentHealth -= dmgValue;
            OnDamaged?.Invoke();
            OnDamagedAction?.Invoke();
        }
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
