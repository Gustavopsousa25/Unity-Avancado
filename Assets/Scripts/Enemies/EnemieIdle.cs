using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieIdle : EntityStateBehaviour
{

    [SerializeField] private float detectRange = 5;
    private DetectTarget detectTarget;
    private Transform playerTarget;

    public DetectTarget DetectTarget {set => detectTarget = value; }
    public Transform PlayerTarget { get => playerTarget; set => playerTarget = value; }

    public override bool Initialize()
    {
        return true;
    }

    public void OnDisable()
    {
    }
    public void OnEnable()
    { 
        PlayerTarget = FindObjectOfType<PlayerMovingState>().gameObject.transform;
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        if(Vector3.Distance(playerTarget.position, transform.position) < detectRange)
        {
            return typeof(EnemieChasing);
        }
        return null;
    }
}
