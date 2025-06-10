using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectTarget : MonoBehaviour
{
    [SerializeField]private GameObject room;
    public Action OnTargetInsideThisRoom;
    private GameObject target;
    private MapGenerator mapGen;

    private void Awake()
    {
        mapGen = MapGenerator.Instance;
        mapGen.OnMapReady += () => DisableRoom();
        mapGen.OnMapReady += () => FindTarget();
        
        print(target);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            room.SetActive(true);
            OnTargetInsideThisRoom?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            DisableRoom();    
        }
    }

    public void DisableRoom()
    {
        room.SetActive(false);
    }
    public void FindTarget()
    {
        target = FindObjectOfType<PlayerMovingState>().gameObject;
    }
}
