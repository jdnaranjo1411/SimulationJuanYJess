using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 5;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float tension = 1f;
    [SerializeField] private float damping = 0.1f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float friction = 0.5f; // Variable de fricción editable
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject caja;
    [SerializeField] private float nodeScale = 1f; // Tamaño de los nodos

    private Vector3 highBoxBounds;
    private Vector3 lowBoxBounds;
    private GameObject[,] nodes;
    private Vector3[,] velocities;
    private float sphereRadius; // Radio de la esfera de la hitbox
    private float k;

    private void Start()
    {
        highBoxBounds = caja.transform.position + caja.transform.localScale * 0.5f;
        lowBoxBounds = caja.transform.position - caja.transform.localScale * 0.5f;

        // Inicializa los nodos y velocidades
        InitializeCloth();

        // Calcular el radio de la esfera de la hitbox
        sphereRadius = hitbox.transform.localScale.x * 0.5f;

        // Calcular la constante del resorte
        k = tension;
    }

    private void Update()
    {
        // Simula la tela
        SimulateCloth();
    }

    private void InitializeCloth()
    {
        nodes = new GameObject[rows, columns];
        velocities = new Vector3[rows, columns];

        // Obtener la posición inicial a partir de la posición del objeto que contiene este script
        Vector3 startPosition = transform.position;

        // Crea los nodos de la tela
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject node = Instantiate(nodePrefab);
                node.transform.position = startPosition + new Vector3(i * spacing, 5, j * spacing);
                node.transform.localScale = Vector3.one * nodeScale; // Ajustar la escala del nodo
                nodes[i, j] = node;
                velocities[i, j] = Vector3.zero;
                node.GetComponent<Renderer>().material.color = Color.blue;
                Destroy(node.GetComponent<SphereCollider>());
            }
        }
    }

    private void SimulateCloth()
    {
        // Detección de colisiones con la caja y la hitbox
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 nodePosition = nodes[i, j].transform.position;

                // Comprueba colisiones con la caja
                if (CheckCollisionWithBox(nodePosition))
                {
                    velocities[i, j] = Vector3.zero;
                }

                // Comprueba colisiones con la hitbox
                if (CheckCollisionWithHitbox(nodePosition))
                {
                    velocities[i, j] = Vector3.zero;
                }
            }
        }

        // Aplica la gravedad a los nodos
        ApplyGravity();

        // Calcula las fuerzas de la tela (tensión y resortes)
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calcula la fuerza de tensión en los nodos vecinos
                Vector3 tensionForce = CalculateTension(i, j);

                // Calcula la fuerza de los resortes entre nodos adyacentes
                Vector3 springForce = CalculateSpringForce(i, j);

                // Calcula la fuerza de fricción
                Vector3 frictionForce = -velocities[i, j] * friction;

                // Aplica las fuerzas
                velocities[i, j] += (tensionForce + springForce + frictionForce) * Time.deltaTime;

                // Aplica el factor de amortiguación
                velocities[i, j] *= 1 - damping * Time.deltaTime;

                // Actualiza la posición del nodo
                nodes[i, j].transform.position += velocities[i, j] * Time.deltaTime;
            }
        }
    }

    private void ApplyGravity()
    {
        // Aplica la gravedad a los nodos
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Solo aplica gravedad si el nodo no está dentro de la caja
                Vector3 nodePosition = nodes[i, j].transform.position;
                if (!CheckCollisionWithBox(nodePosition))
                {
                    velocities[i, j] += Vector3.down * gravity * Time.deltaTime;
                }
            }
        }
    }

    private bool CheckCollisionWithBox(Vector3 nodePosition)
    {
        // Comprueba si la posición del nodo está dentro del AABB de la caja
        return (nodePosition.x >= lowBoxBounds.x && nodePosition.x <= highBoxBounds.x &&
                nodePosition.y >= lowBoxBounds.y && nodePosition.y <= highBoxBounds.y &&
                nodePosition.z >= lowBoxBounds.z && nodePosition.z <= highBoxBounds.z);
    }

    private bool CheckCollisionWithHitbox(Vector3 nodePosition)
    {
        // Calcula la distancia entre el nodo y el centro de la esfera de la hitbox
        float distance = Vector3.Distance(nodePosition, hitbox.transform.position);

        // Comprueba si la distancia es menor que el radio de la esfera
        return distance <= sphereRadius;
    }

    private Vector3 CalculateTension(int i, int j)
    {
        Vector3 force = Vector3.zero;

        // Calcula la fuerza de tensión en función de las posiciones de los nodos vecinos

        // Nodo superior
        if (i > 0)
        {
            Vector3 distance = nodes[i - 1, j].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo inferior
        if (i < rows - 1)
        {
            Vector3 distance = nodes[i + 1, j].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo izquierdo
        if (j > 0)
        {
            Vector3 distance = nodes[i, j - 1].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo derecho
        if (j < columns - 1)
        {
            Vector3 distance = nodes[i, j + 1].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        return force;
    }

    private Vector3 CalculateSpringForce(int i, int j)
    {
        Vector3 force = Vector3.zero;

        // Ley de Hooke: F = -k * x

        // Constante del resorte
        float k = tension;

        // Nodo superior
        if (i > 0)
        {
            Vector3 distance = nodes[i - 1, j].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo inferior
        if (i < rows - 1)
        {
            Vector3 distance = nodes[i + 1, j].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo izquierdo
        if (j > 0)
        {
            Vector3 distance = nodes[i, j - 1].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        // Nodo derecho
        if (j < columns - 1)
        {
            Vector3 distance = nodes[i, j + 1].transform.position - nodes[i, j].transform.position;
            force += k * distance;
        }

        return force;
    }
}
