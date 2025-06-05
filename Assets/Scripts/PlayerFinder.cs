using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private Transform target;
    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        target = FindObjectOfType<PlayerMovingState>().gameObject.transform;
        cam.Follow = target;
        cam.LookAt = target;

       
    }
}
