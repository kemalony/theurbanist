using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public List<GameObject> cityElements; // Assign roads, buildings, healthcare facilities, water sources, etc.
    public GameObject taskNotificationPrefab; // 2D notification prefab that will appear over elements
    public float minTaskInterval = 5f;   // Minimum time between task generations
    public float maxTaskInterval = 15f;  // Maximum time between task generations
    public int baseTasksPerDay = 3;      // Base number of tasks generated per day

    private int currentDay = 1;          // Track the current day
    private int tasksGeneratedToday = 0; // Track tasks generated on the current day
    private int tasksToComplete = 0;     // Tasks needed to complete the day

    // Possible task templates with placeholders
    private List<string> taskTemplates = new List<string>()
    {
        "Fix {quantity} sections of {target}",
        "Repair the {target} at {detail}",
        "Inspect {quantity} units of {target}",
        "Clean up {quantity} tons of waste from {target}",
        "Upgrade {target} to level {quantity}",
        "Install {quantity} solar panels on {target}",
        "Resolve traffic congestion at {detail}",
        "Restock {quantity} supplies at {target}",
        "Conduct a safety inspection at {target}",
        "Inspect The Dome's air quality"
    };

    // Possible targets
    private List<string> targets = new List<string>()
    {
        "road",
        "building",
        "water source",
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
        StartCoroutine(DayCycle());
    }

    // Day cycle coroutine
    IEnumerator DayCycle()
    {
        while (true)
        {
            Debug.Log($"Day {currentDay} started.");

            // Calculate how many tasks need to be generated today
            tasksToComplete = baseTasksPerDay + (currentDay - 1); // Increase tasks with each day
            tasksGeneratedToday = 0;

            Debug.Log($"Tasks to complete today: {tasksToComplete}");

            // Generate tasks for the day
            while (tasksGeneratedToday < tasksToComplete)
            {
                float taskInterval = Random.Range(minTaskInterval, maxTaskInterval);

                Debug.Log($"Next task in {taskInterval} seconds");

                // Countdown for task generation
                for (float countdown = taskInterval; countdown > 0; countdown--)
                {
                    Debug.Log($"Time until next task: {countdown} seconds");
                    yield return new WaitForSeconds(1f);
                }

                // Generate the task
                CityTask newTask = GenerateTask();
                GameObject targetElement = GetRandomCityElement(newTask.target);
                if (targetElement != null)
                {
                    CreateTaskNotification(targetElement, newTask);
                }

                tasksGeneratedToday++;
                Debug.Log($"Task Generated: {newTask.description}");
                Debug.Log($"Tasks remaining today: {tasksToComplete - tasksGeneratedToday}");
            }

            // End of the day
            Debug.Log($"Day {currentDay} completed!");

            // Wait for a new day (adjust the wait time if needed)
            yield return new WaitForSeconds(10f);
            
            // Move to the next day
            currentDay++;
        }
    }

    // Generates a randomized task
    CityTask GenerateTask()
    {
        string template = taskTemplates[Random.Range(0, taskTemplates.Count)];
        string target = targets[Random.Range(0, targets.Count)];
        int quantity = Random.Range(1, 5); // Adjust range as needed
        string detail = details[Random.Range(0, details.Count)];

        string description = template.Replace("{quantity}", quantity.ToString())
                                     .Replace("{target}", target)
                                     .Replace("{detail}", detail);

        return new CityTask(description, target, quantity, detail);
    }

    // Finds a random city element matching the task target
    GameObject GetRandomCityElement(string targetType)
    {
        List<GameObject> matchingElements = new List<GameObject>();

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
            return null;
        }
    }

    // Creates a task notification over the target element
    void CreateTaskNotification(GameObject targetElement, CityTask task)
    {
        GameObject notification = Instantiate(taskNotificationPrefab);
        notification.transform.SetParent(targetElement.transform, false);
        notification.transform.localPosition = new Vector3(0, 5f, 0); // Adjust Y value as needed
        notification.GetComponentInChildren<UnityEngine.UI.Text>().text = task.description;
    }
}

// Custom task class
public class CityTask
{
    public string description;
    public string target;
    public int quantity;
    public string detail;

    public CityTask(string description, string target, int quantity, string detail)
    {
        this.description = description;
        this.target = target;
        this.quantity = quantity;
        this.detail = detail;
    }
}
