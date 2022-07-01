//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 01-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //# Public Variables 
    public KeyCode Key_ThrowExplosive = KeyCode.Mouse0;
    public KeyCode Key_DetonateExplosive = KeyCode.Mouse1;
    
    public delegate void Input_ThrowExplosive();
    public Input_ThrowExplosive Event_ThrowExplosive;

    public delegate void Input_DetonateExplosive();
    public Input_DetonateExplosive Event_DetonateExplosive;
    

    //# Private Variables 

    //# Monobehaviour Events 
    private void Update()
    {
        if (Input.GetKeyDown(Key_ThrowExplosive))
        {
            Event_ThrowExplosive();
        }

        if (Input.GetKeyDown(Key_DetonateExplosive))
        {
            Event_DetonateExplosive();
        }
    }

    //# Private Methods 

    //# Input Event Handlers 

}
