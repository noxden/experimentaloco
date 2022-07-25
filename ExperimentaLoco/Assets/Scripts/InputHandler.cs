//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 20-07-22
//? Could maybe be improved by attaching a player input component to this, setting it to "Invoke Unity Events" and linking the private methods in here to those events, 
//? instead of having to convert the action map into it's own class (here PlayerControls), which can't be edited as easily.
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //# Public Variables 
    public Player player;

    //# Private Variables 
    private PlayerControls playerControls;
    private InputAction throwInput;
    private InputAction detonateInput;
    private InputAction increaseForceInput;
    private InputAction decreaseForceInput;

    //# Monobehaviour Events 
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    private void OnEnable()
    {
        throwInput = playerControls.Player.Throw;
        throwInput.Enable();
        throwInput.performed += Spawn;
        throwInput.canceled += Throw;

        detonateInput = playerControls.Player.Detonate;
        detonateInput.Enable();
        detonateInput.performed += Detonate;

        increaseForceInput = playerControls.Player.IncreaseForce;
        increaseForceInput.Enable();
        increaseForceInput.performed += IncreaseForce;
        
        decreaseForceInput = playerControls.Player.DecreaseForce;
        decreaseForceInput.Enable();
        decreaseForceInput.performed += DecreaseForce;
    }

    private void OnDisable()
    {
        throwInput.Disable();
        detonateInput.Disable();
        increaseForceInput.Disable();
        decreaseForceInput.Disable();
    }

    //# Private Methods 
    private void Spawn(InputAction.CallbackContext context)
    {
        Debug.Log($"InputHandler.Spawn has been called. -> Button is pressed.", this);
        player.OnSpawn();
    }

    private void Throw(InputAction.CallbackContext context)
    {
        Debug.Log($"InputHandler.Throw has been called. -> Button is released.", this);
        player.OnThrow();
    }

    private void Detonate(InputAction.CallbackContext context)
    {
        Debug.Log($"InputHandler.Detonate has been called.", this);
        player.OnDetonate();
    }

    private void IncreaseForce(InputAction.CallbackContext context)
    {
        Debug.Log($"InputHandler.IncreaseForce has been called.", this);
        player.OnIncreaseForce();
    }

    private void DecreaseForce(InputAction.CallbackContext context)
    {
        Debug.Log($"InputHandler.DecreaseForce has been called.", this);
        player.OnDecreaseForce();
    }

    //# Input Event Handlers 
}
