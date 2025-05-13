using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementAction;
    [SerializeField] private InputActionReference RunningAction;
    [SerializeField] private InputActionReference JumpMap;
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintMult;

    private CharacterController charController;
    private UtilLibrary utilityLib;
    private Vector2 _move;
    private float _currentSpeed, _verticalVelocity;
    public override bool Initialize()
    {
        JumpMap.action.Enable();
        charController = GetComponent<CharacterController>();
        utilityLib = GetComponent<UtilLibrary>();
        return charController;
    }

    public override void OnStateFinish()
    {
       
    }

    public override void OnStateStart()
    {
        Debug.Log("Movement State Initialized");
        _currentSpeed = _movementSpeed;
        MovementAction.action.performed += OnMovePerformed;
        MovementAction.action.canceled += OnMoveCancelled;
        RunningAction.action.started += OnRuningPerformed;
        RunningAction.action.canceled += OnRuningPerformed;
    }

    public override void OnStateUpdate()
    {
        MoveInDirection(_currentSpeed);
        utilityLib.ApplyGravity(charController);
    }

    public override Type StateTransitionCondicion()
    {
        if (charController.velocity == Vector3.zero)
        {
           return typeof(PlayerIdleState);
        }
        else if (JumpMap.action.triggered && charController.isGrounded)
        {
            return typeof(PlayerJumpingState);
        }
        return null;
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        _move = Vector2.zero;
    }
    public void OnRuningPerformed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _currentSpeed = _movementSpeed * _sprintMult;
        }
        else if (context.canceled)
        {
            _currentSpeed = _movementSpeed;
        }
    }

    private void MoveInDirection(float movementSpeed)
    {
        Vector3 horizontal = new Vector3(_move.x, 0, _move.y).normalized * movementSpeed;
        // include vertical velocity
        Vector3 finalMove = horizontal + Vector3.up * utilityLib.VerticalVelocity;
        charController.Move(finalMove * Time.deltaTime);

        // rotate to face movement direction whitout vertical movement
        Vector3 newDirection = new Vector3(finalMove.x, 0, finalMove.z);
        utilityLib.FaceDirection(newDirection);
    }
    
}
