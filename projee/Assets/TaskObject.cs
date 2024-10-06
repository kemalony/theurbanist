using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskObject : MonoBehaviour
{
    private CityTask assignedTask;
    private TaskManager taskManager; // Reference to TaskManager

    // Initializes the task object with its assigned task and the TaskManager reference
    public void Initialize(CityTask task, TaskManager manager)
    {
        assignedTask = task;
        taskManager = manager; // Set the reference to TaskManager
    }

    // This method will be called when the player interacts with the object (e.g., clicking it)
    public void OnMouseDown() // Detect mouse click or touch
    {
        if(assignedTask.description.Contains("water"))
        {
            taskManager.CheckTaskCompletion(assignedTask);
            SceneManager.LoadScene("Laboratory Scene");
        }
        else
        {
        // Reduce the remaining related objects in the task
        if (assignedTask.relatedObjectsRemaining > 0)
        {
            assignedTask.relatedObjectsRemaining -= 1;
            Debug.Log($"Task object interacted. Remaining: {assignedTask.relatedObjectsRemaining}");

            // Notify the TaskManager to check for task completion
            taskManager.CheckTaskCompletion(assignedTask);

            Destroy(gameObject); // Destroy this object to simulate it being "cleaned" or "repaired"
        }
        }  
    }
}
