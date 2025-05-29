using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieChasing : EntityStateBehaviour
{
    [SerializeField] private float moveSpeedMult, attackRange;
    private Transform target;
    private NavMeshAgent agent;
    private Animator anim;
    private float WalkingMoveSpeed;
    private ConeOfSight coneOfSightComponent;

    public override bool Initialize()
    {
        target = FindObjectOfType<PlayerIdleState>().transform;
        WalkingMoveSpeed = GetComponent<EnemieWandering>().MoveSpeed; 
        agent = GetComponent<NavMeshAgent>();
        coneOfSightComponent = GetComponent<ConeOfSight>();
        anim = GetComponentInChildren<Animator>();
        return target && agent;
    }

    public override void OnStateFinish()
    {
        agent.isStopped = true;
        anim.SetFloat("Walkspeed", MathF.Abs(0));
    }

    public override void OnStateStart()
    {
        coneOfSightComponent.enabled = true;   
        float ChasingMoveSpeed = WalkingMoveSpeed * moveSpeedMult;
        agent.isStopped = false;
        agent.speed = ChasingMoveSpeed;
        anim.SetFloat("Walkspeed", MathF.Abs(ChasingMoveSpeed));
        ChasePlayer();
    }

    public override void OnStateUpdate()
    {
        if (target != null && coneOfSightComponent.HasSeenPlayerThisFrame())
        {
            ChasePlayer();

            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackRange))
            {
                if (hit.collider.gameObject == target)
                {
                    AssociatedStateMachine.SetState(typeof (EnemieAttacking));
                }
            }
        }
    }

    public override Type StateTransitionCondicion()
    {
        if(!coneOfSightComponent.HasSeenPlayerThisFrame() && agent.isStopped)
        {
            return typeof(EnemieWandering);
        }
        else if (!agent.hasPath)
        {
            return typeof(EnemieIdle);
        }
        return null;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange, Color.green);
    }
}
