using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform destination;

    public void SetDoorDestination(Transform target)
    {
        destination = target;
    }

    public void OnCollisionEnter(Collision collision)
    {
        PlayerMovingState player = collision.gameObject.GetComponent<PlayerMovingState>();
        if (player != null)
        {
            player.transform.position = destination.position - destination.forward * 1;
        }
    }
}
