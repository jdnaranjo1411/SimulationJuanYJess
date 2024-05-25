using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebote : MonoBehaviour
{
    [SerializeField] Vector3 initialVel;
    [SerializeField] float DampingFactor;
    [SerializeField] float mass;

    Vector3 Pos;
    Vector3 Vel;
    Vector3 Force;

    private bool collision;
    private bool isSimulating; // Variable para controlar si se está simulando o no

    // Rango de activación de la simulación
    public float activationRange = 2f;
    private GameObject player; // Referencia al jugador

    private string interactMessage = "Presiona 'E' para interactuar";


    // Start is called before the first frame update
    void Start()
    {
        Pos = transform.position;
        Vel = initialVel;
        collision = false;
        isSimulating = false; // Inicialmente no se está simulando

        player = GameObject.FindGameObjectWithTag("Player"); // Buscar el jugador por etiqueta
    }

    void Update()
    {
        // Verificar si el jugador está dentro del rango de activación
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSimulation(); // Iniciar la simulación cuando se presione la tecla "I"
            }
        }

        if (isSimulating)
        {
            // Actualizar la simulación si se está llevando a cabo
            Simulate(Time.deltaTime);
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

        Vector3 frictionForce = new Vector3(-friction * Vel.normalized.x, 0, 0); // Aplicar fricción solo en el eje X
        Vel += (frictionForce / mass) * Time.deltaTime;
        Vel += g * Time.deltaTime;
    }

    public void DetectCollision()
    {
        collision = false;

        if (Pos.y <= 0.05f)
        {
            Pos.y = 0.05f; // Establecer la posición Y en 0 para evitar que atraviese el suelo
            Vel.y = -Vel.y * DampingFactor; // Invertir la velocidad en Y y aplicar un factor de amortiguación
            collision = true;
        }
    }

    public void Shoot()
    {

        Pos = transform.position;
        isSimulating = true;
    }

    private void StartSimulation()
    {
        // Inicializar la posición y la velocidad
        Pos = transform.position;
        Vel = initialVel;

        // Activar la simulación
        isSimulating = true;
    }

    private void Simulate(float h)
    {
        CalculateForce(Physics.gravity.y, 0); // Puedes ajustar la fricción si es necesario
        Vector3 acceleration = Force / mass;
        Vel += h * acceleration;
        Pos += h * Vel;



        DetectCollision(); // Detectar colisión con el suelo u otros objetos
        LoadPos(); // Cargar la nueva posición
    }

    public void LoadPos()
    {
        transform.position = Pos;
    }
}