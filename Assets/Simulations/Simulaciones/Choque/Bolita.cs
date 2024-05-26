using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Choque Esferas

public class Bolita : MonoBehaviour
{
    [SerializeField] float DampingFactor;
    [SerializeField] float mass;
    [SerializeField] float angleDegrees; // Angulo

    Vector3 Pos;
    Vector3 Vel;
    Vector3 Force;

    private bool collision;
    private bool isSimulating;

    void Start()
    {

        Pos = transform.position; // Usar la posición inicial del asset en la escena
        Vel = Vector3.zero;
        collision = false;
        isSimulating = false;
    }

    void Update()
    {
        if (isSimulating)
        {
            // Actualizar la posición y velocidad si está en simulación
            Simulate(Time.deltaTime);
        }
    }

    private void Simulate(float h)
    {
        CalculateForce(Physics.gravity.y, 0.2f); // Puedes ajustar la fricción si es necesario
        Vector3 acceleration = Force / mass;
        Vel += h * acceleration;
        Pos += h * Vel;

        Detect(); // Detectar colisión con el suelo u otros objetos
        LoadPos(); // Cargar la nueva posición
    }

    public void Detect()
    {
        collision = false;
        if (Pos.y <= 0.05)
        {
            collision = true;
            Pos.y = 0.05f;
        }
    }
    private void CalculateForce(float gravity, float friction)
    {
        Vector3 g;
        if (collision)
        {
            Vel.y = -Vel.y * DampingFactor;
        }
        g.x = 0;
        g.y = mass * gravity;
        g.z = 0;

        // Fuerza de fricción
        Vector3 frictionForce = -friction * Vel.normalized;

        // Aplicar la fuerza de fricción al objeto
        Vel += (frictionForce / mass) * Time.deltaTime;

        // Gravedad
        Vel += g * Time.deltaTime;
    }

    private void DetectCollision(Bolita otherBolita)
    {
        collision = false;

        if (Pos.y <= 0.005)
        {
            collision = true;
        }
        if (otherBolita != null)
        {
            float distance = Vector3.Distance(Pos, otherBolita.Pos);
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

    void OnCollisionEnter(Collision collision)
    {
        // Obtener el punto de impacto y la normal de la colisión
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // Activar la simulación con una fuerza basada en la normal de la colisión
        ApplyImpact(normal * 1f); // Reemplaza someForceMagnitude con la magnitud de la fuerza que deseas aplicar
    }


    void ApplyImpact(Vector3 impactForce)
    {
        Vel = impactForce / mass; // Ajusta la velocidad inicial basada en la fuerza del impacto
        isSimulating = true; // Iniciar la simulación
    }

    public void Shoot(float h, float friction, float gravity, Bolita otherBolita)
    {
        if (!isSimulating) return;

        CalculateForce(gravity, friction);

        Vector3 Acceleration = Force / mass;
        Vel += h * Acceleration;
        Pos += h * Vel;

        DetectCollision(otherBolita);
        LoadPos();
    }

    public void LoadPos()
    {
        transform.position = Pos;
    }
}