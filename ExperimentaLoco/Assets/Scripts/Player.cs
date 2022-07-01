//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 01-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    //# Public Variables 
    public bool canSpawnExplosives;
    public bool canDetonateExplosive;
    public int cooldownExplosiveActivation;

    public List<Explosive> ExplosivesInWorld;
    public int maxExplosivesInWorld;

    public float gravity = -9.8f;

    //# Private Variables 

    private PlayerControls playerControls;
    private PlayerControls.PlayerActions standardControls;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    //# Monobehaviour Events 
    private void Awake()
    {
        playerControls = new PlayerControls();
        standardControls = playerControls.Player;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        // inputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        velocity.y += gravity * Time.deltaTime;
        if (!isGrounded && velocity.y < 0)
            velocity.y = 0;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        // inputManager.Event_ThrowExplosive += ThrowExplosive;
        // inputManager.Event_DetonateExplosive += DetonateExplosive;

        // Throw.performed += ThrowExplosive;
        standardControls.Enable();
    }

    private void OnDisable()
    {
        // inputManager.Event_ThrowExplosive -= ThrowExplosive;
        // inputManager.Event_DetonateExplosive -= DetonateExplosive;

        standardControls.Disable();
    }

    //# Public Methods 

    //# Private Methods 
    private void ThrowExplosive(/*InputAction.CallbackContext context*/)
    {
        // if (!context.performed) // Guard-clause
        //     return;

        Debug.Log($"Player.ThrowExplosive has been called.");
    }

    private void DetonateExplosive(/*InputAction.CallbackContext context*/)
    {
        // if (!context.performed) // Guard-clause
        //     return;

        Debug.Log($"Player.DetonateExplosive has been called.");
    }

    //# Input Event Handlers 
    // private void OnThrow()
    // {
    //     ThrowExplosive();
    // }
    // private void OnDetonate()
    // {
    //     DetonateExplosive();
    // }
}