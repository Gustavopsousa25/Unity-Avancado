using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateBehaviour: MonoBehaviour
{
    public StateMachine AssociatedStateMachine;

    public abstract bool Initialize();

    public abstract void OnStateStart();
    public abstract void OnStateUpdate();
    public abstract void OnStateFinish();

    public abstract Type StateTransitionCondicion();
}
