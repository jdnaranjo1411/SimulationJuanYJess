using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabello : MonoBehaviour
{
    [SerializeField] float k;
    [SerializeField] float restLength;

    // Objeto para el resorte
    [SerializeField] Cabello objResorte;

    [SerializeField] float mass;
    [SerializeField] Vector3 velocity = Vector3.zero;

    // Lista de resortes conectados
    public List<Cabello> resortesConectados = new List<Cabello>();

    private bool isSimulating = false;
    private GameObject player; // Referencia al jugador
    public float activationRange = 2f;
    private string interactMessage = "Presiona 'E' para interactuar";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Buscar el jugador por etiqueta
    }

    void Update()
    {
        // Verificar si el jugador está dentro del rango de activación
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSimulation(); // Iniciar la simulación cuando se presione la tecla "E"
            }
        }

        if (isSimulating)
        {
            // Actualizar la simulación si se está llevando a cabo
            Simulate(Time.deltaTime, 0.1f, -9.81f); // Valores de ejemplo para h, friction y gravity
        }
    }

    void OnGUI()
    {
        // Verificar si el jugador está dentro del rango de activación
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            // Mostrar el mensaje de interacción en la pantalla
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), interactMessage);
        }
    }

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

        if (objResorte != null)
        {
            Vector3 forceFromConnected = CalculateForceFromConnected(objResorte);
            totalForce += forceFromConnected;
        }

        return totalForce;
    }

    Vector3 CalculateForceFromConnected(Cabello connectedResorte)
    {
        Vector3 displacement = transform.position - connectedResorte.transform.position;
        float currentLength = displacement.magnitude;
        Vector3 springForce = k * (restLength - currentLength) * displacement.normalized;

        return springForce;
    }

    private void StartSimulation()
    {
        isSimulating = true;
    }
}

