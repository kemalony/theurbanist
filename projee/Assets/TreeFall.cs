using UnityEngine;
using System.Collections.Generic;

public class TreeFall : MonoBehaviour
{
    // Public variable for the clickable object prefab (e.g., a button or visual cue)
    public GameObject clickableObjectPrefab;

    // List to store the original positions and rotations of trees
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();

    // Dictionary to store a counter for each tree
    private Dictionary<GameObject, int> treeCrashCounter = new Dictionary<GameObject, int>();

    // Public variable to limit crashes to 3
    public int maxCrashes = 3;
    private int a=0;
    void Start()
    {
        // Find all objects tagged as "Tree"
        GameObject[] trees = GameObject.FindGameObjectsWithTag("tree");

        foreach (GameObject tree in trees)
        {
            // Store the original position and rotation
            originalPositions[tree] = tree.transform.position;
            originalRotations[tree] = tree.transform.rotation;

            // Initialize the crash counter for each tree
            treeCrashCounter[tree] = 0;

            // Spawn trees on the nearest road, up to the maxCrashes limit
            for (int i = 0; i < maxCrashes; i++)
            {
                // Ensure the tree falls only if it hasn't reached the crash limit
                if (treeCrashCounter[tree] < maxCrashes)
                {
                    SpawnTreeOnNearestRoad(tree);
                }
            }
        }
    }

    void SpawnTreeOnNearestRoad(GameObject tree)
    {
        // Find all objects tagged as "Road"
        GameObject[] roads = GameObject.FindGameObjectsWithTag("road");

        // Find the nearest road to the tree
        GameObject nearestRoad = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject road in roads)
        {
            float distance = Vector3.Distance(tree.transform.position, road.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRoad = road;
            }
        }
 
        // If a road is found, spawn the tree on a random position on that road
        if (nearestRoad != null && a<4)
        {
            a+=1;
            // Randomize the position on the road (slightly within the bounds)

            Vector3 randomPositionOnRoad = nearestRoad.transform.position + new Vector3(
                Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)
            );

            // Set the tree's position and rotate it by 90 degrees (as if it's fallen)
            tree.transform.position = randomPositionOnRoad;
            tree.transform.rotation = Quaternion.Euler(0, 0, 90);

            // Increment the crash counter for the tree
            treeCrashCounter[tree]++;

            // Instantiate the clickable object and place it near the fallen tree
            GameObject clickableObject = Instantiate(clickableObjectPrefab);
            clickableObject.transform.position = tree.transform.position + new Vector3(0, 1.0f, 0); // Position the clickable above the tree

            // Add the click handler directly on this object
            clickableObject.AddComponent<BoxCollider>(); // Make sure the object has a collider to detect clicks
            clickableObject.AddComponent<ClickableObject>().Init(tree, originalPositions[tree], originalRotations[tree], clickableObject, treeCrashCounter);
        }
    }

    // Internal class for handling the click event
    public class ClickableObject : MonoBehaviour
    {
        private GameObject tree; // The tree that should return to its original position
        private Vector3 originalPosition; // The tree's original position
        private Quaternion originalRotation; // The tree's original rotation
        private GameObject clickableObject; // The clickable object itself
        private Dictionary<GameObject, int> treeCrashCounter; // Reference to the tree's crash counter

        // Initialize the object with the needed data
        public void Init(GameObject tree, Vector3 originalPosition, Quaternion originalRotation, GameObject clickableObject, Dictionary<GameObject, int> treeCrashCounter)
        {
            this.tree = tree;
            this.originalPosition = originalPosition;
            this.originalRotation = originalRotation;
            this.clickableObject = clickableObject;
            this.treeCrashCounter = treeCrashCounter;
        }

        void OnMouseDown()
        {
            // Return the tree to its original position and rotation
            tree.transform.position = originalPosition;
            tree.transform.rotation = originalRotation;

            // Decrease the crash counter for the tree
            treeCrashCounter[tree]--;

            // Destroy the clickable object (since the tree is back in its place)
            Destroy(clickableObject);
        }
    }
}
