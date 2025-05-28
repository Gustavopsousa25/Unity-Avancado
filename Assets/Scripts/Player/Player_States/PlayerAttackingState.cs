using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
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
        //attackInput.action.performed -= OnAttack;
        AssociatedStateMachine.SetState(typeof (PlayerMovingState));
    }

    public override void OnStateStart()
    {
        //attackInput.action.performed += OnAttack;
        anim.SetTrigger("isAttacking");
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        throw new NotImplementedException();
    }
    /*public void OnAttack(InputAction.CallbackContext context)
    {
        anim.SetTrigger("isAttacking");
    }*/
    public void PushPlayer()
    {
        //charController.Move(trasform.position = (transform.forward + 1));
    }
    public void StartAttackPeriod()
    {
        Debug.Log("Start Attack Period");
    }
    public void EndAttackPeriod()
    {
        Debug.Log("End Attack Period");
    }
    public void AttackEnded()
    {
        
    }
}
