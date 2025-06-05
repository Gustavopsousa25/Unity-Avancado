using System.Collections.Generic;
using UnityEngine;

// Basic Manager For Points of interest, used as a patrol point at this moment.
public class POIManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> PointsOfInterest = new List<Vector3>();
    [SerializeField] private float maxRange = 10f;
    [SerializeField, Range(0f, 10f)] private float minDis;

    private Transform origin;

    private void Awake()
    {
        origin = GetComponent<Transform>();
        RandomizeAllPoints();
    }
    // Get an element from the list.
    public Vector3 GetPOIAtIndex(int index)
    {
        if (!IsIndexValid(index)) 
            return Vector3.zero;
        
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
    private Vector3 RandomizePointPosition(int index)
    {
         PointsOfInterest[index] = GetRandomPositionInRange(transform.position, minDis, maxRange);

        return PointsOfInterest[index];
    }
    private Vector3 GetRandomPositionInRange(Vector3 center, float innerRadius, float outerRadius)
    {
        // pick a random direction on XZ
        Vector2 newpointPosition = new Vector2(Random.Range(innerRadius, outerRadius),Random.Range(innerRadius, outerRadius));//Random.insideUnitCircle.normalized;
        // pick a distance between inner and outer radius
        float distance = Random.Range(innerRadius, outerRadius);
        // build the final position
        Vector3 offset = new Vector3(newpointPosition.x, 0f, newpointPosition.y);  
        return offset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDis);
    }

}
