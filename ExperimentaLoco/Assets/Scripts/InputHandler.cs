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

    //# Monobehaviour Events 

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    //# Private Methods 

    //# Input Event Handlers 
    public void OnThrowPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.Spawn has been called. -> Button is pressed.", this);
            player.OnSpawn();
        }
        if (context.canceled)
        {
            Debug.Log($"InputHandler.Throw has been called. -> Button is released.", this);
            player.OnThrow();
        }
    }

    public void OnDetonatePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.Detonate has been called.", this);
            player.OnDetonate();
        }
    }

    public void OnIncreaseForcePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.IncreaseForce has been called.", this);
            player.OnIncreaseForce();
        }
    }

    public void OnDecreaseForcePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.DecreaseForce has been called.", this);
            player.OnDecreaseForce();
        }
    }
}