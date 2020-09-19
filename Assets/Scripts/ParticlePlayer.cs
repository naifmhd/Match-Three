using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem[] allParticles;
    public float lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        allParticles = GetComponentsInChildren<ParticleSystem>();

        Destroy(gameObject, lifeTime);
    }

    public void Play()
    {
        foreach(ParticleSystem ps in allParticles)
        {
            ps.Stop();
            ps.Play();
        }
    }

}
