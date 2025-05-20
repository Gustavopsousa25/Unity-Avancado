using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieIdle : EntityStateBehaviour
{
    [SerializeField] private float minWaitTime, maxWaitTime;
    private ConeOfSight coneOfSightComponent;

    private Coroutine waitTimeRoutine;

    public override bool Initialize()
    {
        coneOfSightComponent = GetComponent<ConeOfSight>();
        return coneOfSightComponent;
    }

    public override void OnStateFinish()
    {
        if(waitTimeRoutine != null)
        {
            StopCoroutine(waitTimeRoutine);
        }
    }

    public override void OnStateStart()
    {
        waitTimeRoutine = StartCoroutine(WaitForTimeAndSwitchState());
    }

    public override void OnStateUpdate()
    {
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
    private IEnumerator WaitForTimeAndSwitchState()
    {
        float time = UnityEngine.Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(time);

        AssociatedStateMachine.SetState(typeof(EnemieWandering));
    }
}
