using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour {

    public Rigidbody m_Shell;
    public float m_TimeToExplode, m_yPos, m_ExplosionHeight;
    
    private Vector3 m_ExplosionPos;

    private void Start()
    {
        // Change y level to m_yPos
        transform.position = new Vector3(transform.position.x, m_yPos, transform.position.z);

        // The explosion should be a little higher
        m_ExplosionPos = new Vector3(transform.position.x, m_ExplosionHeight, transform.position.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        //On tank enter: Instantiate shell
        if (other.tag == "Object" || other.tag == "NPC")
        {
            Explode();
        }
    }

    public void Explode()
    {
        StartCoroutine(ExplosionDelay());
    }

    private IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(m_TimeToExplode);

        Rigidbody shellInstance = Instantiate(m_Shell, m_ExplosionPos, transform.rotation) as Rigidbody;

        gameObject.SetActive(false);
    }
}
