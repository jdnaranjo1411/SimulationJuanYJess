using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Resorte : MonoBehaviour
{
    [SerializeField] float k;
    [SerializeField] float restLength;

    //Objeto para el resorte
    [SerializeField] Resorte objResorte;

    [SerializeField] float mass;
    [SerializeField] Vector3 velocity = Vector3.zero;

    // Lista de resortes conectados
    public List<Resorte> resortesConectados = new List<Resorte>();

    //mover resortes
    private bool isDragging = false;
    private Vector3 offset;

    public void Simulate(float h, float friction, float gravity)
    {
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
        Vector3 totalForce = Vector3.zero;

        //Resorte actual
        Vector3 displacement = transform.position - objResorte.transform.position;
        float currentLength = displacement.magnitude;
        // Ley de Hooke (F = -k * (longitud actual - longitud natural))
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;
        totalForce += springForce;

        if (objResorte != null)
        {
            Vector3 forceFromConnectes = CalculateForceFromConnected(objResorte);
            totalForce += forceFromConnectes;
        }
        return totalForce;
    }

    Vector3 CalculateForceFromConnected(Resorte connectedResorte)
    {
        Vector3 displacement = transform.position - connectedResorte.transform.position;
        float currentLength = displacement.magnitude;
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;

        return springForce;
    }

    //  MOVER RESORTES
    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        offset = transform.position - GetMouseWorldPosition();

    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition.z = transform.position.z;

            // Calcula la diferencia de posición entre el resorte principal y los resortes conectados
            Vector3 positionOffset = newPosition - transform.position;

            // Mueve el resorte principal
            transform.position = newPosition;

            // Mueve los resortes conectados
            foreach (Resorte connectedResorte in resortesConectados)
            {
                // Calcula la nueva posición de los resortes conectados
                Vector3 connectedNewPosition = connectedResorte.transform.position + positionOffset;
                connectedNewPosition.z = connectedResorte.transform.position.z;

                // Asigna la nueva posición a los resortes conectados
                connectedResorte.transform.position = connectedNewPosition;
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

}