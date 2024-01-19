using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform Target; // The car's transform
    public float SmoothSpeed = 5.0f; // Smoothing factor for camera movement
    private Vector3 offset = new Vector3(0, 2.8f, -5.8f); // Camera offset relative to the car
    public Vector3 TargetPosition = new Vector3(0, 2.8f, -5.8f); //(0, 6.5f, -10f)
    [SerializeField] private GameObject FireWorkParticle;

    private void LoadTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Target = player.transform;
    }
    private void Start()
    {
        LoadTarget();
    }

    public void ResetCamAng()
    {
        StopAllCoroutines();
        offset = new Vector3(0, 2.8f, -5.8f);
    }
    void FixedUpdate()
    {
        LoadTarget();
        if (Target == null)
        {
            // If the target is not assigned, do nothing
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = Target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Make the camera look at the target (the car)
        transform.LookAt(Target);
    }
    public void ChangeAspect()
    {
        StopAllCoroutines();
        StartCoroutine(LerpCameraPosition());

        //offset = Vector3.Lerp(offset, new Vector3(0, 6.5f, -10),Time.fixedDeltaTime);
        //offset = new Vector3(0, 6.5f, -10);
    }
    IEnumerator LerpCameraPosition()
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = offset;

        while (timeElapsed < 3f)
        {
            offset = Vector3.Lerp(startingPosition, TargetPosition, timeElapsed / 3f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    public void OnNitro()
    {

    }
    public void ActiveFireWorkParticle()
    {
        FireWorkParticle.SetActive(true);
    }
    public void InActiveFireWorkParticle()
    {
        FireWorkParticle.SetActive(false);
    }
}
