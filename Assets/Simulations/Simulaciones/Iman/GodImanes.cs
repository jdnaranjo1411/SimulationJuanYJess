using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodImanes : MonoBehaviour
{
    private float h;
    private float friction;
    private float gravity;
    public float fuerzaImanes;
    public List<Iman> imanes = new List<Iman>();

    public void Initialize(float h, float friction, float gravity)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        Iman[] imanesEnEscena = FindObjectsOfType<Iman>();
        imanes.AddRange(imanesEnEscena);
    }

    public void Simulate()
    {
        foreach (Iman iman in imanes)
        {
            foreach (Iman otroIman in imanes)
            {
                if (iman != otroIman)
                {
                    Vector3 fuerza = iman.CalcularFuerza(otroIman.transform.position, otroIman.polaridad, h, friction, fuerzaImanes);
                    iman.AplicarFuerza(fuerza, gravity);
                }
            }
        }
    }
}
