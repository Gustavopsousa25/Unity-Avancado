using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;
    private MapGenerator mapGen;
    private void Awake()
    {
       surface = GetComponent<NavMeshSurface>();
       mapGen = MapGenerator.Instance;
       mapGen.OnMapReady += () => BakeNavMesh();
    }
    public void BakeNavMesh()
    {
        if(surface.navMeshData != null)
        {
            surface.RemoveData();
        }
        surface.BuildNavMesh();
    }
}
