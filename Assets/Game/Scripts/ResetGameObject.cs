using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameObject : Singleton<ResetGameObject>
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Called when the script instance is being loaded
    void Start()
    {
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Function to reset the GameObject and its children
    public void ResetGameObjectAndChildren()
    {
        // Reset the main GameObject
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Iterate through all child objects
        foreach (Transform child in transform)
        {
            // Reset the child's position and rotation
            child.position = child.gameObject.GetComponent<ResetGameObject>().initialPosition;
            child.rotation = child.gameObject.GetComponent<ResetGameObject>().initialRotation;

            // You may need to reset other properties or components depending on your game
            // For example, resetting a script attached to the child GameObject
            // child.gameObject.GetComponent<YourCustomScript>().ResetFunction();
        }
    }
}
