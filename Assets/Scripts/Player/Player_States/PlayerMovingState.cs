using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementAction, RunningAction, DashActionMap, JumpAction, AttackActionMap;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintMult;
    [Header("Jump")]
    [SerializeField] private float _JumpHeight, _jumpCooldown;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private Rigidbody charRB;
    private Animator anim;
    private UtilLibrary utilityLib;
    private Vector2 _move;
    private float _currentSpeed, VerticalVelocity;
    private bool /*_canJump = true ,*/ _canDash = true, _canMove = true;

    Vector3 move;
    public override bool Initialize()
    {
        MovementAction.action.Enable();
        RunningAction.action.Enable();
        DashActionMap.action.Enable();
        AttackActionMap.action.Enable();
        //JumpAction.action.Enable();
        charRB = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        utilityLib = GetComponent<UtilLibrary>();
        return charRB && anim && utilityLib;
    }

    public void OnDisable()
    {
        MovementAction.action.performed -= OnMovePerformed;
        MovementAction.action.canceled -= OnMoveCancelled;
        RunningAction.action.started -= OnRunningPerformed;
        RunningAction.action.canceled -= OnRunningPerformed;
        DashActionMap.action.performed -= OnDashPerformed;
        DashActionMap.action.canceled -= OnDashPerformed;
        AttackActionMap.action.started -= OnAttackPerformed;
        AttackActionMap.action.canceled -= OnAttackPerformed;
    }

    public void OnEnable()
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
        AttackActionMap.action.started += OnAttackPerformed;
        AttackActionMap.action.canceled += OnAttackPerformed;
        //JumpAction.action.performed += OnJumpPerformed;
    }

    public override void OnStateUpdate()
    {
        if (_canMove)
        {
            MoveInDirection(_currentSpeed);
            anim.SetFloat("Walkspeed", MathF.Abs(_currentSpeed));
        }        
    }
    public override void OnStateFixedUpdate()
    {
        // Calculate movement direction
        Vector3 moveDirection = move.normalized;

        // Only apply movement if input is present
        if (moveDirection.magnitude > 0f)
        {
            // Set velocity directly to move at fixed speed
            Vector3 velocity = moveDirection * _movementSpeed;
            charRB.velocity = new Vector3(velocity.x, charRB.velocity.y, velocity.z);

            // Face movement direction (ignore vertical)
            utilityLib.FaceDirection(new Vector3(move.x, 0, move.z));
        }
        else
        {
            // No input – stop horizontal movement completely
            charRB.velocity = new Vector3(0f, charRB.velocity.y, 0f);
        }
    }

    public override Type StateTransitionCondicion()
    {
        if (_move == Vector2.zero)
        {
           return typeof(PlayerIdleState);
        }
        if (AttackActionMap.action.triggered)
        {
            return typeof(PlayerAttackingState);
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
        move = Vector3.zero;
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
        _currentSpeed = _movementSpeed * _sprintMult;
    }
    public void OnRunningCancelled(InputAction.CallbackContext context)
    {
        _currentSpeed = _movementSpeed;
    }

    public void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash)
        {
            StartCoroutine(DashCoroutine());
        }       
    }
    public void OnDashCancelled(InputAction.CallbackContext context)
    {
    }
    public void OnAttackPerformed(InputAction.CallbackContext context)
    {
       //AssociatedStateMachine.SetState(typeof (PlayerAttackingState));
    }
    private void MoveInDirection(float movementSpeed)
    {
        Vector3 horizontal = new Vector3(_move.x, 0, _move.y).normalized * movementSpeed;
        // include vertical velocity
       // Vector3 finalMove = horizontal + Vector3.up * VerticalVelocity;
        move = horizontal;


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
            charRB.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
            timer += Time.deltaTime;
            yield return null;
        }
        _canMove = true;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    
}
