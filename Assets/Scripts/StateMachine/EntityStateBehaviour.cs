using System;
using UnityEngine;

public abstract class EntityStateBehaviour: MonoBehaviour
{
    public StateMachine AssociatedStateMachine;
    public abstract bool Initialize();
    public abstract void OnStateUpdate();
    public abstract Type StateTransitionCondicion();
}
