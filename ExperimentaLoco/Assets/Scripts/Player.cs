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
    public float friction = -0.3f;  //< while on ground
    public float drag = -0.1f;      //< while in air
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

        // Todo: Depending on if value is below or above 0, go towards 0
        if (isGrounded)
        {
            velocity += new Vector3(friction * Time.deltaTime, 0, friction * Time.deltaTime);
        }
        else
        {
            velocity += new Vector3(drag * Time.deltaTime, 0, drag * Time.deltaTime);
        }
    }

    //# Public Methods 
    public void Launch(Vector3 propulsion)
    {
        velocity += propulsion;
        //controller.Move(velocity); not needed, unless you want to make a dash, because controller.Move is already being called every frame
    }

    //# Private Methods 
    private void ThrowExplosive()
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

    private void DetonateExplosive()
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

    //# Input Event Handlers 
}