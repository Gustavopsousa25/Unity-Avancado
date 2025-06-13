using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{
    private UIManager uiManager;
    private void Start()
    {
        uiManager = UIManager.Instance;
        OnDamagedAction += () => uiManager.UpdateHpBar(GetCurrentHealth(),maxHealth);
    }

}
