using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private PlayerWeapon weapon;
    [SerializeField] private float attackDamage;
    [SerializeField] private float dmgMult = 1.5f;
    [SerializeField] private float pushDistance = 5f;

    private float currentDmgMult;
    private int attackCount = 0;
    private Collider weaponCollider;
    private bool attackEnded = true;
    private bool shouldAttackBuffered = false;

    private Rigidbody charRB;
    private Animator anim;
    private RythmManager rythmManager;

    public override bool Initialize()
    {
        charRB = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        weaponCollider = weapon.GetComponent<Collider>();
        weapon.WeaponDamage = (int)attackDamage;
        weaponCollider.enabled = false;
        rythmManager = RythmManager.Instance;
        return charRB != null && anim != null && weaponCollider != null;
    }

    private void OnEnable()
    {
        attackInput.action.performed += OnAttack;
        attackInput.action.Enable();

        attackCount = 0;
        attackEnded = true;
        shouldAttackBuffered = false;

        OnAttack(new InputAction.CallbackContext());
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
        bool onBeat = rythmManager.IsOnBeat();
        if (!attackEnded)
        {
            // Buffer the next attack if pressed during an active attack
            shouldAttackBuffered = true;
            return;
        }
        if (onBeat)
        {
            weapon.WeaponDamage = Mathf.RoundToInt(attackDamage * dmgMult);
        }
        // Start a new attack
        attackEnded = false;
        charRB.velocity = Vector3.zero;
        attackCount++;
        anim.SetInteger("Attack", attackCount);
        shouldAttackBuffered = false;

        Debug.Log($"Attack {attackCount} started with "+ weapon.WeaponDamage);
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
        weapon.WeaponDamage = (int)attackDamage;
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
