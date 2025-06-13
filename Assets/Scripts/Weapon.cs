using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int weaponDamage;

    public int WeaponDamage { get => weaponDamage; set => weaponDamage = value; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health == GetComponentInParent<HealthComponent>()) 
        {
            return;
        }
        if (health != null)
        {
            health.TakeDamage(WeaponDamage);
        }

    }
}
