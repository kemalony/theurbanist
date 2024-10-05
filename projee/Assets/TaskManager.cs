using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    // List of city elements to assign tasks to
    public List<GameObject> cityElements; // Assign roads, buildings, healthcare facilities, water sources, etc.
    public GameObject taskNotificationPrefab; // 2D notification prefab that will appear over elements
    public float minTaskInterval = 5f;   // Minimum time between task generations
    public float maxTaskInterval = 15f;  // Maximum time between task generations

    // Possible task templates with placeholders
    private List<string> taskTemplates = new List<string>()
    {
        "Fix {quantity} sections of {target}",
        "Repair the {target} at {detail}",
        "Inspect {quantity} units of {target}",
        "Provide healthcare to {quantity} people at {detail}",
        "Clean up {quantity} tons of waste from {target}",
        "Upgrade {target} to level {quantity}",
        "Install {quantity} solar panels on {target}",
        "Resolve traffic congestion at {detail}",
        "Restock {quantity} supplies at {target}",
        "Conduct a safety inspection at {target}"
    };

    // Possible targets
    private List<string> targets = new List<string>()
    {
        "road",
        "building",
        "water source",
        "hospital",
        "school",
        "park",
        "power plant",
        "residential area",
        "commercial district",
        "industrial zone"
    };

    // Possible details (could be locations or specific names)
    private List<string> details = new List<string>()
    {
        "Main Street",
        "Downtown",
        "Riverside",
        "Old Town",
        "East Side",
        "West End",
        "North Gate",
        "South Park",
        "Harbor Area",
        "Central Plaza"
    };

    void Start()
    {
        StartCoroutine(GenerateRandomTasks());
    }

    // Randomly generates tasks at intervals
    IEnumerator GenerateRandomTasks()
    {
        while (true)
        {
            // Wait for a random time before generating the next task
            float taskInterval = Random.Range(minTaskInterval, maxTaskInterval);
            yield return new WaitForSeconds(taskInterval);

            // Generate a random task
            Task newTask = GenerateTask();

            // Assign the task to a random city element
            GameObject targetElement = GetRandomCityElement(newTask.target);
            if (targetElement != null)
            {
                // Display the task notification over the target element
                CreateTaskNotification(targetElement, newTask);
            }
        }
    }

    // Generates a randomized task
    Task GenerateTask()
    {
        // Select a random template
        string template = taskTemplates[Random.Range(0, taskTemplates.Count)];

        // Randomize target, quantity, and detail
        string target = targets[Random.Range(0, targets.Count)];
        int quantity = Random.Range(1, 100); // Adjust range as needed
        string detail = details[Random.Range(0, details.Count)];

        // Replace placeholders in the template
        string description = template.Replace("{quantity}", quantity.ToString())
                                     .Replace("{target}", target)
                                     .Replace("{detail}", detail);

        // Create and return the new task
        return new Task(description, target, quantity, detail);
    }

    // Finds a random city element matching the task target
    GameObject GetRandomCityElement(string targetType)
    {
        List<GameObject> matchingElements = new List<GameObject>();

        // Assuming each city element has a tag matching its type
        foreach (GameObject element in cityElements)
        {
            if (element.CompareTag(targetType))
            {
                matchingElements.Add(element);
            }
        }

        if (matchingElements.Count > 0)
        {
            int index = Random.Range(0, matchingElements.Count);
            return matchingElements[index];
        }
        else
        {
            return null; // No matching element found
        }
    }

    // Creates a task notification over the target element
    void CreateTaskNotification(GameObject targetElement, Task task)
    {
        // Instantiate the notification prefab
        GameObject notification = Instantiate(taskNotificationPrefab);

        // Set the notification as a child of the target element
        notification.transform.SetParent(targetElement.transform, false);

        // Position the notification above the target element
        notification.transform.localPosition = new Vector3(0, 5f, 0); // Adjust Y value as needed

        // Update the notification text with the task description
        notification.GetComponentInChildren<UnityEngine.UI.Text>().text = task.description;
    }
}
