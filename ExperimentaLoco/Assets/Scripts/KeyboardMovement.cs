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

public class KeyboardMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction moveInput;
    public CharacterController controller;
    public float movementSpeed = 12f;
    
    private void Start()
    {
        moveInput = playerInput.actions["Move"];
    }

    void Update()
    {
        Vector3 movementDirection = transform.right * moveInput.ReadValue<Vector2>().x + transform.forward * moveInput.ReadValue<Vector2>().y;

        controller.Move(movementDirection * movementSpeed * Time.deltaTime);
    }
}
