using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI components
using System.Collections.Generic; // For using List
using UnityEngine.SceneManagement;


public class BottleSimulation : MonoBehaviour
{
    // Variables to store target position and rotation
    private Vector3 targetPosition = new Vector3(4.2f, 1.3f, 10.7f);
    private Vector3 targetRotation = new Vector3(0, 0, -90);

    // Start position and rotation
    private Vector3 startPosition = new Vector3(3.8f, 1.09f, 10.7f);
    private Vector3 startRotation = new Vector3(0, 0, 0);
    public GameObject panell;

    // Duration for the animation
    public float duration = 2.0f;

    // Water bottle material
    public Material waterBottleMaterial;

    // UI notification
    public Text notificationText;

    // Output text for water quality results
    public Text resultText; // New UI Text element for results

    // List of potential water quality results
    private List<string> waterQualityLevels = new List<string>
    {
        "Heavy Metals: Low levels detected.",
        "Nitrates: Safe levels detected.",
        "Chlorine: Acceptable concentration."
    };

    // List of colors corresponding to the levels
    private Dictionary<string, Color> waterQualityColors = new Dictionary<string, Color>
    {
        { "Heavy Metals", new Color(1.0f, 1.0f, 0.5f) }, // Pale yellow
        { "Nitrates", new Color(0.0f, 1.0f, 0.0f) }, // Green for safe levels
        { "Chlorine", new Color(0.0f, 0.0f, 1.0f) } // Blue for acceptable concentration
    };

    private Queue<string> levelsToTest; // Queue to keep track of levels to test

    // Public variable for the GameObject to move
    public GameObject objectToMove;

    public void start()
    {
        // Set the panel inactive at the start
        panell.SetActive(false);

        // Initialize the queue with the water quality levels
        levelsToTest = new Queue<string>(waterQualityLevels);
        
        // Start the simulation
        StartCoroutine(SimulatePourProcess());
    }

    IEnumerator SimulatePourProcess()
    {
        // Test each water quality level at least once
        while (levelsToTest.Count > 0)
        {
            // Pour and check for contaminants
            yield return StartCoroutine(PourAndCheckQuality());

            // Return to the original position
            yield return StartCoroutine(ReturnToOriginalPosition());
        }

        // Final notification
        resultText.text = "All levels are safe!";
        
        // Set the panel active after simulation is complete
        SceneManager.LoadSceneAsync("gamescreen");

    }

    IEnumerator PourAndCheckQuality()
    {
        // Move and rotate the specified object to the target position
        float elapsedTime = 0f;

        // Initial position and rotation
        objectToMove.transform.position = startPosition;
        objectToMove.transform.eulerAngles = startRotation;

        while (elapsedTime < duration)
        {
            // Lerp position and rotation for smooth transition
            objectToMove.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            objectToMove.transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are set
        objectToMove.transform.position = targetPosition;
        objectToMove.transform.eulerAngles = targetRotation;

        // Check the next water quality level to test
        string selectedLevel = levelsToTest.Dequeue(); // Dequeue the next level to test

        // Notify user and change color based on the selected level
        NotifyUser(selectedLevel);
        waterBottleMaterial.color = waterQualityColors[selectedLevel.Split(':')[0]];

        // Wait for a moment to display the result
        yield return new WaitForSeconds(2f);
    }

    void NotifyUser(string message)
    {
        // Update the UI notification text
        notificationText.text = message;

        // Optionally, you can implement a timer to clear the notification after a few seconds
        StartCoroutine(ClearNotification());
    }

    IEnumerator ClearNotification()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        notificationText.text = ""; // Clear the notification
    }

    IEnumerator ReturnToOriginalPosition()
    {
        float elapsedTime = 0f;

        // Move back to the original position
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Lerp position and rotation for smooth return
            objectToMove.transform.position = Vector3.Lerp(targetPosition, startPosition, t);
            objectToMove.transform.eulerAngles = Vector3.Lerp(targetRotation, startRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is back at the starting position and rotation
        objectToMove.transform.position = startPosition;
        objectToMove.transform.eulerAngles = startRotation;
    }
}
