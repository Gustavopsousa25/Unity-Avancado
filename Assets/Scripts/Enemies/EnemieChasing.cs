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
        target = FindObjectOfType<PlayerMovingState>().transform;
        WalkingMoveSpeed = GetComponent<EnemieWandering>().MoveSpeed; 
        agent = GetComponent<NavMeshAgent>();
        coneOfSightComponent = GetComponent<ConeOfSight>();
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
        coneOfSightComponent.enabled = true;   
        float ChasingMoveSpeed = WalkingMoveSpeed * moveSpeedMult;
        agent.enabled = true;
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
                if (hit.collider.gameObject.GetComponent<PlayerMovingState>())
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
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange, Color.green);
    }
}
