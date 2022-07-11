//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 11-07-22
// Use: This script is intended for HMD-less debugging and
//      testing of game features.
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction lookInput;
    public float mouseSensitivity = 100f;
    public Transform playerTransform;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookInput = playerInput.actions["Look"];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = lookInput.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouse.x);
    }
}
