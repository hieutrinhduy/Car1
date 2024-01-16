using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Sticked");
            // Disable gravity and set the car's rotation to match the wall
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            CarController.Ins.TurnOffGravity();

            // Calculate the rotation to align with the wall
            Vector3 contactNormal = collision.contacts[0].normal;
            Quaternion rotation = Quaternion.FromToRotation(transform.up, contactNormal) * transform.rotation;

            // Apply the rotation
            transform.rotation = rotation;
        }
    }
}