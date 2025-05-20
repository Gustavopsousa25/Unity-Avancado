using System.Collections.Generic;
using UnityEngine;

// Basic Manager For Points of interest, used as a patrol point at this moment.
public class POIManager : MonoBehaviour
{
    [SerializeField] private List<Transform> PointsOfInterest = new List<Transform>();
    [SerializeField] private float maxRange = 10f;
    [SerializeField, Range(0f, 10f)] private float minDis;

    private Transform origin;

    private void Awake()
    {
        origin = GetComponent<Transform>();
    }
    // Get an element from the list.
    public Transform GetPOIAtIndex(int index)
    {
        if (!IsIndexValid(index)) 
            return null;
        
        return PointsOfInterest[index];
    }

    // Check if the index is valid in the list
    public bool IsIndexValid(int index)
    {
        return index < PointsOfInterest.Count && index >= 0;
    }
    public void RandomizeAllPoints()
    {
        for (int i = 0; i < PointsOfInterest.Count; i++)
            RandomizePointPosition(i);
    }
    private Transform RandomizePointPosition(int index)
    {
        var point = PointsOfInterest[index];
        point.position = GetRandomPositionInRange(origin.position, minDis, maxRange);
        return PointsOfInterest[index];
    }
    private Vector3 GetRandomPositionInRange(Vector3 center, float innerRadius, float outerRadius)
    {
        // pick a random direction on XZ
        Vector2 unitCircle = Random.insideUnitCircle.normalized;
        // pick a distance between inner and outer radius
        float distance = Random.Range(innerRadius, outerRadius);
        // build the final position
        Vector3 offset = new Vector3(unitCircle.x, 0f, unitCircle.y) * distance;
        return center + offset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDis);
    }

}
