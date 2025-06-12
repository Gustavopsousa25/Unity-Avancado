using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieHealthComponent : HealthComponent
{
    private EnemieSpawner parent;

    protected override void Awake()
    {
        base.Awake();
        parent = GetComponentInParent<EnemieSpawner>();
    }
}
