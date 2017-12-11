using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPC_Manager{

    public Color m_Color;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_NPC_Number;
    [HideInInspector] public GameObject m_Instance;


    public void Setup()
    {
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_Color;
        }
    }

    public void Reset()
    {
        //Teleport to spawnpoint
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        //Animate the explosion and play audio
        NPC_Health healthScript = m_Instance.GetComponent<NPC_Health>();

        healthScript.m_ExplosionParticles.transform.position = m_Instance.transform.position;
        healthScript.m_ExplosionParticles.gameObject.SetActive(true);

        healthScript.m_ExplosionParticles.Play();
        healthScript.m_ExplosionAudio.Play();

        //Reset the tank to remove all forces (isKinematic is triggered in the NPC_Movement script)
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);

        healthScript.m_Dead = false;
    }
}
