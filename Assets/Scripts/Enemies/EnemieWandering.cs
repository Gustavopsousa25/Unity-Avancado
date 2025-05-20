using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemieWandering : EntityStateBehaviour
{
    [SerializeField] private POIManager PatrolPath;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private UnityEvent OnDestinationReached;
    private ConeOfSight ConeOfSightComponent ;
    private NavMeshAgent agent ;
    private Animator anim;
    private Vector3 Destination = Vector3.zero;

    private int POIIndex = 0;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public override void OnStateStart()
    {
        agent.isStopped = false;
        agent.speed = MoveSpeed;
        anim.SetFloat("Walkspeed", MathF.Abs(MoveSpeed));
        Destination = PatrolPath.GetPOIAtIndex(POIIndex++).position;
        if (!PatrolPath.IsIndexValid(POIIndex))
        {
            POIIndex = 0;
        }
        transform.LookAt(Destination);
        agent.SetDestination(Destination);
        OnDestinationReached.AddListener(PatrolPath.RandomizeAllPoints);
    }

    public override void OnStateUpdate()
    {
    }

    public override void OnStateFinish()
    {
        agent.isStopped = true;
        anim.SetFloat("Walkspeed", 0);
        OnDestinationReached?.Invoke();
    }
    public override bool Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        ConeOfSightComponent = GetComponent<ConeOfSight>();
        anim = GetComponentInChildren<Animator>();
        return agent != null && ConeOfSightComponent != null && PatrolPath != null;
    }

    public override Type StateTransitionCondicion()
    {
        if (agent && !agent.hasPath)
        {
            return typeof(EnemieIdle);
        }

        if (ConeOfSightComponent && ConeOfSightComponent.HasSeenPlayerThisFrame())
        {
            return typeof(EnemieChasing);
        }

        return null;
    }
}
