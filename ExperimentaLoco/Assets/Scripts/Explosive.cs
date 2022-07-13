//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144), Lili Weirich (769701)
// Last changed: 13-07-22
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
    public AudioSource explosionSound;

    //public bool canBeActivated; //< If for some reason, a bomb just cannot / must not be activated.
    //public bool isSelected; //< For a later iteration, where you can detonate bombs you look at.

    //# Private Variables 

    //# Monobehaviour Events 
    private void Start()
    {
        radiusIndicator.transform.localScale = Vector3.one * 2 * explosionRadius;  //< Needs to be multiplied by 2 for the radius to diameter conversion 
    }

    //# Public Methods 
    public void Detonate(Player player)  //> Applies velocity to all players within explosionRadius
    {
        Debug.Log($"{this.name} has been detonated!");
        Vector3 vectorToPlayer = player.transform.position - this.transform.position;
        
        if (vectorToPlayer.magnitude <= explosionRadius)
        {
            Debug.Log($"{this.name} has been detonated near player {player.gameObject.name}!");
            player.Launch(vectorToPlayer.normalized * explosionForce);
        }

        explosionSound.Play();
        MeshRenderer[] allMeshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer entry in allMeshRenderers)
        {
            entry.enabled = false;
        }

        StartCoroutine(DestroyAfterSoundDone());
    }

    public void Despawn()
    {
        Destroy(this.gameObject);
    }

    //# Private Methods 

    IEnumerator DestroyAfterSoundDone()
    {
        float clipDuration = explosionSound.clip.length;
        yield return new WaitForSeconds(clipDuration);
        Despawn();
    }
    //# Input Event Handlers 
}
