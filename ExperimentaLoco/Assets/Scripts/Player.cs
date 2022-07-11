//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 11-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    //# Public Variables 
    public float gravity = -9.81f;
    public float friction = 0.3f;  //< while on ground
    public float drag = 0.1f;      //< while in air
    public int maxExplosivesInWorld;
    public GameObject explosiveSpawnOrigin;
    public GameObject DEBUGExplosiveSpawn;
    public GameObject explosivePrefab;
    public List<GameObject> ExplosivesInWorld;

    //public bool canSpawnExplosives;
    //public bool canDetonateExplosive; //maybe set this up as return method instead?

    //public int cooldownExplosiveActivation;

    //# Private Variables 
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    //# Monobehaviour Events 
    private void Start()
    {
        controller = GetComponent<CharacterController>();   //< Set up CharacterController reference

        if (GameManager.Instance.DebugWithoutHMD)           //< Every change for debugging without an HMD goes here
        {
            explosiveSpawnOrigin = DEBUGExplosiveSpawn;
        }
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        controller.Move(velocity * Time.deltaTime); //< Multiply by Time.deltaTime a second time, because that's how physics work (https://youtu.be/_QajrabyTJc?t=998)

        velocity.x = DecreaseVelocity(velocity.x);
        velocity.y = DecreaseVelocity(velocity.y);
    }

    //# Public Methods 
    public void Launch(Vector3 propulsion)
    {
        velocity += propulsion;
        //controller.Move(velocity); not needed, unless you want to make a dash, because controller.Move is already being called every frame
    }

    public void ThrowExplosive()
    {
        Debug.Log($"Player.ThrowExplosive has been called.");
        GameObject newExplosive = Instantiate(explosivePrefab, explosiveSpawnOrigin.transform.position, Quaternion.identity);
        ExplosivesInWorld.Add(newExplosive);

        if (maxExplosivesInWorld <= 0)
            return;
        else if (ExplosivesInWorld.Count > maxExplosivesInWorld)
        {
            GameObject OldestExplosive = ExplosivesInWorld[0];
            ExplosivesInWorld.Remove(OldestExplosive);
            OldestExplosive.GetComponent<Explosive>().Despawn();
        }
    }

    public void DetonateExplosive()
    {
        Debug.Log($"Player.DetonateExplosive has been called.");
        List<GameObject> Reversed_ExplosivesInWorld = ExplosivesInWorld;
        Reversed_ExplosivesInWorld.Reverse();
        GameObject CurrentExplosive = Reversed_ExplosivesInWorld[0];

        if (CurrentExplosive != null)
        {
            ExplosivesInWorld.Remove(CurrentExplosive);
            CurrentExplosive.GetComponent<Explosive>().Detonate(this);
        }
    }

    //# Private Methods 
    private float DecreaseVelocity(float velocity)
    {
        if (velocity == 0)     //< Cancel out immediately if input value is 0. (Don't bother running this method at all if player is standing still.)
            return velocity;

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

    //# Input Event Handlers 


    //# ARCHIVED - Nice ideas, but there were better 
    // private void DecreaseFloat(float value)
    // {
    //     if (velocity.x == 0 && velocity.z == 0)  //< Don't bother running this method at all if player is standing still.
    //         return;

    //     //> Setting the reductionValue based on if player is grounded or in the air.
    //     float reductionValue;
    //     if (isGrounded)
    //         reductionValue = friction;
    //     else
    //         reductionValue = drag;

    //     //> Applying the reductionValue so that it always diminshes the velocity towards 0
    //     //  If velocity in that axis is already 0, then there is no need to edit it.
    //     if (velocity.x > 0)
    //         velocity.x -= reductionValue * Time.deltaTime;
    //     else if (velocity.x < 0)
    //         velocity.x += reductionValue * Time.deltaTime;

    //     if (velocity.y > 0)                                  //< This is partially redundant code, but I do not know how to improve it currently.
    //         velocity.y -= reductionValue * Time.deltaTime;
    //     else if (velocity.y < 0)
    //         velocity.y += reductionValue * Time.deltaTime;
    // }
}