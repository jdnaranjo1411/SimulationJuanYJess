using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Velocity;
    public float Radius;
    public float InitialScale = 1f; // Nuevo campo para definir el tamaño inicial

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * InitialScale; // Ajustar la escala inicial de la esfera
        Radius = InitialScale * 0.5f; // Calcular el radio inicial basado en la escala
    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position;
        Radius = transform.localScale.y * 0.5f; // Actualizar el radio en base a la escala actual
    }
}
