//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144), Lili Weirich (769701)
// With tips by: Jesco
// Last changed: 18-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    //# Public Variables 
    public int explosionRadius = 5;
    public bool hideRangeIndicator = false;
    public GameObject radiusIndicator;
    public bool canAttachToSurface = true;

    //public bool canBeActivated; //< If for some reason, a bomb just cannot / must not be activated.
    //public bool isSelected; //< For a later iteration, where you can detonate bombs you look at.

    //# Private Variables 
    private AudioSource audioSource;
    private new Rigidbody rigidbody;
    [SerializeField] private GameObject particlesPrefab;
    private GameObject particlesGameObject;

    //# Monobehaviour Events 
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (hideRangeIndicator)
            radiusIndicator.SetActive(false);   //< Disable the indicator if hideRangeIndicator is true
        else
            radiusIndicator.transform.localScale = Vector3.one * 2 * explosionRadius;  //< Needs to be multiplied by 2 for the radius to diameter conversion 

        StartCoroutine(SpawnAnimation());
    }

    //> This continuous check guarantees that the explosive sticks to any surface and cannot fall through when held into a wall and then dropped.
    private void OnCollisionStay(Collision collision)
    {
        AttachToSurface(collision);
    }

    //# Public Methods 
    public void Detonate(Player player, int explosionForce)  //> Applies velocity to all players within explosionRadius
    {
        //Debug.Log($"{this.name} has been detonated!");

        Vector3 playerCenter = player.GetCameraPosition() + new Vector3(0, -1, 0);
        //< The "Center" / Impact point on the player is always 1 meter below the camera, which should roughly be hip height. Should probably be done via get method in player.
        Vector3 vectorToPlayer = playerCenter - this.transform.position;

        if (vectorToPlayer.magnitude <= explosionRadius)
        {
            //Debug.Log($"{this.name} has been detonated near player {player.gameObject.name}!");
            player.Launch(vectorToPlayer.normalized * explosionForce);
        }

        //> Play VFX and SFX
        particlesGameObject = Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        PlayExplosionSound();
    }

    public void Despawn()
    {
        Destroy(particlesGameObject);
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

    private IEnumerator DestroyAfterSoundDone()
    {
        float clipDuration = audioSource.clip.length;
        yield return new WaitForSeconds(clipDuration);
        Despawn();
    }

    private void AttachToSurface(Collision collision)
    {
        //> Constrain / Freeze rigidbody
        //Debug.Log("Collided with something that isn't the player.");    //< Because it can't collide with the player due to collision layers
        if (canAttachToSurface)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Collider>().enabled = false;
        }

        //> Align explosive to surface
        Vector3 surfaceNormal = collision.contacts[0].normal;
        //Debug.DrawRay(collision.contacts[0].point, surfaceNormal, Color.green, 10, false);
        transform.up = surfaceNormal;
    }

    private IEnumerator SpawnAnimation()
    {
        Vector3 startingScale = transform.localScale;
        float animationTime = 0.1f;
        float stepSize = 1 / animationTime;
        float delta = 0;

        while (delta < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, startingScale, delta);
            delta += stepSize * Time.deltaTime;
            yield return null;
        }
    }

    //# Input Event Handlers 
}
