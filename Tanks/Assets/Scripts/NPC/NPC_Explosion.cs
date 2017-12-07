using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Explosion : MonoBehaviour {

    public Rigidbody m_Shell;

    private void OnTriggerEnter(Collider other)
    {
        
        if ((other.tag == "Object" || other.tag == "NPC") && !GetComponent<NPC_Health>().m_Dead)
        {
            Rigidbody shellInstance = Instantiate(m_Shell, transform.position, transform.rotation) as Rigidbody;

            GetComponent<NPC_Health>().m_Dead = true;

            Debug.Log("Dead: " + GetComponent<NPC_Health>().m_Dead);

        }
    }
}

