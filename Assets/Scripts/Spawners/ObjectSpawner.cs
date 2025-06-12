using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] protected int maxObjSpawned = 5;
    [SerializeField] protected EnemieIdle objectToSpawn;
    [SerializeField] protected Transform location;
    public Transform Location { get => location; set => location = value; }
    public virtual void SpawnObjects()
    {
    }
}
