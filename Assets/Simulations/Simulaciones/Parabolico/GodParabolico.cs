using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodParabolico : MonoBehaviour
{
    private float h;
    private float friction;
    private float gravity;
    public ParticleMovement[] particles;

    public void Initialize(float h, float friction, float gravity)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        particles = GameObject.FindObjectsOfType<ParticleMovement>();
    }

    public void Simulate()
    {
        foreach (ParticleMovement particle in particles)
        {
            particle.Shoot();
        }
    }
}
