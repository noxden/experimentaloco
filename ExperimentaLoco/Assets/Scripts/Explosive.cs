//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 01-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    //# Public Variables 
    public int explosionRadius;
    public int explosionForce;
    public bool canBeActivated;

    //# Private Variables 

    //# Monobehaviour Events 

    //# Public Methods 
    public void Detonate()
    {
        // Applies velocity to all players within explosionRadius
    }
    public void Despawn()
    {

    }

    //# Private Methods 

    //# Input Event Handlers 
}
