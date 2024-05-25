using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.tvOS;
using static UnityEditor.PlayerSettings;

public class Iman : MonoBehaviour
{
    //VARIABLES

    //Definir la polaridad del iman
    public enum Polaridad
    {
        Positivo,
        Negativo
    }

    //Asignar la selección de la polaridad a una variable
    public Polaridad polaridad;

    //Distancia para que los imanes no se clipeen
    private float distanciaMinima = 1.0f;


    //MÉTODOS

    //  FUNCIONAMIENTO IMÁN

    // Método para calcular la fuerza entre dos imanes
    public Vector3 CalcularFuerza(Vector3 posiciónOtroImán, Polaridad polaridadOtroImán, float h, float friccion, float fuerzaImanes)
    {
        //Se calcula la distancia entre los imanes
        float distancia = Vector3.Distance(transform.position, posiciónOtroImán);


        //Se asigna el atraer para las polaridades opuestas
        bool atraer = (polaridad == Polaridad.Positivo && polaridadOtroImán == Polaridad.Negativo) || (polaridad == Polaridad.Negativo && polaridadOtroImán == Polaridad.Positivo);

        //Ley de coulomb F= (k*q1*q1)/r*r
        float fuerza = fuerzaImanes / (distancia * distancia);
        
        //Si se repelen, la fuerza toma el lado contrario
        if (!atraer)
        {
            fuerza *= -1;
        }

        //Se calculan las direcciones a tomar
        Vector3 dirección = posiciónOtroImán - transform.position;
        dirección.Normalize();


        Vector3 fuerzaFinal = dirección * fuerza;


        // Aplicar efectos adicionales como h y fricción
        fuerzaFinal *= h;
        fuerzaFinal -= friccion * fuerzaFinal;

        // Si los imanes están muy cerca, evitan clippeos
        if (distancia < distanciaMinima)
        {
            Vector3 ajuste = dirección * (distanciaMinima - distancia) / 2f;
            transform.position -= ajuste;
        }

        return fuerzaFinal;
    }

    // Método para aplicar fuerza al imán
    public void AplicarFuerza(Vector3 fuerza, float gravedad)
    {
            // Aplica fuerza
            transform.position += (fuerza + Vector3.down * gravedad) * Time.deltaTime;

            // Detecta colisión con el plano
            if (transform.position.y < transform.localScale.y / 2) // Si la posición Y del imán está por debajo del plano
            {
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z); // Ajusta la posición Y del imán para que la esfera quede completamente sobre el plano
            }

    }
}