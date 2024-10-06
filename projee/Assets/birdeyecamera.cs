using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdeyecamera : MonoBehaviour
{
    // Pan settings
    public float panSpeed = 20f;            // Speed of panning
    public float panBorderThickness = 10f;  // Thickness of screen border for panning
    public Vector2 panLimit;                 // Limits for panning movement

    // Zoom settings
    public float scrollSpeed = 20f;         // Speed of zooming
    public float minY = 10f;                // Minimum zoom level
    public float maxY = 100f;               // Maximum zoom level

    void Update()
    {
        Vector3 pos = transform.position;

        // --- Pan camera using WASD or Arrow keys ---
        if (Input.GetKey("s") || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("w") || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
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
    }
}
