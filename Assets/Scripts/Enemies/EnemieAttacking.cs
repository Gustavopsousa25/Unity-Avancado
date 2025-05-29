using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttacking : EntityStateBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private int damage;
    [SerializeField] private Transform punchLocator;
    private Collider weaponCollider;
    private Animator anim;
    private ConeOfSight coneOfSightComponent;
    public override bool Initialize()
    {
        coneOfSightComponent = GetComponent<ConeOfSight>();
        anim = GetComponentInChildren<Animator>();
        weaponCollider = weapon.GetComponent<Collider>();
        return coneOfSightComponent && anim;
    }

    public void OnDisable()
    {
        coneOfSightComponent.enabled = true;
    }

    public void OnEnable()
    {
        coneOfSightComponent.enabled = false;
        weaponCollider.enabled = false;
        anim.SetTrigger("isAttacking");
    }

    public override void OnStateUpdate()
    {
    }

    public override Type StateTransitionCondicion()
    {
        return null;
    }
    public void StartEnemieAttack()
    {
        weaponCollider.enabled = true;
       /* Collider[] hitObjects = Physics.OverlapSphere(punchLocator.transform.position, 1.0f);
        for (int i = 0; i < hitObjects.Length; ++i)
        {
            HealthComponent player = hitObjects[i].gameObject.GetComponent<HealthComponent>();
            if (player != null)
            {
                player.TakeDamage(damage);
                return;
            }
        }*/
    }
    public void EndEnemieAttack()
    {
        weaponCollider.enabled = false;
    }
    public void EndAnimation()
    {
        AssociatedStateMachine.SetState(typeof (EnemieChasing));
    }

    public override void OnStateFixedUpdate()
    {
    }
}
