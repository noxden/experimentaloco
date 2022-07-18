//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 17-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandDisplay : MonoBehaviour
{
    //# Public Variables 


    //# Private Variables 
    private Player player;
    private Text display;

    //# Monobehaviour Events 

    //# Public Methods 
    public void Setup()
    {
        player = GetComponentInParent<Player>();
        Debug.Log($"Player is {player.name}");
        display = GetComponentInChildren<Text>();
        Debug.Log($"Display is {display.name}");
    }

    public void UpdateDisplay()
    {
        int _explosionForce = player.GetExplosionForce();
        Debug.Log($"_explosionForce is {_explosionForce}");
        display.text = _explosionForce.ToString();
    }

    //# Input Event Handlers 
    //> These don't work, probably because only one GameObject can receive a message?
    // private void OnIncreaseForce()
    // {
    //     Debug.Log($"HandDisplay.OnIncreaseForce was triggered.");
    //     UpdateDisplay();
    // }

    // private void OnDecreaseForce()
    // {
    //     Debug.Log($"HandDisplay.OnDecreaseForce was triggered.");
    //     UpdateDisplay();
    // }
}