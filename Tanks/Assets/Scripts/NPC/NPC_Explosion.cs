using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Explosion : MonoBehaviour {

    public Rigidbody m_Shell;

    private void OnTriggerEnter(Collider other)
    {
        //Explode if colliding with object or NPC
        if ((other.tag == "Object" || other.tag == "NPC") && !GetComponent<NPC_Health>().m_Dead)
        {
            Explode();
        }
    }

    public void Explode()
    {
        //Spawn a shell at the NPCs location
        Rigidbody shellInstance = Instantiate(m_Shell, transform.position, transform.rotation) as Rigidbody;

        //Trigger the OnDeath function in the NPC_Health script
        GetComponent<NPC_Health>().OnDeath();

    }
}

