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
    private Rigidbody charRB;
    private Animator anim;
    public override bool Initialize()
    {
        charRB = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = false;
        return charRB;
    }

    public void OnDisable()
    {
        //attackInput.action.performed -= OnAttack;
        //AssociatedStateMachine.SetState(typeof (PlayerMovingState));
    }

    public void OnEnable()
    {
        attackEnded = false;
        //attackInput.action.performed += OnAttack;
        charRB.velocity = Vector3.zero;
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
        charRB.AddForce(transform.forward * pushDistance, ForceMode.Impulse);
    }
    public void StartAttackPeriod()
    {
        weaponCollider.enabled = true;
    }
    public void EndAttackPeriod()
    {
        weaponCollider.enabled = false;
    }
    public void AttackEnded()
    {
        AssociatedStateMachine.SetState(typeof(PlayerMovingState));

    }
}
