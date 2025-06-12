using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float pushDistance;
    private int attackCount;
    private Collider weaponCollider;
    private bool attackEnded = true;
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
        attackInput.action.performed -= OnAttack;

    }

    public void OnEnable()
    {
        attackInput.action.performed += OnAttack;
        attackInput.action.Enable();
        OnAttack(new InputAction.CallbackContext());
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        return null;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        print(attackEnded);
        if (attackEnded)
        {
            attackEnded = false;
            charRB.velocity = Vector3.zero;
            if (attackCount == 0 || attackCount == 2)
            {
                attackCount = 1;
            }
            else
            {
                attackCount = 2;
            }

            print(attackCount);
            anim.SetInteger("Attack", attackCount);
        }
    }
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
        attackEnded = true;
    }
    public void AttackEnded()
    {
        attackCount = 0;
        anim.SetInteger("Attack", attackCount);
        print("ended");
        AssociatedStateMachine.SetState(typeof(PlayerMovingState));
    }
}
