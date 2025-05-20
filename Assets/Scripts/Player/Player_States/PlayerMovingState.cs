using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementAction;
    [SerializeField] private InputActionReference RunningAction;
    [SerializeField] private InputActionReference DashActionMap;
    [SerializeField] private InputActionReference JumpAction;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintMult;
    [Header("Jump")]
    [SerializeField] private float _JumpHeight, _jumpCooldown;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private CharacterController charController;
    private Animator anim;
    private UtilLibrary utilityLib;
    private Vector2 _move;
    private float _currentSpeed, VerticalVelocity;
    private bool /*_canJump = true ,*/ _canDash = true, _canMove = true;
    public override bool Initialize()
    {
        MovementAction.action.Enable();
        RunningAction.action.Enable();
        DashActionMap.action.Enable();
        JumpAction.action.Enable();
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        utilityLib = GetComponent<UtilLibrary>();
        return charController && anim && utilityLib;
    }

    public override void OnStateFinish()
    {
       
    }

    public override void OnStateStart()
    {
        Debug.Log("Movement State Initialized");
        _currentSpeed = _movementSpeed;
        VerticalVelocity = utilityLib.VerticalVelocity;
        MovementAction.action.performed += OnMovePerformed;
        MovementAction.action.canceled += OnMoveCancelled;
        RunningAction.action.started += OnRunningPerformed;
        RunningAction.action.canceled += OnRunningPerformed;
        DashActionMap.action.performed += OnDashPerformed;
        DashActionMap.action.canceled += OnDashPerformed;
        //JumpAction.action.performed += OnJumpPerformed;
    }

    public override void OnStateUpdate()
    {
        if (_canMove)
        {
            MoveInDirection(_currentSpeed);
            anim.SetFloat("Walkspeed", MathF.Abs(_currentSpeed));
        }        
        utilityLib.ApplyGravity(charController);
    }

    public override Type StateTransitionCondicion()
    {
        if (charController.velocity == Vector3.zero)
        {
           return typeof(PlayerIdleState);
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
    /*public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (charController.isGrounded && _canJump)
        {
            anim.SetTrigger("isJumping");
            StartCoroutine(JumpRoutine());
        }
    }*/
    public void OnRunningPerformed(InputAction.CallbackContext context)
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
    public void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash)
        {
            StartCoroutine(DashCoroutine());
        }
        
    }

    private void MoveInDirection(float movementSpeed)
    {
        Vector3 horizontal = new Vector3(_move.x, 0, _move.y).normalized * movementSpeed;
        // include vertical velocity
        Vector3 finalMove = horizontal + Vector3.up * VerticalVelocity;
        charController.Move(finalMove * Time.deltaTime);

        // rotate to face movement direction whitout vertical movement
        Vector3 newDirection = new Vector3(finalMove.x, 0, finalMove.z);
        utilityLib.FaceDirection(newDirection);
    }
    /*IEnumerator JumpRoutine()
    {        
        _canJump = false;
        utilityLib.VerticalVelocity = Mathf.Sqrt(_JumpHeight * -2f * Physics.gravity.y);
        yield return new WaitForSeconds(_jumpCooldown);
        anim.SetTrigger("isFalling");
        _canJump = true;
    }*/
    IEnumerator DashCoroutine()
    {
        _canDash = false;
        anim.SetTrigger("isDashing");
        float timer = 0f;
        while (timer < dashDuration)
        {
            _canMove = false;
            charController.Move(transform.forward * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        _canMove = true;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
}
