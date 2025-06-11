using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieIdle : EntityStateBehaviour
{
    [SerializeField] private float minWaitTime, maxWaitTime;
    private ConeOfSight coneOfSightComponent;
    private DetectTarget detectTarget;
    private Coroutine waitTimeRoutine;

    private Transform playerTarget;

    public DetectTarget DetectTarget {set => detectTarget = value; }

    public override bool Initialize()
    {
        //coneOfSightComponent = GetComponent<ConeOfSight>();
        return true;
    }

    public void OnDisable()
    {
        if(waitTimeRoutine != null)
        {
            StopCoroutine(waitTimeRoutine);
        }
    }
    public void OnEnable()
    {

        playerTarget= detectTarget.Player.transform;
    }

    public override void OnStateUpdate()
    {
        if (playerTarget==null)
        {
            playerTarget = detectTarget.Player.transform;
        }
        if (Vector3.Distance(playerTarget.position, transform.position)<3)
            ChasePlayer();
       // throw new NotImplementedException();
    }

    public override Type StateTransitionCondicion()
    {
        if(coneOfSightComponent != null && coneOfSightComponent.HasSeenPlayerThisFrame())
        {
            return typeof(EnemieChasing);
        }
        return null;
    }
    private void ChasePlayer()
    {
        AssociatedStateMachine.SetState(typeof(EnemieChasing));
    }
}
