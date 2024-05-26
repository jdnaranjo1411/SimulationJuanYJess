using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodParabolico : MonoBehaviour
{
    private float h = 0.01f;
    private float friction = 0.02f;
    private float gravity = -9.8f;
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
