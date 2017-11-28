using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour {

    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public Transform[] m_PlayerPositions;


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
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
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

        Move();
        TurnToClosestPlayer();

    }


    private float TurnToClosestPlayer()
    {
        float direction = 0f;

        if(m_PlayerPositions.Length < 0)
        {
            Vector3 originToClosestPlayer = transform.position - m_PlayerPositions[0].position;

            float distanceToClosestPlayer = originToClosestPlayer.magnitude;

            for(int i = 1; i < m_PlayerPositions.Length; i++)
            {
                Vector3 originToPlayer = transform.position - m_PlayerPositions[i].position;

                float distanceToPlayer = originToPlayer.magnitude;

                if(distanceToPlayer < distanceToClosestPlayer)
                {
                    originToClosestPlayer = originToPlayer;
                    distanceToClosestPlayer = distanceToPlayer;
                }
            }


        }

        return direction;
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.

        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }

}
