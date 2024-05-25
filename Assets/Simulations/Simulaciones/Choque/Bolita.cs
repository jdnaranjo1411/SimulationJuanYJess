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
        CalculateForce(Physics.gravity.y, 0); // Puedes ajustar la fricción si es necesario
        Vector3 acceleration = Force / mass;
        Vel += h * acceleration;
        Pos += h * Vel;

        DetectCollision(null); // Si necesitas detectar colisiones con otras bolitas, pásalas aquí
        LoadPos();
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

        Vector3 frictionForce = -friction * Vel.normalized;
        Vel += (frictionForce / mass) * Time.deltaTime;
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

        // Calcular la dirección del disparo relativa al punto de origen del disparo
        Vector3 shootDirection = (transform.position - contact.point).normalized;
        float angle = Vector3.Angle(-normal, shootDirection);

        // Calcular la nueva dirección de movimiento basada en el ángulo de impacto
        Vector3 newVelocity = Quaternion.AngleAxis(angle, Vector3.Cross(-normal, shootDirection)) * -normal;

        // Aplicar la nueva velocidad a la bolita
        Vel = newVelocity.normalized * Vel.magnitude;


        // Activar la simulación
        ApplyImpact();
    }

    void ApplyImpact()
    {
        // Iniciar la simulación
        isSimulating = true;
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