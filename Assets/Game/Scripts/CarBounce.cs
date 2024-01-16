using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBounce : MonoBehaviour
{
    public float suspensionDistance = 0.2f; // Adjust the suspension distance
    public float springForce = 5000f; // Adjust the spring force
    public float damper = 50f; // Adjust the damper

    private Rigidbody carRigidbody;
    private WheelCollider[] wheelColliders;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        wheelColliders = GetComponentsInChildren<WheelCollider>();

        foreach (var wheel in wheelColliders)
        {
            JointSpring spring = wheel.suspensionSpring;
            spring.spring = springForce;
            spring.damper = damper;
            spring.targetPosition = suspensionDistance;
            wheel.suspensionSpring = spring;
        }
    }

    void Update()
    {
        // Simulate the car bounce effect (e.g., in response to user input or other events)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyBounceForce();
        }
    }

    void ApplyBounceForce()
    {
        foreach (var wheel in wheelColliders)
        {
            wheel.motorTorque = 0f;
            carRigidbody.AddForceAtPosition(Vector3.up * 1000f, wheel.transform.position, ForceMode.Impulse);
        }
    }
}
