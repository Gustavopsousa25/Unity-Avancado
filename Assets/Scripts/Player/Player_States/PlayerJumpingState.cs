using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementActionMap;

    [Header("Jump")]
    [SerializeField] private float _JumpHeight;

    private UtilLibrary utilityLib;
    private CharacterController charController;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        utilityLib = GetComponent<UtilLibrary>();
        return charController;
    }

    public override void OnStateFinish()
    {
    }

    public override void OnStateStart()
    {
        utilityLib.VerticalVelocity = Mathf.Sqrt(_JumpHeight * -2f * Physics.gravity.y);
    }

    public override void OnStateUpdate()
    {
        utilityLib.ApplyGravity(charController);
        charController.Move(Vector3.up * utilityLib.VerticalVelocity * Time.deltaTime);
    }

    public override Type StateTransitionCondicion()
    {
        if (MovementActionMap.action.triggered)
        {
            return typeof(PlayerMovingState);
        }
        return null;
    }

}
