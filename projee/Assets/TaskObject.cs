using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour
{
    private CityTask assignedTask;
    private TaskManager taskManager; // Reference to TaskManager
    private Slider Score;
    private Slider Memnun;
    private Slider Para;
    private Slider Some;

    private void Start()
    {
        // Find the sliders in the scene by their names
        Score = GameObject.Find("Score").GetComponent<Slider>();
        Memnun = GameObject.Find("Memnun").GetComponent<Slider>();
        Para = GameObject.Find("Para").GetComponent<Slider>();
        Some = GameObject.Find("Dome").GetComponent<Slider>();

        // Load saved slider values from PlayerPrefs
        LoadSliderValues();
    }

    private void OnDestroy()
    {
        // Save slider values when the object is destroyed (e.g., when changing scenes)
        SaveSliderValues();
    }

    // Initializes the task object with its assigned task and the TaskManager reference
    public void Initialize(CityTask task, TaskManager manager)
    {
        assignedTask = task;
        taskManager = manager; // Set the reference to TaskManager
    }

    // This method will be called when the player interacts with the object (e.g., clicking it)
    public void OnMouseDown() // Detect mouse click or touch
    {
        if (assignedTask.description.Contains("water"))
        {
            Memnun.value += 5;
            Some.value += 3;

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
                Memnun.value += 2;
                Some.value += 1;
                Para.value -= 1;
                Score.value = ((Para.value)+(Memnun.value)+(Some.value))/3;
                // Save the updated slider values
                SaveSliderValues();

                Destroy(gameObject); // Destroy this object to simulate it being "cleaned" or "repaired"
            }
        }
    }

    // Save slider values to PlayerPrefs
    private void SaveSliderValues()
    {
        PlayerPrefs.SetFloat("Score", Score.value);
        PlayerPrefs.SetFloat("Memnun", Memnun.value);
        PlayerPrefs.SetFloat("Para", Para.value);
        PlayerPrefs.SetFloat("Some", Some.value);
        PlayerPrefs.Save(); // Save the values to disk
    }

    // Load slider values from PlayerPrefs
    private void LoadSliderValues()
    {
        // Check if the keys exist before loading
    
    }
}