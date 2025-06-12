using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieChasing : EntityStateBehaviour
{
    [SerializeField] private float RunningSpeedMult, attackRange;
    [SerializeField] private float WalkingMoveSpeed;
    private Transform target;
    private NavMeshAgent agent;
    private Animator anim; 
    private EnemieIdle enemieIdle;

    public override bool Initialize()
    {
        enemieIdle = GetComponent<EnemieIdle>();
        target = enemieIdle.PlayerTarget;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        return target && agent;
    }

    public void OnDisable()
    {
        agent.enabled = false;
        anim.SetFloat("Walkspeed", MathF.Abs(0));
    }

    public void OnEnable()
    {
        float ChasingMoveSpeed = WalkingMoveSpeed * RunningSpeedMult;
        agent.enabled = true;
        agent.speed = ChasingMoveSpeed;
        anim.SetFloat("Walkspeed", MathF.Abs(ChasingMoveSpeed));
    }

    public override void OnStateUpdate()
    {
            ChasePlayer();

            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackRange))
            {
                if (hit.collider.gameObject.GetComponent<PlayerMovingState>())
                {
                    AssociatedStateMachine.SetState(typeof (EnemieAttacking));
                }
            }
    }

    public override Type StateTransitionCondicion()
    {
        return null;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange, Color.green);
    }
}
