using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _range;

    private CharacterController charController;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        return charController;
    }

    public override void OnStateFinish()
    {
        throw new NotImplementedException();
    }

    public override void OnStateStart()
    {
        throw new NotImplementedException();
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
