using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int weaponDamage;
    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health == GetComponentInParent<HealthComponent>()) 
        {
            return;
        }
        if (health != null)
        {
            print("target has health");
            health.TakeDamage(weaponDamage);
        }

    }
}
