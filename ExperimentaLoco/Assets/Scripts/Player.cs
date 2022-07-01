//================================================================
// Darmstadt University of Applied Sciences, Expanded Realities
// Course:       Travel & Transit in VR (by Philip Hausmeier)
// Script by:    Daniel Heilmann (771144)
// Last changed: 01-07-22
//================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    //# Public Variables 
    public float gravity = -9.81f;
    public bool canSpawnExplosives;
    public bool canDetonateExplosive;

    //public int cooldownExplosiveActivation;

    public List<GameObject> ExplosivesInWorld;
    public int maxExplosivesInWorld;
    public GameObject explosiveSpawnOrigin;
    public GameObject explosivePrefab;

    //# References to other components 
    private CharacterController controller;
    private InputManager inputManager;

    //# Private Variables 
    private Vector3 velocity;
    private bool isGrounded;

    //# Monobehaviour Events 
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        controller.Move(velocity * Time.deltaTime); //< Multiply by Time.deltaTime a second time, because that's how physics work (https://youtu.be/_QajrabyTJc?t=998)
    }

    private void OnEnable()
    {
        inputManager.Event_ThrowExplosive += ThrowExplosive;
        inputManager.Event_DetonateExplosive += DetonateExplosive;
    }

    private void OnDisable()
    {
        inputManager.Event_ThrowExplosive -= ThrowExplosive;
        inputManager.Event_DetonateExplosive -= DetonateExplosive;
    }

    //# Public Methods 
    public void Launch(Vector3 propulsion)
    {
        velocity += propulsion;
        controller.Move(velocity);
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