using UnityEngine;

public class ObjectClickDetector : MonoBehaviour
{
    public GameObject taskpanel;
    void Update()
    {
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            DetectObjectClick();
        }
    }

    void DetectObjectClick()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and check if it hit anything
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the collider exists before accessing it
            if (hit.collider != null)
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject.tag);
                if(hit.collider.gameObject.tag == "taskstuff")
                {
                    Debug.Log("taskstuff is clicked");
                    taskpanel.SetActive(true);
                }
                // Example action: deactivate the clicked object
                // hit.collider.gameObject.SetActive(false);
            }

            else
            {
                Debug.LogWarning("Raycast hit but no collider was found.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast did not hit any object.");
        }
    }
}
