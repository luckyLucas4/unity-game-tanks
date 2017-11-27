using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour {

    public Rigidbody m_Shell;
    public float m_TimeToExplode;
    public bool m_Exploding;
    
    //Set to y level -0.24

    private Vector3 m_ExplosionPos;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, -0.24f, transform.position.z);
        m_ExplosionPos = new Vector3(transform.position.x, 0.1f, transform.position.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        //On tank enter: Instantiate shell
        if (other.tag == "Object")
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
        m_Exploding = true;

        yield return new WaitForSeconds(m_TimeToExplode);

        Rigidbody shellInstance = Instantiate(m_Shell, m_ExplosionPos, transform.rotation) as Rigidbody;

        Destroy(gameObject);

        m_Exploding = false;
    }
}
