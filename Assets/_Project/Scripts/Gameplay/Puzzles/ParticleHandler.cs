using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    public void PlayParticle(Vector2 position)
    {
        foreach(ParticleSystem paticle in particles)
        {
            paticle.transform.position = position;
            paticle.Play();
        }
    }
}
