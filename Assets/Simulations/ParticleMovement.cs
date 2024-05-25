using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour
{
    [SerializeField] Vector3 initialPos;
    [SerializeField] Vector3 initialVel;
    [SerializeField] float DampingFactor;
    [SerializeField] float mass;

    [SerializeField] float angleDegrees; //Angulo

    Vector3 Pos;
    Vector3 Vel;
    Vector3 Force;

    private Boolean collision;

    // Start is called before the first frame update
    void Start()
    {
        Pos = initialPos;
        Vel = initialVel;
        collision = false;
    }

    private void CalculateForce(float gravity, float friction)
    {
        Vector3 g;
        //Vector3 b;
        if (collision)
        {
            Vel.y = -Vel.y * DampingFactor;
        }
        g.x = 0;
        g.y = mass * gravity;
        g.z = 0;

        //b = mass * friction * Vel;
        //Force = g + b;

        Vector3 frictionForce = -friction * Vel.normalized;
        Vel += (frictionForce / mass) * Time.deltaTime;
        Vel += g * Time.deltaTime;

    }

    public void DetectColission()
    {
        collision = false;
        if (Pos.y <= 0.005)
        {
            collision = true;

        }
    }

    public void Shoot(float h, float friction, float gravity)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad; //Angulo a radianes

        CalculateForce(gravity, friction);

        float initialVelX = initialVel.x * Mathf.Cos(angleRadians);
        float initialVelY = initialVel.y * Mathf.Sin(angleRadians);

        Vector3 Acceleration = Force / mass;
        Vel += h * Acceleration;
        Pos += h * Vel;

        DetectColission();
        LoadPos();
    }

    public void LoadPos()
    {
        transform.position = Pos;
    }
}
