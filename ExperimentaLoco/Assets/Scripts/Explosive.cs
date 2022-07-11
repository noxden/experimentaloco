//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 06-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    //# Public Variables 
    public int explosionRadius = 5;
    public int explosionForce = 5;
    public GameObject radiusIndicator;

    //public bool canBeActivated; //< If for some reason, a bomb just cannot / must not be activated.
    //public bool isSelected; //< For a later iteration, where you can detonate bombs you look at.

    //# Private Variables 
    //public Vector3 velocity;

    //# Monobehaviour Events 
    private void Start()
    {
        radiusIndicator.transform.localScale = new Vector3 (explosionRadius, explosionRadius, explosionRadius);
    }

    //# Public Methods 
    public void Detonate(Player player)  //> Applies velocity to all players within explosionRadius
    {
        Debug.Log($"{this.name} has been detonated!");
        Vector3 distToPlayer = player.transform.position - this.transform.position;
        if (distToPlayer.magnitude <= explosionRadius)
        {
            Debug.Log($"{this.name} has been detonated near player {player.gameObject.name}!");

            float propulsionForce = Mathf.Clamp(explosionForce / Mathf.Clamp(distToPlayer.magnitude, 1f, float.MaxValue), 0f, explosionForce);  // Todo: Needs fixing
            Debug.Log($"Propulsion Force: {propulsionForce}");
            player.Launch(distToPlayer.normalized * propulsionForce);
        }

        Despawn();
        
    }
    public void Despawn()
    {
        Destroy(this.gameObject);
    }

    //# Private Methods 

    //# Input Event Handlers 
}
