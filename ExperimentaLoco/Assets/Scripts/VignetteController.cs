//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 26-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteController : MonoBehaviour
{
    //# Public Variables 
    [Range(0f, 5f)] public float minActivationThreshold;
    [Range(0f, 1f)] public float maxIntensity, minIntensity;
    public float duration;

    //# Private Variables 
    private Vignette vignette;
    private Player player;
    private float delta = 0;

    //# Monobehaviour Events 
    private void Start()
    {
        vignette = GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        bool hasVelocity = (player.velocity.magnitude >= minActivationThreshold);

        if (hasVelocity)
        {
            if (delta < 1)
                delta += Time.deltaTime / duration;     //< The higher the duration, the lower the increase in delta.
        }
        else
        {
            if (delta > 0)
                delta -= Time.deltaTime / duration;
        }
        vignette.intensity.Interp(minIntensity, maxIntensity, delta);
    }

    //# Private Methods 

    //# Input Event Handlers 
}
