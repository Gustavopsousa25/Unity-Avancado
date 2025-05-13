using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementActionMap;
    [SerializeField] private InputActionReference JumpActionMap;
    private CharacterController charController;
    private bool _jumpRequested = false;
    public override bool Initialize()
    {
        MovementActionMap.action.Enable();
        JumpActionMap.action.Enable();
        charController = GetComponent<CharacterController>();
        return MovementActionMap != null;
    }

    public override void OnStateFinish()
    {
        
    }

    public override void OnStateStart()
    {
        Debug.Log("Idle State Initialize");
        JumpActionMap.action.performed += OnJumpPerformed;
    }

    public override void OnStateUpdate()
    {
     
    }

    public override Type StateTransitionCondicion()
    {
        Debug.Log($"[Idle] Checking transition: _jumpRequested={_jumpRequested}, grounded={charController.isGrounded}");
        if (MovementActionMap.action.triggered)
        {
            return typeof(PlayerMovingState);
        }
        if(JumpActionMap.action.triggered && charController.isGrounded)
        {
            Debug.Log("[Idle] → Jumping!");
            _jumpRequested = false;
            return typeof(PlayerJumpingState);
        }
        return null;
    }
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        _jumpRequested = true;
        Debug.Log($"[Idle] OnJumpPerformed! grounded? {charController.isGrounded}");
    }
}
