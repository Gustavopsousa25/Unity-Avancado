using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectTarget : MonoBehaviour
{
    [SerializeField]private GameObject room;
    public Action OnTargetInsideThisRoom;
    private MapGenerator mapGen;
    private PlayerMovingState _player;

    public PlayerMovingState Player { get => _player; set => _player = value; }

    private void Awake()
    {
        mapGen = MapGenerator.Instance;
        mapGen.OnMapReady += () => DisableRoom();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovingState player))
        {
            _player = player;
            room.SetActive(true);
            OnTargetInsideThisRoom?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovingState player))
        {
            DisableRoom();    
        }
    }

    public void DisableRoom()
    {
        room.SetActive(false);
    }
}
