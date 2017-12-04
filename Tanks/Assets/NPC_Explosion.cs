using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Explosion : MonoBehaviour {

    public Rigidbody m_Shell;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Object" || other.tag == "NPC")
        {
            Rigidbody shellInstance = Instantiate(m_Shell, transform.position, transform.rotation) as Rigidbody;

            gameObject.SetActive(false);
        }
    }
}

