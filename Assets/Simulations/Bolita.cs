using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Choque Esferas

public class Bolita : MonoBehaviour
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

    public void DetectColission(Bolita otherBolita)
    {
        collision = false;
        float angleRadians = angleDegrees * Mathf.Deg2Rad; //Angulo a radianes

        if (Pos.y <= 0.005)
        {
            collision = true;
        }

        if (otherBolita != null)
        {
            float distance = Mathf.Sqrt(
            Mathf.Pow(otherBolita.Pos.x - Pos.x, 2) + Mathf.Pow(otherBolita.Pos.y - Pos.y, 2) + Mathf.Pow(otherBolita.Pos.z - Pos.z, 2)
        );

            float radius = transform.localScale.x / 2 + otherBolita.transform.localScale.x / 2;

            if (distance <= radius)
            {
                collision = true;
                Vel = -Vel * DampingFactor;
                otherBolita.Vel = -otherBolita.Vel * otherBolita.DampingFactor;
                angleDegrees = 180 - angleDegrees;
            }
        }

    }

    public void Shoot(float h, float friction, float gravity, Bolita otherBolita)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad; //Angulo a radianes

        CalculateForce(gravity, friction);

        float initialVelX = initialVel.magnitude * Mathf.Cos(angleRadians);
        float initialVelY = initialVel.magnitude * Mathf.Sin(angleRadians);

        Vector3 Acceleration = Force / mass;
        Vel += h * Acceleration;

        

        Pos.x += h * Vel.x;
        Pos.y += h * Vel.y;

        DetectColission(otherBolita);
        LoadPos();
    }

    public void LoadPos()
    {
        transform.position = Pos;
    }
}