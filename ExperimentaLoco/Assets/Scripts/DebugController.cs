//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 01-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    //# Public Variables 
    public float rotationSpeed = 3.0f;

    //# Private Variables 
    private new Camera camera;
    private GameObject cameraObject;
    private Vector3 cameraRotation;

    //# Monobehaviour Events 
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        camera = GetComponentInChildren<Camera>();
        cameraObject = camera.gameObject;
    }

    private void Update()
    {
        rotate();
    }

    //# Private Methods 
    private void rotate()
    {
        Vector3 newRotation = Vector3.zero;
        newRotation.y = Input.GetAxis("Mouse X") * rotationSpeed;
        newRotation.x = Input.GetAxis("Mouse Y") * rotationSpeed;
        cameraObject.transform.Rotate(newRotation); // perforam rotation controlled by mouse
    }

    //# Input Event Handlers 
}
