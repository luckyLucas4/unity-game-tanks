using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "NPC")
        {
            other.GetComponent<NPC_Health>().OnDeath();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
