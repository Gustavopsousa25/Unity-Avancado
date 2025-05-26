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
    private StateMachine playerStateMachine;
    private Animator anim;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        playerStateMachine = GetComponent<StateMachine>();
        return charController;
    }

    public override void OnStateFinish()
    {
        attackInput.action.performed -= OnAttack;
        AssociatedStateMachine.SetState(typeof (PlayerMovingState));
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
        Debug.Log("Input Attack");
        anim.SetTrigger("isAttacking");
    }
    public void PushPlayer()
    {
        //charController.Move(trasform.position = (transform.forward + 1));
    }
    public void Attack()
    {
        Debug.Log("Attack succed");
    }
}
