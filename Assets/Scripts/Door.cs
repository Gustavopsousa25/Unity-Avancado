using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Action OnPlayerTeleported;
    private Transform destination;
    private Door destinationDoor;
    private bool onCoolDown = false;

    public bool OnCoolDown { get => onCoolDown; set => onCoolDown = value; }
    public void SetDoorDestination(Transform target)
    {
        destination = target;
        destinationDoor = destination.GetComponent<Door>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        PlayerMovingState player = collision.gameObject.GetComponent<PlayerMovingState>();
        if (player != null && OnCoolDown == false)
        {
            player.transform.position = destination.position - destination.forward * 1.5f;
            destinationDoor?.OnPlayerTeleported?.Invoke();
            print("Player Teleported to the destination door");
        }
    }
}
