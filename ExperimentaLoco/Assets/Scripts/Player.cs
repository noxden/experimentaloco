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
    public float friction = 3f;  //< while on ground
    public float drag = 1f;      //< while in air
    public int maxExplosivesInWorld;
    public GameObject explosivePrefab;
    public GameObject explosiveSpawnOrigin;
    public GameObject explosiveSpawnOriginDebug;
    public GameObject LeftHand;
    public GameObject RightHand;
    public List<GameObject> ExplosivesInWorld;

    //public bool canSpawnExplosives;
    //public bool canDetonateExplosive; //maybe set this up as return method instead?

    //public int cooldownExplosiveActivation;

    //# Private Variables 
    private CharacterController controller;
    [SerializeField] private Vector3 velocity;   //< SerializeField for debugging purposes.
    private bool isGrounded;

    //# Monobehaviour Events 
    private void Start()
    {
        controller = GetComponent<CharacterController>();   //< Set up CharacterController reference

        if (GameManager.Instance.DebugWithoutHMD)           //< Every change for debugging without an HMD goes here
        {
            explosiveSpawnOrigin = explosiveSpawnOriginDebug;
            controller.center = Vector3.zero;

            LeftHand.SetActive(false);
            RightHand.SetActive(false);
        }
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
        if (ExplosivesInWorld.Count == 0)
        {
            Debug.Log($"Player.DetonateExplosive: There are no explosives in the world.");
            return;
        }

        Debug.Log($"Player.DetonateExplosive has been called.");
        List<GameObject> Reversed_ExplosivesInWorld = ExplosivesInWorld;
        Reversed_ExplosivesInWorld.Reverse();
        GameObject CurrentExplosive = Reversed_ExplosivesInWorld[0];

        ExplosivesInWorld.Remove(CurrentExplosive);
        CurrentExplosive.GetComponent<Explosive>().Detonate(this);
    }

    //# Private Methods 
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
        float stopThreshold = 0.001f;
        return ((value <= stopThreshold && value > 0) || (value >= -stopThreshold && value < 0));
    }

    //# Input Event Handlers 
    private void OnThrow()
    {
        ThrowExplosive();
    }

    private void OnDetonate()
    {
        DetonateExplosive();
    }
}