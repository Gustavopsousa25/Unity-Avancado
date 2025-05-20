using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConeOfSight : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField, Range(0f, 180f)] private float viewAngle = 45f;
    [SerializeField] private float viewRange = 10f;

    private GameObject target;
    private Transform targetTransform;
    private bool hasSeenPlayer;

    private void Awake()
    {
        target = FindObjectOfType<PlayerMovingState>().gameObject;
        targetTransform = target.transform;  
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            hasSeenPlayer = false;
            return;
        }

        // Direction and distance to player
        Vector3 toPlayer = targetTransform.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        // 1) Range check
        if (distanceToPlayer > viewRange)
        {
            hasSeenPlayer = false;
            return;
        }

        // 2) Angle check
        Vector3 dirNorm = toPlayer.normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, dirNorm);
        if (angleToPlayer > viewAngle)
        {
            hasSeenPlayer = false;
            return;
        }

        // 3) Line-of-sight check
        Ray ray = new Ray(transform.position, dirNorm);
        Debug.DrawRay(ray.origin, ray.direction * viewRange, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, viewRange) &&
            hitInfo.collider.GetComponent<PlayerMovingState>())
        {
            hasSeenPlayer = true;
            return;
        }

        hasSeenPlayer = false;
    }
    public bool HasSeenPlayerThisFrame()
    {
        return hasSeenPlayer;
    }
    private void OnDrawGizmosSelected()
    {
        // Draw range as a yellow wire sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        // Draw the two boundary rays of the cone
        Gizmos.color = Color.red;
        // Left boundary
        Quaternion leftRot = Quaternion.Euler(0f, -viewAngle, 0f);
        Vector3 leftDir = leftRot * transform.forward;
        Gizmos.DrawRay(transform.position, leftDir * viewRange);
        // Right boundary
        Quaternion rightRot = Quaternion.Euler(0f, viewAngle, 0f);
        Vector3 rightDir = rightRot * transform.forward;
        Gizmos.DrawRay(transform.position, rightDir * viewRange);
    }
}
