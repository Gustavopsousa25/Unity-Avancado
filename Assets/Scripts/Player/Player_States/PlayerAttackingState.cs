using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private float pushDistance;
    private Collider weaponCollider;
    private bool attackEnded = false;
    private CharacterController charController;
    private StateMachine playerStateMachine;
    private Animator anim;
    public override bool Initialize()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        playerStateMachine = GetComponent<StateMachine>();
        weaponCollider = weapon.GetComponent<Collider>();   
        return charController;
    }

    public override void OnStateFinish()
    {
        //attackInput.action.performed -= OnAttack;
        //AssociatedStateMachine.SetState(typeof (PlayerMovingState));
    }

    public override void OnStateStart()
    {
        //attackInput.action.performed += OnAttack;
        attackEnded = false;
        anim.SetTrigger("isAttacking");
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        if (attackEnded)
        {
            return typeof(PlayerMovingState);
        }
        return null;
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
        weaponCollider.enabled = true;
    }
    public void EndAttackPeriod()
    {
        Debug.Log("End Attack Period");
        weaponCollider.enabled = false;
    }
    public void AttackEnded()
    {
        attackEnded = true;
        //AssociatedStateMachine.SetState(typeof(PlayerMovingState));
    }
}
