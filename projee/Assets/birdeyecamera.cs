using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdeyecamera : MonoBehaviour
{

    // Pan settings
    public float panSpeed = 20f;            // Speed of panning
    public float panBorderThickness = 10f;  // Thickness of screen border for panning
    public Vector2 panLimit;                // Limits for panning movement

    // Zoom settings
    public float scrollSpeed = 20f;         // Speed of zooming
    public float minY = 10f;                // Minimum zoom level
    public float maxY = 100f;               // Maximum zoom level

    // Drag settings
    public float dragSpeed = 2f;            // Speed for dragging camera
    private Vector3 dragOrigin;             // Initial point for dragging
    private bool isDragging = false;        // Track drag state

    // Rotation settings
    public float rotateSpeed = 50f;         // Speed of rotation
    private bool doRotation = false;        // Toggle rotation mode

    void Update()
    {
        Vector3 pos = transform.position;

        // --- Pan camera using WASD or Arrow keys ---
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        // --- Pan with mouse near the screen edges ---
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // --- Zoom in/out using the scroll wheel ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);  // Clamp zoom within min/max limits

        // --- Limit camera position based on defined boundaries ---
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Apply camera position after movement
        transform.position = pos;

        // --- Drag camera using right mouse button ---
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;  // Set origin when drag starts
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;  // End drag
        }

        if (isDragging)
        {
            Vector3 difference = dragOrigin - Input.mousePosition;

            // Move camera in the opposite direction of the drag
            Vector3 move = new Vector3(difference.x * dragSpeed * Time.deltaTime, 0, difference.y * dragSpeed * Time.deltaTime);

            // Update camera position
            transform.Translate(move, Space.World);

            // Update drag origin for smooth movement
            dragOrigin = Input.mousePosition;
        }

        // --- Rotate camera with middle mouse button ---
        if (Input.GetMouseButtonDown(2))  // Middle mouse button pressed
        {
            doRotation = true;
        }
        if (Input.GetMouseButtonUp(2))  // Middle mouse button released
        {
            doRotation = false;
        }

        if (doRotation)
        {
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");

            // Rotate around the Y-axis for horizontal movement
            transform.Rotate(Vector3.up, rotateHorizontal * rotateSpeed * Time.deltaTime, Space.World);
        }
    }

}
