using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttacking : EntityStateBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Transform punchLocator;
    private Animator anim;
    private ConeOfSight coneOfSightComponent;
    public override bool Initialize()
    {
        coneOfSightComponent = GetComponent<ConeOfSight>();
        anim = GetComponentInChildren<Animator>();
        return coneOfSightComponent && anim;
    }

    public override void OnStateFinish()
    {
        coneOfSightComponent.enabled = true;
        AssociatedStateMachine.SetState(typeof(EnemieChasing));
    }

    public override void OnStateStart()
    {
        coneOfSightComponent.enabled = false;
        anim.SetTrigger("isAttacking");
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        throw new NotImplementedException();
    }
    public void EnemieAttack()
    {
        Collider[] hitObjects = Physics.OverlapSphere(punchLocator.transform.position, 1.0f);
        for (int i = 0; i < hitObjects.Length; ++i)
        {
            HealthComponent player = hitObjects[i].gameObject.GetComponent<HealthComponent>();
            if (player != null)
            {
                player.TakeDamage(damage);
                return;
            }
        }
    }
}
