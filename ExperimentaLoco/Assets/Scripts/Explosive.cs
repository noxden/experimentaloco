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

    //public bool canBeActivated; //< If for some reason, a bomb just cannot / must not be activated.
    //public bool isSelected; //< For a later iteration, where you can detonate bombs you look at.

    //# Private Variables 
    private AudioSource audioSource;
    private new Rigidbody rigidbody;

    //# Monobehaviour Events 
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        radiusIndicator.transform.localScale = Vector3.one * 2 * explosionRadius;  //< Needs to be multiplied by 2 for the radius to diameter conversion 
    }

    private void OnCollisionEnter(Collision collision)
    {
        AttachToSurface(collision);
    }

    //# Public Methods 
    public void Detonate(Player player)  //> Applies velocity to all players within explosionRadius
    {
        Debug.Log($"{this.name} has been detonated!");

        Vector3 playerCenter = new Vector3(player.mainCamera.transform.position.x, player.mainCamera.transform.position.y - 1f, player.mainCamera.transform.position.z);
        //< The "Center" / Impact point on the player is always 1 meter below the camera, which should roughly be hip height. Should probably be done via get method in player
        Vector3 vectorToPlayer = playerCenter - this.transform.position;

        if (vectorToPlayer.magnitude <= explosionRadius)
        {
            Debug.Log($"{this.name} has been detonated near player {player.gameObject.name}!");
            player.Launch(vectorToPlayer.normalized * explosionForce);
        }

        PlayExplosionSound();
    }

    public void Despawn()
    {
        Destroy(this.gameObject);
    }

    //# Private Methods 

    private void PlayExplosionSound()
    {
        audioSource.Play();
        MeshRenderer[] allMeshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer entry in allMeshRenderers)
        {
            entry.enabled = false;
        }

        StartCoroutine(DestroyAfterSoundDone());
    }

    IEnumerator DestroyAfterSoundDone()
    {
        float clipDuration = audioSource.clip.length;
        yield return new WaitForSeconds(clipDuration);
        Despawn();
    }

    private void AttachToSurface(Collision collision)
    {
        //> Constrain / Freeze rigidbody
        Debug.Log("Collided with something that isn't the player.");    //< Because it can't collide with the player due to collision layers
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Debug.DrawRay(transform.position, transform.up, Color.red, 10, false);

        Vector3 collisionNormal = collision.contacts[0].normal;
        Debug.DrawRay(collision.contacts[0].point, collisionNormal, Color.green, 10, false);

        if (collisionNormal.x != 0)         //< so if (collisionNormal == Vector3.right || collisionNormal == Vector3.left)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, collisionNormal);
        else if (collisionNormal.z != 0)    //< so if (collisionNormal == Vector3.forward || collisionNormal == Vector3.back)
            transform.rotation = Quaternion.LookRotation(Vector3.left, collisionNormal);
        else if (collisionNormal.y != 0)
            transform.rotation = Quaternion.LookRotation(Vector3.back, collisionNormal);

        Debug.Log($"Transform Rotation: {transform.rotation}");
        Debug.Log($"Environment Normal: {collision.contacts[0].normal}");
        Debug.Log($"Quaternion Euler: {Quaternion.Euler(collisionNormal)}");
    }

    //# Input Event Handlers 
}
