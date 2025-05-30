using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementActionMap, DashActionMap, AttackActionMap;
    private UtilLibrary utilityLib;
    private Animator anim;
    private CharacterController charController;
    public override bool Initialize()
    {
        MovementActionMap.action.Enable();
        DashActionMap.action.Enable();
        AttackActionMap.action.Enable();
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        utilityLib = GetComponent<UtilLibrary>();
        return MovementActionMap != null;
    }

    public void OnDisable()
    {
    }

    public override void OnStateFixedUpdate()
    {
    }

    public void OnEnable()
    {
        anim.SetFloat("Walkspeed", 0f);
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        Vector2 move = MovementActionMap.action.ReadValue<Vector2>();
        if (move.sqrMagnitude > 0.01f)
            return typeof(PlayerMovingState);

        /*if (MovementActionMap.action.phase == InputActionPhase.Started)
        {
            return typeof(PlayerMovingState);
        } */
        if(AttackActionMap.action.triggered)
        {
            return typeof(PlayerAttackingState);
        }
        return null;
    }
}
