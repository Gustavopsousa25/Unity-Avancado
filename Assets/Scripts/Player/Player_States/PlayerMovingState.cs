using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MoveAction, RunAction, DashAction, AttackAction;
    [SerializeField] private float walkSpeed = 5f, sprintMult = 1.5f;
    [SerializeField] private float dashSpeed = 20f, dashDuration = .2f, dashCooldown = 1f;

    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;
    private bool canDash = true;
    private float currentSpeed => moveInput == Vector2.zero ? 0f
                                : (RunAction.action.ReadValue<float>() > 0.5f
                                   ? walkSpeed * sprintMult
                                   : walkSpeed);

    public override bool Initialize()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Movement callbacks
        MoveAction.action.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        MoveAction.action.canceled += ctx => moveInput = Vector2.zero;
        MoveAction.action.Enable();

        // Dash
        DashAction.action.performed += _ =>
        {
            if (canDash) StartCoroutine(DashRoutine());
        };
        DashAction.action.Enable();

        // Attack
        AttackAction.action.performed += _ =>
            AssociatedStateMachine.SetState(typeof(PlayerAttackingState));
        AttackAction.action.Enable();

        // (We’re reading RunAction in a pull model in currentSpeed, but you can subscribe if you prefer.)
        RunAction.action.Enable();

        return rb && anim;
    }

    public override void OnStateUpdate()
    {
        // 1) Animation
        anim.SetFloat("Walkspeed", currentSpeed);

        // 2) Check high‐level transitions
        if (AttackAction.action.triggered)
            return; // (we already set state in the callback)

        if (DashAction.action.triggered)
            return; // likewise

        // 3) No more “go to idle” here — idle is just moveInput == zero
    }

    public override void OnStateFixedUpdate()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        // horizontal movement
        rb.velocity = new Vector3(dir.x * currentSpeed,
                                  rb.velocity.y,
                                  dir.z * currentSpeed);
        // face direction only if moving
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    IEnumerator DashRoutine()
    {
        canDash = false;
        anim.SetTrigger("isDashing");

        float t = 0f;
        while (t < dashDuration)
        {
            rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override Type StateTransitionCondicion()
    {
        return null;
    }
}


