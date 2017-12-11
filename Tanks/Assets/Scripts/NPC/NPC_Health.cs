using UnityEngine;
using UnityEngine.UI;

public class NPC_Health : MonoBehaviour
{
    public GameObject m_ExplosionPrefab;

    [HideInInspector] public bool m_Dead = false;
    [HideInInspector] public AudioSource m_ExplosionAudio;
    [HideInInspector] public ParticleSystem m_ExplosionParticles;


    private void Awake()
    {
        //The components that will be used when exploding
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_Dead = false;
    }


    public void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.

        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}
