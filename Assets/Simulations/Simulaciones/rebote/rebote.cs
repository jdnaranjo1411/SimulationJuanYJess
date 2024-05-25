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
    private bool isSimulating; // Variable para controlar si se est� simulando o no

    // Rango de activaci�n de la simulaci�n
    public float activationRange = 2f;
    private GameObject player; // Referencia al jugador

    private string interactMessage = "Presiona 'E' para interactuar";


    // Start is called before the first frame update
    void Start()
    {
        Pos = transform.position;
        Vel = initialVel;
        collision = false;
        isSimulating = false; // Inicialmente no se est� simulando

        player = GameObject.FindGameObjectWithTag("Player"); // Buscar el jugador por etiqueta
    }

    void Update()
    {
        // Verificar si el jugador est� dentro del rango de activaci�n
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSimulation(); // Iniciar la simulaci�n cuando se presione la tecla "I"
            }
        }

        if (isSimulating)
        {
            // Actualizar la simulaci�n si se est� llevando a cabo
            Simulate(Time.deltaTime);
        }
    }

    void OnGUI()
    {
        // Verificar si el jugador est� dentro del rango de activaci�n
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            // Mostrar el mensaje de interacci�n en la pantalla
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

        Vector3 frictionForce = new Vector3(-friction * Vel.normalized.x, 0, 0); // Aplicar fricci�n solo en el eje X
        Vel += (frictionForce / mass) * Time.deltaTime;
        Vel += g * Time.deltaTime;
    }

    public void DetectCollision()
    {
        collision = false;

        if (Pos.y <= 0.05f)
        {
            Pos.y = 0.05f; // Establecer la posici�n Y en 0 para evitar que atraviese el suelo
            Vel.y = -Vel.y * DampingFactor; // Invertir la velocidad en Y y aplicar un factor de amortiguaci�n
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
        // Inicializar la posici�n y la velocidad
        Pos = transform.position;
        Vel = initialVel;

        // Activar la simulaci�n
        isSimulating = true;
    }

    private void Simulate(float h)
    {
        CalculateForce(Physics.gravity.y, 0); // Puedes ajustar la fricci�n si es necesario
        Vector3 acceleration = Force / mass;
        Vel += h * acceleration;
        Pos += h * Vel;



        DetectCollision(); // Detectar colisi�n con el suelo u otros objetos
        LoadPos(); // Cargar la nueva posici�n
    }

    public void LoadPos()
    {
        transform.position = Pos;
    }
}