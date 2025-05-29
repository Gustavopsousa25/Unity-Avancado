using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemieWandering : EntityStateBehaviour
{
    [SerializeField] private POIManager PatrolPath;
    [SerializeField] private float moveSpeed = 2f;
    private Action OnDestinationReached;
    private ConeOfSight coneOfSightComponent ;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 Destination = Vector3.zero;

    private int POIIndex = 0;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public void OnEnable()
    {
        coneOfSightComponent.enabled = true;
        agent.isStopped = false;
        agent.speed = MoveSpeed;
        anim.SetFloat("Walkspeed", MathF.Abs(MoveSpeed));
        Destination = PatrolPath.GetPOIAtIndex(POIIndex++);
        if (!PatrolPath.IsIndexValid(POIIndex))
        {
            POIIndex = 0;
        }
        transform.LookAt(Destination);
        agent.SetDestination(Destination);
        OnDestinationReached += () => PatrolPath.RandomizeAllPoints();
    }

    public override void OnStateUpdate()
    {
    }

    public void OnDisable()
    {
        agent.isStopped = true;
        anim.SetFloat("Walkspeed", 0);
        OnDestinationReached?.Invoke();
        OnDestinationReached -= () => PatrolPath.RandomizeAllPoints();
    }
    public override bool Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        coneOfSightComponent = GetComponent<ConeOfSight>();
        anim = GetComponentInChildren<Animator>();
        return agent != null && coneOfSightComponent != null && PatrolPath != null;
    }

    public override Type StateTransitionCondicion()
    {
        if (agent && !agent.hasPath)
        {
            return typeof(EnemieIdle);
        }

        if (coneOfSightComponent && coneOfSightComponent.HasSeenPlayerThisFrame())
        {
            return typeof(EnemieChasing);
        }

        return null;
    }

    public override void OnStateFixedUpdate()
    {

    }
}
