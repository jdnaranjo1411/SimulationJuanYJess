using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tela : MonoBehaviour
{
    //Traer valores de god
    private God godScript;
    private float k;
    private float restLength;
    private Vector3 velocity;

    //Objeto para el resorte
    [SerializeField] Tela objTela;

    public float mass = 1;

    // Lista de resortes conectados
    public List<Tela> TelaConectada = new List<Tela>();

    //mover resortes
    private bool isDragging = false;
    private Vector3 offset;

    void Start()
    {
        // Obtener una referencia al script God
        godScript = FindObjectOfType<God>();

        // Verificar si se encontró el script God
        if (godScript == null)
        {
            Debug.LogError("No se encontró el script God en la escena.");
            return;
        }

        // Obtener los valores de k, restLength y velocity desde el script God
        k = godScript.k;
        restLength = godScript.restLength;
        velocity = godScript.velocity;
    }


    public void Simulate(float h, float friction, float gravity)
    {
        if (!objTela) return;

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
        if (!objTela) return Vector3.zero;

        Vector3 totalForce = Vector3.zero;
        Vector3 displacement;

        displacement = transform.position - objTela.transform.position;

        //Resorte actual

        float currentLength = displacement.magnitude;
        // Ley de Hooke (F = -k * (longitud actual - longitud natural))
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;
        totalForce += springForce;

        if (objTela != null)
        {
            Vector3 forceFromConnectes = CalculateForceFromConnected(objTela);
            totalForce += forceFromConnectes;
        }

        return totalForce;
    }

    Vector3 CalculateForceFromConnected(Tela connectedResorte)
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

            Vector3 positionOffset = newPosition - transform.position;

            transform.position = newPosition;

            foreach (Tela TelaConectada in TelaConectada)
            {
                Vector3 connectedNewPosition = TelaConectada.transform.position + positionOffset;
                connectedNewPosition.z = TelaConectada.transform.position.z;
                TelaConectada.transform.position = connectedNewPosition;
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
