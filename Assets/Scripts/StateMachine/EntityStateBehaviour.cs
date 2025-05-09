using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateBehaviour: MonoBehaviour
{
    protected StateMachine AssociatedStateMachine;

    public abstract bool Initialize();
}
