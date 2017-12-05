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

    //private GameObject m_CanvasGameObject;

    public void Setup()
    {
        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_Color;
        }
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
