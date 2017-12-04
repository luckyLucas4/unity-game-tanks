﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour {

    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public int m_NumberOfPlayers = 2;
    public List<Transform> m_PlayerTransforms = new List<Transform>();


    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue = 1f;
    private float m_TurnInputValue;
    private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update()
    {
        // Make sure the audio for the engine is playing. 
        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.

        TurnToClosestPlayer();
        Move();

    }


    private void TurnToClosestPlayer()
    {
        //If there are tanks
        if(m_PlayerTransforms.Count > 0)
        {
            // Setup values for the first player in list
            Transform closestPlayer = m_PlayerTransforms[0];
            Vector3 fromOriginToClosestPlayer = transform.position - closestPlayer.position;

            float distanceToClosestPlayer = fromOriginToClosestPlayer.magnitude;

            // For every player in the list
            foreach(Transform playerTransform in m_PlayerTransforms)
            {
                // A vector from the npc to the player
                Vector3 fromOriginToPlayer = transform.position - playerTransform.position;

                // Length (magnitude) of the vector
                float distanceToPlayer = fromOriginToPlayer.magnitude;

                // If the length is the longer than the previous
                if(distanceToPlayer < distanceToClosestPlayer)
                {
                    // Setup values for newly found player
                    fromOriginToClosestPlayer = fromOriginToPlayer;
                    distanceToClosestPlayer = distanceToPlayer;
                    closestPlayer = playerTransform;
                }
            }
            // A new vector that is rotated from current forward direction to the direction of the closest player (inverted?)
            // by an angle relative to m_TurnSpeed
            Vector3 newDir = Vector3.RotateTowards(transform.forward, -fromOriginToClosestPlayer, 
                m_TurnSpeed * Time.deltaTime, 0.0F);
            // Set the vector as the quaternion that is the current rotation
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }


    private void Move()
    {
        // Adjust the position of the tank 

        //A new vector that is extended from the forward direction
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        //Move the tank to the end of the new vector by adding its values
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }

}
