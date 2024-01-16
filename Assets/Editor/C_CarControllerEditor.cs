using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(C_CarController))]
public class C_CarControllerEditor : Editor
{

    C_CarController carScript;

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        carScript = (C_CarController)target;

        if (GUILayout.Button("Create WheelColliders"))
            CreateWheelColliders();

        if (PrefabUtility.GetCorrespondingObjectFromSource(carScript.gameObject) == null)
        {

            if (GUILayout.Button("Create Prefab"))
                CreatePrefab();

        }
        else
        {

            if (GUILayout.Button("Save Prefab"))
                SavePrefab();

        }

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();

    }

    void CreatePrefab()
    {

        PrefabUtility.SaveAsPrefabAssetAndConnect(carScript.gameObject, "Assets/Resources/Vehicles/" + carScript.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Created prefab for" + carScript.gameObject.name + ".prefab" + " in Assets/Resources/Vehicles/");

    }

    void SavePrefab()
    {

        PrefabUtility.SaveAsPrefabAssetAndConnect(carScript.gameObject, "Assets/Resources/Vehicles/" + carScript.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Saved prefab for" + carScript.gameObject.name + ".prefab" + " in Assets/Resources/Vehicles/");

    }

    /// <summary>
    /// Creates the wheel colliders.
    /// </summary>
    private void CreateWheelColliders()
    {

        Debug.Log("Creating Wheelcolliders");

        WheelCollider[] oldWheelColliders = carScript.transform.GetComponentsInChildren<WheelCollider>();

        foreach (WheelCollider item in oldWheelColliders)    
            DestroyImmediate(item.gameObject);

        // Holding default rotation.
        Quaternion currentRotation = carScript.transform.rotation;

        // Resetting rotation.
        carScript.transform.rotation = Quaternion.identity;

        // Creating a new gameobject called Wheel Colliders for all Wheel Colliders, and parenting it to this gameobject.
        GameObject WheelColliders = new GameObject("Wheel Colliders");
        WheelColliders.transform.SetParent(carScript.transform, false);
        WheelColliders.transform.localRotation = Quaternion.identity;
        WheelColliders.transform.localPosition = Vector3.zero;
        WheelColliders.transform.localScale = Vector3.one;

        for (int i = 0; i < carScript.wheels.Length; i++){

            if (carScript.wheels[i].wheelModel != null){

                GameObject wheelcollider = new GameObject(carScript.wheels[i].wheelModel.transform.name);

                wheelcollider.transform.position = C_GetBounds.GetBoundsCenter(carScript.wheels[i].wheelModel.transform);
                wheelcollider.transform.rotation = carScript.transform.rotation;
                wheelcollider.transform.name = carScript.wheels[i].wheelModel.transform.name;
                wheelcollider.transform.SetParent(WheelColliders.transform);
                wheelcollider.transform.localScale = Vector3.one;
                wheelcollider.AddComponent<WheelCollider>();
                carScript.wheels[i].wheelCollider = wheelcollider.GetComponent<WheelCollider>();

                Bounds biggestBound = new Bounds();
                Renderer[] renderers = carScript.wheels[i].wheelModel.GetComponentsInChildren<Renderer>();

                foreach (Renderer render in renderers)
                {
                    if (render.bounds.size.z > biggestBound.size.z)
                        biggestBound = render.bounds;
                }

                wheelcollider.GetComponent<WheelCollider>().radius = (biggestBound.extents.y) / carScript.transform.localScale.y;

                JointSpring spring = wheelcollider.GetComponent<WheelCollider>().suspensionSpring;

                spring.spring = 35000f;
                spring.damper = 3500f;
                spring.targetPosition = .5f;

                wheelcollider.GetComponent<WheelCollider>().suspensionSpring = spring;
                wheelcollider.GetComponent<WheelCollider>().suspensionDistance = .2f;
                wheelcollider.GetComponent<WheelCollider>().forceAppPointDistance = 0f;
                wheelcollider.GetComponent<WheelCollider>().mass = 100f;
                wheelcollider.GetComponent<WheelCollider>().wheelDampingRate = 1f;

                WheelFrictionCurve sidewaysFriction;
                WheelFrictionCurve forwardFriction;

                sidewaysFriction = wheelcollider.GetComponent<WheelCollider>().sidewaysFriction;
                forwardFriction = wheelcollider.GetComponent<WheelCollider>().forwardFriction;

                forwardFriction.extremumSlip = .4f;
                forwardFriction.extremumValue = 1;
                forwardFriction.asymptoteSlip = .8f;
                forwardFriction.asymptoteValue = .5f;
                forwardFriction.stiffness = 2f;

                sidewaysFriction.extremumSlip = .2f;
                sidewaysFriction.extremumValue = 1;
                sidewaysFriction.asymptoteSlip = .5f;
                sidewaysFriction.asymptoteValue = .75f;
                sidewaysFriction.stiffness = 2f;

                wheelcollider.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction;
                wheelcollider.GetComponent<WheelCollider>().forwardFriction = forwardFriction;

            }
            else
            {

                Debug.LogError("Wheel model of the " + carScript.transform.name + " is missing! Select all wheel models before creating wheelcolliders!");

            }

        }

        carScript.transform.rotation = currentRotation;

        Debug.Log("Created Wheelcolliders");

        //    // Creating a list for all wheel models.
        //    List<Transform> allWheelModels = new List<Transform>();
        //    allWheelModels.Add(FrontLeftWheelTransform); allWheelModels.Add(FrontRightWheelTransform); allWheelModels.Add(RearLeftWheelTransform); allWheelModels.Add(RearRightWheelTransform);

        //    // If we have additional rear wheels, add them too.
        //    if (ExtraRearWheelsTransform.Length > 0 && ExtraRearWheelsTransform[0])
        //    {

        //        foreach (Transform t in ExtraRearWheelsTransform)
        //            allWheelModels.Add(t);

        //    }

        //    // If we don't have any wheelmodels, throw an error.
        //    if (allWheelModels != null && allWheelModels[0] == null)
        //    {

        //        Debug.LogError("You haven't choosen your Wheel Models. Please select all of your Wheel Models before creating Wheel Colliders. Script needs to know their sizes and positions, aye?");
        //        return;

        //    }

        //    //asdwqaeda

        //    // Creating WheelColliders.
        //    foreach (Transform wheel in allWheelModels)
        //    {

        //        // weasda

        //    }

        //    RCC_WheelCollider[] allWheelColliders = new RCC_WheelCollider[allWheelModels.Count];
        //    allWheelColliders = GetComponentsInChildren<RCC_WheelCollider>();

        //    FrontLeftWheelCollider = allWheelColliders[0];
        //    FrontRightWheelCollider = allWheelColliders[1];
        //    RearLeftWheelCollider = allWheelColliders[2];
        //    RearRightWheelCollider = allWheelColliders[3];

        //    ExtraRearWheelsCollider = new RCC_WheelCollider[ExtraRearWheelsTransform.Length];

        //    for (int i = 0; i < ExtraRearWheelsTransform.Length; i++)
        //    {
        //        ExtraRearWheelsCollider[i] = allWheelColliders[i + 4];
        //    }



        //}

    }

}
