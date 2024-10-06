using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watersim : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private GameObject selectedObject; // Reference to the currently selected object
    private Vector3 offset; // Offset between object and mouse position

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse drag and drop
        DragAndDrop();
    }

    void DragAndDrop()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // Perform a raycast to check if we hit an object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("healthcare")) // Check if the object has the tag 'healthcare'
                {
                    selectedObject = hit.collider.gameObject; // Select the object
                    // Calculate the offset between the object and mouse position
                    offset = selectedObject.transform.position - GetMouseWorldPos();
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null) // While dragging
        {
            // Update the object's position to follow the mouse, maintaining the offset
            selectedObject.transform.position = GetMouseWorldPos() + offset;
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            selectedObject = null; // Deselect the object
        }
    }

    // Convert mouse position to world position
    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(selectedObject.transform.position).z; // Maintain object's Z position

        return mainCamera.ScreenToWorldPoint(mousePoint); // Return mouse position in world coordinates
    }
}


