using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementfps : MonoBehaviour
{

   public float moveSpeed = 5f;  // Movement speed

    void Update()
    {
        // Get input from the WASD keys
        float moveX = Input.GetAxis("Horizontal"); // A (left) and D (right)
        float moveZ = Input.GetAxis("Vertical");   // W (forward) and S (backward)

        // Create a movement vector
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Move the object
        transform.Translate(move * moveSpeed * Time.deltaTime);
    }


}
