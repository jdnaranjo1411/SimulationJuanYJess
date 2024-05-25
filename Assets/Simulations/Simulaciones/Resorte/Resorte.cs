using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Resorte : MonoBehaviour
{
    [SerializeField] float k; // Constante del resorte
    [SerializeField] float restLength; // Longitud natural del resorte
    [SerializeField] float mass; // Masa de la bolita
    [SerializeField] Vector3 velocity = Vector3.zero; // Velocidad de la bolita
    [SerializeField] Transform anchorPoint; // Punto de anclaje (la caja sujeta al suelo)

    void Update()
    {

    }

    // M�todo para calcular la fuerza del resorte
    public void Simulate(float h, float friction, float gravity)
    {
        Vector3 force = CalculateSpringForce();

        Vector3 acceleration = force / mass;

        // Limitar la aceleraci�n solo a los ejes X y Z
        acceleration.x = 0f;
        acceleration.z = 0f;

        // Aplicar la gravedad solo en el eje Y
        acceleration.y += gravity;

        // Aplicar la fricci�n solo en los ejes X y Z
        velocity.x -= friction * velocity.x;
        velocity.z -= friction * velocity.z;

        // Aplicar la aceleraci�n y la velocidad
        velocity += acceleration * h;
        transform.position += velocity * h;

        // Limitar la velocidad solo a los ejes X y Z
        velocity.y = 0f;

        // Ajustar la posici�n para asegurarse de que el resorte est� por encima del punto de anclaje
        transform.position = anchorPoint.position + Vector3.up * restLength;
    }

    Vector3 CalculateSpringForce()
    {
        Vector3 totalForce = Vector3.zero;

        // Calcula el vector de desplazamiento entre el objeto y el punto de anclaje
        Vector3 displacement = transform.position - anchorPoint.position;
        float currentLength = displacement.magnitude;

        // Calcula la fuerza del resorte seg�n la ley de Hooke
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;
        totalForce += springForce;

        return totalForce;
    }
}