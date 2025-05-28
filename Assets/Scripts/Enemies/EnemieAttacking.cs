using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttacking : EntityStateBehaviour
{
    [SerializeField] private int damage;
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
        AssociatedStateMachine.SetState(typeof(EnemieChasing));
    }

    public override void OnStateStart()
    {
        coneOfSightComponent.enabled = false;
    }

    public override void OnStateUpdate()
    {
        throw new NotImplementedException();
    }

    public override Type StateTransitionCondicion()
    {
        throw new NotImplementedException();
    }
}
