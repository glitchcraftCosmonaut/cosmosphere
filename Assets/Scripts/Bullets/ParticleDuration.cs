using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDuration : MonoBehaviour
{
    private ParticleSystem ps;
    public float duration = 2;
    BulletHellSpawner bulletHellSpawner;

    void Awake()
    {
        // bulletHellSpawner = GetComponentInParent<BulletHellSpawner>();
        ps = GetComponent<ParticleSystem>();
        ps.Stop(); // Cannot set duration whilst Particle System is playing

        var main = ps.main;
        main.duration = duration;

        ps.Play();
    }
}
