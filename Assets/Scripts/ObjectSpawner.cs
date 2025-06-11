using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int maxObjSpawned = 5;
    [SerializeField] private EnemieIdle objectToSpawn;
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
            EnemieIdle newObj = Instantiate(objectToSpawn, new Vector3(Location.position.x /*+ UnityEngine.Random.Range(0,3)*/,Location.position.y+ 2,Location.position.z /*= UnityEngine.Random.Range(0, 3)*/), Quaternion.identity);
           newObj.DetectTarget = detect;
            newObj.transform.SetParent(transform,true);
        }
        print(objAmount);
    }
}
