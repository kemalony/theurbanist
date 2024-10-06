using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject tree; // The tree that should return to its original position
    public Vector3 originalPosition; // The tree's original position
    public Quaternion originalRotation; // The tree's original rotation

    // This function is called when the clickable object is clicked
    void OnMouseDown()
    {
        // Return the tree to its original position and rotation
        tree.transform.position = originalPosition;
        tree.transform.rotation = originalRotation;

        // Destroy this clickable object (since the tree is back in its place)
        Destroy(gameObject);
    }
}
