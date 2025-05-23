using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private float pushDistance;
    private CharacterController charController;
    private Animator anim;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        return charController;
    }

    public override void OnStateFinish()
    {
        throw new NotImplementedException();
    }

    public override void OnStateStart()
    {
        attackInput.action.performed += OnAttack;
    }

    public override void OnStateUpdate()
    {
        throw new NotImplementedException();
    }

    public override Type StateTransitionCondicion()
    {
        throw new NotImplementedException();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        anim.SetTrigger("isAttacking");
    }
    public void PushPlayer()
    {
        //charController.Move(trasform.position = (transform.forward + 1));
    }
}
