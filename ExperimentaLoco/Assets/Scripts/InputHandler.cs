//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 26-07-22
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
    public void OnSpawnPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.Spawn has been called. -> Button is pressed.", this);
            player.OnSpawn();
        }
    }

    public void OnThrowPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"InputHandler.Throw has been performed. -> Button is released.", this);
            player.OnThrow();
        }
        // if (context.canceled)    //! If gripPressed is set to "Interaction: On Release Only", it only calls CallbackContext.canceled when pressed.
        // {
        //     Debug.Log($"InputHandler.Throw has been canceled. -> Button is released.", this);
        //     player.OnThrow();
        // }
    }

    public void OnDetonatePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log($"InputHandler.Detonate has been called.", this);
            player.OnDetonate();
        }
    }

    public void OnIncreaseForcePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log($"InputHandler.IncreaseForce has been called.", this);
            player.OnIncreaseForce();
        }
    }

    public void OnDecreaseForcePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log($"InputHandler.DecreaseForce has been called.", this);
            player.OnDecreaseForce();
        }
    }
}
