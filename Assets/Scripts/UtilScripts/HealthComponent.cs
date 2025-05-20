using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private UnityEvent OnDamaged, OnDeath;
    private int currentHealth;

    protected void Die()
    {
        OnDeath.Invoke();
    }

    protected void TakeDamage(int dmgValue)
    {
        currentHealth -= dmgValue;
        OnDamaged.Invoke();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
