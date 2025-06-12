using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float pushDistance = 5f;

    private int attackCount = 0;
    private Collider weaponCollider;
    private bool attackEnded = true;
    private bool shouldAttackBuffered = false;

    private Rigidbody charRB;
    private Animator anim;

    public override bool Initialize()
    {
        charRB = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = false;
        return charRB != null && anim != null && weaponCollider != null;
    }

    private void OnEnable()
    {
        attackInput.action.performed += OnAttack;
        attackInput.action.Enable();

        attackCount = 0;
        attackEnded = true;
        shouldAttackBuffered = false;

        OnAttack(new InputAction.CallbackContext()); // optional: auto-start attack on state enter
    }

    private void OnDisable()
    {
        attackInput.action.performed -= OnAttack;
    }

    public override void OnStateUpdate()
    {
        // Optional: add movement cancel logic, etc.
    }

    public override Type StateTransitionCondicion()
    {
        return null;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!attackEnded)
        {
            // Buffer the next attack if pressed during an active attack
            shouldAttackBuffered = true;
            return;
        }

        // Start a new attack
        attackEnded = false;
        charRB.velocity = Vector3.zero;
        attackCount++;
        anim.SetInteger("Attack", attackCount);
        shouldAttackBuffered = false;

        Debug.Log($"Attack {attackCount} started");
    }

    public void PushPlayer()
    {
        charRB.AddForce(transform.forward * pushDistance, ForceMode.Impulse);
    }

    public void StartAttackPeriod()
    {
        weaponCollider.enabled = true;
        attackEnded = false;
    }

    public void EndAttackPeriod()
    {
        weaponCollider.enabled = false;
        //attackEnded = true;
        // Don't check buffer here anymore
    }


    public void AttackEnded()
    {
        Debug.Log($"AttackEnded called. attackCount: {attackCount}, shouldAttackBuffered: {shouldAttackBuffered}");

        attackEnded = true;

        if (shouldAttackBuffered)
        {
            shouldAttackBuffered = false;
            OnAttack(new InputAction.CallbackContext());
            return;
        }

        attackCount = 0;
        anim.SetInteger("Attack", attackCount);
        Debug.Log("Attack combo ended");
        AssociatedStateMachine.SetState(typeof(PlayerMovingState));
    }


}
