using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private Transform target;
    private MapGenerator mapGen;
    private void Awake()
    {
        mapGen = MapGenerator.Instance;
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        FindPlayer();
    }
    private void FindPlayer()
    {
        target = FindObjectOfType<PlayerMovingState>().gameObject.transform;
        cam.Follow = target;
        cam.LookAt = target;
    }
}
