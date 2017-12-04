using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Setup : MonoBehaviour {

    public Color m_PlayerColor;
    public Transform m_StartTransform;

    private Vector3 m_StartPos;

    // Use this for initialization
    void Start () {

        m_StartPos = new Vector3(m_StartTransform.position.x, m_StartTransform.position.y, m_StartTransform.position.z);

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }

    void OnEnable()
    {
        Debug.Log(m_StartPos);
        GetComponent<Rigidbody>().MovePosition(m_StartPos);
    }
}
