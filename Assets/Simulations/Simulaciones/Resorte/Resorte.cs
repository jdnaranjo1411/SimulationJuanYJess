using System.Collections.Generic;
using UnityEngine;

public class Resorte : MonoBehaviour
{
    [SerializeField] float k;
    [SerializeField] float restLength;

    // Objeto para el resorte
    [SerializeField] Resorte objResorte;

    [SerializeField] float mass;
    [SerializeField] Vector3 velocity = Vector3.zero;

    // Lista de resortes conectados
    public List<Resorte> resortesConectados = new List<Resorte>();

    private bool isSimulating = false;

    // Simulación del resorte
    public void Simulate(float h, float friction, float gravity)
    {
        if (!objResorte) return;

        Vector3 force = CalculateSpringForce();

        Vector3 acceleration = force / mass;

        acceleration -= friction * velocity;

        acceleration += new Vector3(0, gravity, 0);

        velocity += acceleration * h;
        transform.position += velocity * h;

        if (transform.position.y < 0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            velocity.y = 0.0f;
        }
    }

    Vector3 CalculateSpringForce()
    {
        if (!objResorte) return Vector3.zero;

        Vector3 totalForce = Vector3.zero;
        Vector3 displacement;

        displacement = transform.position - objResorte.transform.position;

        // Resorte actual
        float currentLength = displacement.magnitude;
        // Ley de Hooke (F = -k * (longitud actual - longitud natural))
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;
        totalForce += springForce;

        return totalForce;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Obtener el punto de impacto y la normal de la colisión
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // Activar la simulación al recibir una colisión
        StartSimulation();

        // Aplicar una fuerza adicional basada en la velocidad relativa de la colisión
        ApplyImpact(normal * 3f);

        // Ajustar la componente Y de la velocidad a cero para evitar movimientos en esa dirección
        velocity.y = 0f;
    }

    void ApplyImpact(Vector3 impactForce)
    {
        // Aplicar la fuerza de impacto a la masa del resorte
        velocity += impactForce / mass;
    }


    private void StartSimulation()
    {
        isSimulating = true;
    }
}
