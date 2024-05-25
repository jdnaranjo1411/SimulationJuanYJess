using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodResorte : MonoBehaviour
{
    private float h;
    private float friction;
    private float gravity;
    private Resorte resorte; // Referencia al resorte
    private bool isSimulating = false; // Variable para controlar si se está simulando o no

    // Método público para inicializar el GodResorte
    public void Initialize(float h, float friction, float gravity, Resorte resorte = null)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        this.resorte = resorte; // Asignar el resorte proporcionado (o null si no se proporciona)
    }

    // Método para simular el movimiento del resorte
    public void Simulate()
    {
        // Verificar si se está simulando el movimiento del resorte
        if (!isSimulating) return;

        // Simular el movimiento del resorte
        if (resorte != null)
        {
            resorte.Simulate(h, friction, gravity); // Llamar al método Simulate del resorte
        }
    }
}
