//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 25-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    //# Public Variables 
    public Vector3 velocity;
    public float gravity = -9.81f;
    public float friction = 3f;  //< while on ground
    public float drag = 1f;      //< while in air
    public int maxExplosivesInWorld;
    public GameObject explosivePrefab;
    public GameObject explosiveSpawnOrigin;
    public GameObject ControllerLeft;
    public GameObject ControllerRight;
    public GameObject DebugAnchorLeft;
    public GameObject DebugAnchorRight;

    public List<GameObject> ExplosivesInWorld;

    //# Private Variables 
    private CharacterController controller;
    private HandDisplay handDisplay;
    private Camera mainCamera;
    private int explosionForce = 2;
    private GameObject heldExplosive;
    private bool isGrounded;

    //# Monobehaviour Events 
    private void Awake()
    {
        controller = GetComponent<CharacterController>();   //< Set up CharacterController reference
        mainCamera = GetComponentInChildren<Camera>();
        handDisplay = GetComponentInChildren<HandDisplay>();
    }
    private void Start()
    {
        if (GameManager.Instance.DebugWithoutHMD)           //< Every change for debugging without an HMD goes here
        {
            //explosiveSpawnOrigin = explosiveSpawnOriginDebug;
            controller.center = new Vector3(0f, -0.7f, 0f);     //< Adjusts camera position so that it is approximately at eye level, even without an hmd

            ControllerLeft.transform.SetParent(DebugAnchorLeft.transform, false);
            ControllerRight.transform.SetParent(DebugAnchorRight.transform, false);
        }

        handDisplay.UpdateDisplay();
    }

    private void Update()
    {
        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        controller.Move(velocity * Time.deltaTime); //< Multiply by Time.deltaTime a second time, because that's how physics work (https://youtu.be/_QajrabyTJc?t=998)

        isGrounded = controller.isGrounded;     //< For some reason, this order works. This line needs to be called directly after a non-zero controller.Move!

        velocity.x = DecreaseVelocity(velocity.x);
        velocity.z = DecreaseVelocity(velocity.z);
    }

    //# Public Methods 
    public void Launch(Vector3 propulsion)
    {
        velocity += propulsion;
    }

    public int GetExplosionForce()
    {
        return explosionForce;
    }

    public Vector3 GetCameraPosition()
    {
        return mainCamera.transform.position;
    }

    //# Private Methods 
    private void SpawnExplosive()
    {
        if (maxExplosivesInWorld == 0)  //< Guard clause
        {
            Debug.Log($"Player.SpawnExplosive: You cannot spawn any explosives, as maxExplosivesInWorld is currently set to 0.");
            return;
        }

        if (heldExplosive != null)    //< Guard clause -> If player is already holding an explosive, don't spawn another one.
            return;

        //Debug.Log($"Player.SpawnExplosive: Spawning explosive!");

        //> Instantiating and listing newly spawned explosive
        heldExplosive = Instantiate(explosivePrefab, explosiveSpawnOrigin.transform.position, Quaternion.identity);
        heldExplosive.GetComponent<Explosive>().canAttachToSurface = false;
        ExplosivesInWorld.Add(heldExplosive);

        //> Attaching explosive to hand ("explosiveSpawnOrigin") via FixedJoint.
        explosiveSpawnOrigin.GetComponent<FixedJoint>().connectedBody = heldExplosive.GetComponent<Rigidbody>();

        //> If necessary, removes oldest explosive in ExplosivesInWorld from list and despawns it.
        if (maxExplosivesInWorld <= -1)
            return;
        else if (ExplosivesInWorld.Count > maxExplosivesInWorld)
        {
            GameObject OldestExplosive = ExplosivesInWorld[0];
            ExplosivesInWorld.Remove(OldestExplosive);
            OldestExplosive.GetComponent<Explosive>().Despawn();
        }
    }

    private void ThrowExplosive()
    {
        if (heldExplosive == null)    //< If explosive was detonated in hand (and thereby deleted & removed from "heldExplosive"), then there is no need to throw it anymore.
            return;

        //Debug.Log($"Player.ThrowExplosive: Throwing explosive!");

        //> Making sure that the explosive is awake and then detaching it from hand.
        explosiveSpawnOrigin.GetComponent<FixedJoint>().connectedBody.WakeUp();
        explosiveSpawnOrigin.GetComponent<FixedJoint>().connectedBody = null;

        heldExplosive.GetComponent<Explosive>().canAttachToSurface = true;
        heldExplosive = null;
    }

    private void DetonateExplosive()
    {
        if (ExplosivesInWorld.Count == 0)
        {
            Debug.Log($"Player.DetonateExplosive: There are no explosives in the world.");
            return;
        }
        //Debug.Log($"Player.DetonateExplosive: Detonating explosive!.");

        //> Removes newest explosive in ExplosivesInWorld from list and detonates it.
        GameObject newestExplosive = ExplosivesInWorld[ExplosivesInWorld.Count - 1];
        ExplosivesInWorld.Remove(newestExplosive);
        newestExplosive.GetComponent<Explosive>().Detonate(this, explosionForce * 5);    //< Actual explosionForce is always 5 times the power displayed -> increasing power in steps of five when TweakExplosionForce is called
    }

    private float DecreaseVelocity(float velocity)
    {
        if (velocity == 0)     //< Cancel out immediately if input value is 0. (Don't bother running this method at all if player is standing still.)
            return 0;

        if (IsMiniscule(velocity))
            return 0;

        //> Setting the reductionValue based on if player is grounded or not.
        float reductionValue;
        if (isGrounded)
            reductionValue = friction;
        else
            reductionValue = drag;

        //> Applying the reductionValue so that it always diminshes the velocity's value towards 0.
        if (velocity > 0)
            velocity -= reductionValue * Time.deltaTime;
        else if (velocity < 0)
            velocity += reductionValue * Time.deltaTime;

        return velocity;
    }

    private bool IsMiniscule(float value)   //> Returns true if input value's magnitude is less than stopTreshold.
    {
        float stopThreshold = 0.1f;       //< DEFAULT: 0.001f, STANDALONE VR: 0.1f
        return (value <= stopThreshold && value >= -stopThreshold);
    }

    private void TweakExplosionForce(int change)
    {
        explosionForce += change;
        explosionForce = Mathf.Clamp(explosionForce, 1, 4); //< Four stages of explosionForce

        Debug.Log($"Player.TweakExplosionForce: Changed explosion force of {name} to {explosionForce}.");
        handDisplay.UpdateDisplay();
        handDisplay.PlaySound();
    }

    //# Input Event Handlers 
    public void OnSpawn()
    {
        SpawnExplosive();
    }

    public void OnThrow()
    {
        ThrowExplosive();
    }

    public void OnDetonate()
    {
        DetonateExplosive();
    }

    public void OnIncreaseForce()
    {
        TweakExplosionForce(1);
    }

    public void OnDecreaseForce()
    {
        TweakExplosionForce(-1);
    }
}