using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference DashAction;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private CharacterController charController;
    private bool _animationEnded, _canDash;
    private Animator anim;
    private UtilLibrary utilityLib;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        utilityLib = GetComponent<UtilLibrary>();
        _canDash = true;
        return charController;
    }

    public override void OnStateFinish()
    {
    }

    public override void OnStateStart()
    {
        DashAction.action.performed += OnDashperformed;
  
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        if (_animationEnded)
        {
            return typeof(PlayerMovingState);
        }
        return null;
    }
    public void OnDashperformed(InputAction.CallbackContext context)
    {
        _animationEnded = false;
        if (_canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    IEnumerator DashCoroutine()
    {
        _canDash = false;
        anim.SetTrigger("isDashing");   
        float timer = 0f;
        while (timer < dashDuration)
        {
            utilityLib.ApplyGravity(charController);
            charController.Move(transform.forward * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        } 
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
    public void EndAnimation()
    {
        _animationEnded = true;
    }
}

