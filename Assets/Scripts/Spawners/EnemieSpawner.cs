using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieSpawner : ObjectSpawner
{
    [SerializeField] private DetectTarget detect;
    public Action RoomClear;
    private List<GameObject> Enemies = new List<GameObject>();
    protected virtual void Awake()
    {
        Enemies.Capacity = maxObjSpawned;
        detect.OnTargetInsideThisRoom += () => SpawnObjects();
    }
    public override void SpawnObjects()
    {
        int objAmount = UnityEngine.Random.Range(1, maxObjSpawned);

        for (int i = 0; i <= objAmount; i++)
        {
            EnemieIdle newObj = Instantiate(objectToSpawn, new Vector3(Location.position.x + UnityEngine.Random.Range(0, 10), Location.position.y + 2, Location.position.z + UnityEngine.Random.Range(0, 10)), Quaternion.identity);
            newObj.DetectTarget = detect;
            EnemieHealthComponent enemieHealthComponent = newObj.GetComponent<EnemieHealthComponent>();
            enemieHealthComponent.OnDeath.AddListener(RemoveEnemieFromList); 
            newObj.transform.SetParent(transform, true);
            Enemies.Add(newObj.gameObject);
            
        }
        print(Enemies.Count);
    }
    private void RemoveEnemieFromList()
    {
        Enemies.Remove(gameObject);
        if(Enemies.Count < 1)
        {
            RoomClear();
        }
    }
}
