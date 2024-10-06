using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public List<GameObject> cityElements; // List of city elements like roads, buildings, water sources

    public GameObject taskbuttonn; // Button prefab for task display
    public GameObject trashBinPrefab; 
    public GameObject watertaskPrefab;// Prefab for a trash bin for clean-up tasks
    public GameObject repairItemPrefab; // Prefab for repair items for repair tasks
    public Transform taskSpawnParent; // Parent object to spawn task-related objects under
    public float minTaskInterval = 0.2f;   // Minimum time between task generations
    public float maxTaskInterval = 1f;  // Maximum time between task generations
    public int baseTasksPerDay = 3;      // Base number of tasks generated per day
    public Text currentdayy; 
    private int currentDay = 1; // Current day tracker
    private int tasksGeneratedToday = 0; // Number of tasks generated for the current day
    private int tasksToComplete = 0;     // Tasks needed to complete the day
    private List<CityTask> activeTasks = new List<CityTask>(); // List of currently active tasks

    // Task templates with placeholders
    private List<string> taskTemplates = new List<string>()
    {
         "Repair the {target}", 
        "Clean up {quantity} tons of waste from {target}",
        "Check the pureification of the water sources",
    };

    // Possible targets
    private List<string> targets = new List<string>()
    {
        "road"//"building", "water source", "stadium", "factory", "windmill"
    };

    // Possible details (could be locations or specific names)
    private List<string> details = new List<string>()
    {
        "Main Street", "Downtown", "Riverside", "Old Town", "East Side", "West End"
    };

    void Start()
    {
        currentdayy.text = currentDay + "  ";

        StartCoroutine(DayCycle());
    }

    // Day cycle coroutine for generating tasks
    IEnumerator DayCycle()
    {
        while (true)
        {
            tasksToComplete = baseTasksPerDay + (currentDay - 1); // Increase tasks each day
            tasksGeneratedToday = 0;

            while (tasksGeneratedToday < tasksToComplete)
            {
                float taskInterval = Random.Range(minTaskInterval, maxTaskInterval);
                Debug.Log($"Next task will be generated in: {taskInterval} seconds."); // Countdown log
                yield return new WaitForSeconds(taskInterval); // Wait for next task generation

                CityTask newTask = GenerateTask();
                activeTasks.Add(newTask); // Add to active tasks list
                tasksGeneratedToday++;
            }

            Debug.Log($"All tasks for day {currentDay} generated.");
            currentdayy.text = "Day "+ currentDay + "  ";
            // Wait for a new day
            yield return new WaitForSeconds(10f);
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
        
        // Create the task button
        GameObject taskBtn = Instantiate(taskbuttonn);
        taskBtn.SetActive(true);
        taskBtn.transform.SetParent(taskbuttonn.transform.parent);
        taskBtn.transform.position = new Vector3(taskbuttonn.transform.position.x, taskbuttonn.transform.position.y - 10, taskbuttonn.transform.position.z);
        
        // Set the task description in the button's text component
        Text textComponent = taskBtn.GetComponentInChildren<Text>();
        textComponent.text = description;
        
        // Find the "ok" button in the task button and set its onClick listener
        Button okButton = taskBtn.transform.Find("ok").GetComponent<Button>();
        okButton.onClick.AddListener(() => AcceptTask(new CityTask(description, target, quantity, detail), taskBtn));
        
        return new CityTask(description, target, quantity, detail);
    }

    // Handles task acceptance
    void AcceptTask(CityTask task, GameObject taskBtn)
    {
        Debug.Log($"Task accepted: {task.description}");
        taskBtn.SetActive(false); // Hide the task button

        // Spawn related objects based on the task type
        if (task.description.Contains("Clean up"))
        {
            SpawnCleanupItems(task);
        }
        else if (task.description.Contains("Repair"))
        {
            SpawnRepairItems(task);
        }
         else if (task.description.Contains("water"))
        {
            watercleanup(task);
        }
        // Monitor the task completion
        StartCoroutine(MonitorTaskCompletion(task));
    }
  void watercleanup(CityTask task)
  {
    GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(GetTagForTarget("water source"));
     if (targetObjects.Length == 0)
    {
        Debug.LogWarning($"No objects found with tag {GetTagForTarget(task.target)} for task: {task.description}");
        return;
    }
      // Spawn trash bins on the found objects
    for (int i = 0; i < task.quantity; i++)
    {
        GameObject targetObject = targetObjects[Random.Range(0, targetObjects.Length)];

        // Spawn trash bin at the position of the target object
        GameObject wate = Instantiate(watertaskPrefab, targetObject.transform.position, Quaternion.identity, taskSpawnParent);
        wate.GetComponent<TaskObject>().Initialize(task, this); // Pass TaskManager reference
    }

  }
    // Spawns cleanup items (e.g., trash bins)
   void SpawnCleanupItems(CityTask task)
{
    GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(GetTagForTarget(task.target));

    if (targetObjects.Length == 0)
    {
        Debug.LogWarning($"No objects found with tag {GetTagForTarget(task.target)} for task: {task.description}");
        return;
    }

    // Spawn trash bins on the found objects
    for (int i = 0; i < task.quantity; i++)
    {
        GameObject targetObject = targetObjects[Random.Range(0, targetObjects.Length)];

        // Spawn trash bin at the position of the target object
        GameObject trashBin = Instantiate(trashBinPrefab, targetObject.transform.position, Quaternion.identity, taskSpawnParent);
        trashBin.GetComponent<TaskObject>().Initialize(task, this); // Pass TaskManager reference
    }
}

// Spawns repair items (e.g., repair tools) on tagged objects
void SpawnRepairItems(CityTask task)
{
    GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("road");

    if (targetObjects.Length == 0)
    {
        Debug.LogWarning($"No objects found with tag {GetTagForTarget(task.target)} for task: {task.description}");
        return;
    }

    // Spawn repair items on the found objects
    for (int i = 0; i < task.quantity; i++)
    {
        GameObject targetObject = targetObjects[Random.Range(0, targetObjects.Length)];

        // Spawn repair item at the position of the target object
        GameObject repairItem = Instantiate(repairItemPrefab, targetObject.transform.position, Quaternion.identity, taskSpawnParent);
        repairItem.GetComponent<TaskObject>().Initialize(task, this); // Pass TaskManager reference
    }
}
// Method to check if a task is complete
public void CheckTaskCompletion(CityTask task)
{
    if (task.relatedObjectsRemaining <= 0)
    {
        Debug.Log($"Task completed: {task.description}");
        activeTasks.Remove(task); // Remove task from active tasks list
    }
}

    // Helper method to get the tag based on the task target
    string GetTagForTarget(string target)
    {
        switch (target)
        {
            case "road":
                return "road"; // Use the tag "road"
            case "building":
                return "apartman"; // Use the tag "apartman"
            case "water source":
                return "water source"; // Use the tag "water source"
            case "stadium":
                return "stadium"; // Use the tag "stadium"
            case "factory":
                return "fabrika"; // Use the tag "fabrika"
            case "windmill":
                return "windmill"; // Use the tag "windmill"
            case "air capsule":
                return "air";
            default:
                Debug.LogWarning($"No tag mapped for target: {target}");
                return "";
        }
    }

    // Monitors if the task has been completed (all related objects interacted with)
    IEnumerator MonitorTaskCompletion(CityTask task)
    {
        while (task.relatedObjectsRemaining > 0)
        {
            yield return null; // Wait until task completion
        }
        Debug.Log($"Task completed: {task.description}");
        activeTasks.Remove(task); // Remove task from active tasks list
    }

    // Get random task position (you can improve this logic based on your map)
    Vector3 GetRandomTaskPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)); // Example coordinates
    }
}

// Custom task class
public class CityTask
{
    public string description;
    public string target;
    public int quantity;
    public string detail;
    public int relatedObjectsRemaining; // Tracks the number of spawned task-related objects

    public CityTask(string description, string target, int quantity, string detail)
    {
        this.description = description;
        this.target = target;
        this.quantity = quantity;
        this.detail = detail;
        this.relatedObjectsRemaining = quantity; // Initialize remaining objects
    }
}


