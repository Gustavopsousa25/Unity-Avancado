using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectTarget : MonoBehaviour
{
    private GameObject room;
    private Collider detectionZone;
    private GameObject target;

    private void Awake()
    {
        room = FindObjectOfType<RoomContentHolder>().gameObject;
        detectionZone = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        room.SetActive(false);
        target = FindObjectOfType<PlayerMovingState>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            room.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            room.SetActive(false);
        }
    }
}
