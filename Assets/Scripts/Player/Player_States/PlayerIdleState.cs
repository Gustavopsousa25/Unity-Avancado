using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : EntityStateBehaviour
{
    [SerializeField] private InputActionReference MovementActionMap;
    [SerializeField] private InputActionReference DashActionMap;
    private UtilLibrary utilityLib;
    private Animator anim;
    private CharacterController charController;
    public override bool Initialize()
    {
        MovementActionMap.action.Enable();
        DashActionMap.action.Enable();
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        utilityLib = GetComponent<UtilLibrary>();
        return MovementActionMap != null;
    }

    public override void OnStateFinish()
    {
        
    }

    public override void OnStateStart()
    {
        Debug.Log("Idle State Initialize");
        anim.SetFloat("Walkspeed", 0f);
    }

    public override void OnStateUpdate()
    {
        utilityLib.ApplyGravity(charController);
    }

    public override Type StateTransitionCondicion()
    {
        if (MovementActionMap.action.triggered)
        {
            return typeof(PlayerMovingState);
        } 
        else if (DashActionMap.action.triggered)
        {
            return typeof (PlayerDashingState);
        }
        return null;
    }
}
