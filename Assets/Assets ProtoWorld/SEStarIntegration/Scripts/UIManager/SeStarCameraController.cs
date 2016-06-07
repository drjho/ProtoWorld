﻿/*
 * 
 * SESTAR INTEGRATION
 * SeStarCameraController.cs
 * Miguel Ramos Carretero
 * Aram Azhari
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for SeStar UDP Camera, which sends the position of SyntheticEntities. 
/// </summary>
public class SeStarCameraController : MonoBehaviour
{
    [HideInInspector]
    public SEStar seStarObject;
    public bool Activated = false;
    public float UpdateFrequency = 0.1f;
    public float Unity_SeStarFovOffset = 20;
    private float time = 0;
    private Vector3 oldPos;
    private Vector3 oldRot;
    private float oldFov;

    /// <summary>
    /// Start the script.
    /// </summary>
    void Start()
    {
        seStarObject = this.transform.GetComponent<SEStar>();
        time = Time.time;
        oldPos = Camera.main.transform.position;
        oldRot = Camera.main.transform.rotation.eulerAngles;
        oldFov = Camera.main.fieldOfView;
    }

    /// <summary>
    /// Update method. 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            AIControllerWithLOD.StopAnimationDistance -= 5;
            Mathf.Clamp(AIControllerWithLOD.StopAnimationDistance, 0, 150);
            Debug.Log("ThalesAIController.StopAnimationDistance = " + AIControllerWithLOD.StopAnimationDistance);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            AIControllerWithLOD.StopAnimationDistance += 5;
            Mathf.Clamp(AIControllerWithLOD.StopAnimationDistance, 0, 150);
            Debug.Log("ThalesAIController.StopAnimationDistance = " + AIControllerWithLOD.StopAnimationDistance);
        }
        if (Activated)
        {
            if (Time.time - time > UpdateFrequency)
            {
                if (oldPos != Camera.main.transform.position
                    || oldRot != Camera.main.transform.rotation.eulerAngles
                    || oldFov != Camera.main.fieldOfView)
                {
                    seStarObject.UpdateSEStarCamera(
                        Camera.main.transform.position,
                        Camera.main.transform.rotation.eulerAngles,
                        Mathf.Clamp(Camera.main.fieldOfView + Unity_SeStarFovOffset, 1, 179));

                    time = Time.time;
                    oldPos = Camera.main.transform.position;
                    oldRot = Camera.main.transform.rotation.eulerAngles;
                    oldFov = Camera.main.fieldOfView;
                }
            }
        }
    }

    /// <summary>
    /// Activates the SEStar UDP Camera.
    /// </summary>
    public void Activate()
    {
        if (!Activated)
        {
            if (seStarObject != null)
                seStarObject.CreateNewSEStarCamera();
            Activated = true;
        }
    }
}
