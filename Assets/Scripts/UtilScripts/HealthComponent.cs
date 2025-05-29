using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public UnityEvent OnDamaged, OnDeath;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    protected void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    public void TakeDamage(int dmgValue)
    {
        currentHealth -= dmgValue;
        OnDamaged.Invoke();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
