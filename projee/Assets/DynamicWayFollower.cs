using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWayFollower : MonoBehaviour
{
  
  public float speed = 5f;      // Vehicle speed
    public float detectionRadius = 10f; // Radius to detect nearby road objects
    private Transform targetWaypoint;   // Current target waypoint
    private Transform lastWaypoint;     // Last visited waypoint

    void Start()
    {
        // Find the first road segment when the vehicle spawns
        FindNextWaypoint();
    }

    void Update()
    {
        if (targetWaypoint != null)
        {
            // Move the vehicle towards the current waypoint
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotate the vehicle to face the next waypoint
            transform.LookAt(targetWaypoint.position);

            // Check if the vehicle has reached the waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
            {
                lastWaypoint = targetWaypoint; // Mark the waypoint as visited
                FindNextWaypoint();            // Get the next waypoint
            }
        }
    }

    void FindNextWaypoint()
    {
        // Find all road segments within a certain radius
        Collider[] nearbyRoads = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Default"));

        float closestDistance = Mathf.Infinity;
        Transform closestWaypoint = null;

        foreach (Collider road in nearbyRoads)
        {
            if (road.CompareTag("road") && road.transform != lastWaypoint)
            {
                float distance = Vector3.Distance(transform.position, road.transform.position);

                // Find the closest road object
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWaypoint = road.transform;
                }
            }
        }

        // Set the new target waypoint if one is found
        if (closestWaypoint != null)
        {
            targetWaypoint = closestWaypoint;
        }
        else
        {
            Debug.LogWarning("No more road waypoints found!"); // Handle edge case
        }
    }


}
