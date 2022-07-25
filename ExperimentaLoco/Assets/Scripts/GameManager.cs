//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 06-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //# Public Variables
    public static GameManager Instance { set; get; }
    public bool DebugWithoutHMD;

    //# Private Variables 

    //# Monobehaviour Events 
    private void Awake()
    {
        if (Instance == null)   //< With this if-structure it is IMPOSSIBLE to create more than one instance.
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); //< Referring to the gameObject, this singleton script (class) is attached to.
        }
        else
        {
            Destroy(this.gameObject);   //< If you somehow still get to create a new singleton gameobject regardless, destroy the new one.
        }
    }

    //# Private Methods 

    //# Input Event Handlers 
}