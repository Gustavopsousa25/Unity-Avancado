using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int maxObjSpawned = 5;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform location;
    [SerializeField] private DetectTarget detect;

    public Transform Location { get => location; set => location = value; }

    private void Awake()
    {
        detect.OnTargetInsideThisRoom += () => SpawnObjects();
    }
    public void SpawnObjects()
    {
        int objAmount = UnityEngine.Random.Range(1, maxObjSpawned);

        for(int i = 0; i <= objAmount; i++)
        {
            GameObject newObj = Instantiate(objectToSpawn, new Vector3(Location.position.x /*+ UnityEngine.Random.Range(0,3)*/, 1,Location.position.y /*= UnityEngine.Random.Range(0, 3)*/), quaternion.identity, transform);
        }
        print(objAmount);
    }
}
