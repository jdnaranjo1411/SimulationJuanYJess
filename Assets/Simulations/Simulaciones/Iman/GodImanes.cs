using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodImanes : MonoBehaviour
{
    private float h = 0.01f;
    private float friccion = 0.02f;
    private float gravedad = -9.8f;
    public float fuerzaImanes;

    //METODOS

    public List<Iman> imanes = new List<Iman>();

    public GameObject caja;

    private bool simulacionIniciada = false;

    void Start()
    {
        Iman[] imanesEnEscena = FindObjectsOfType<Iman>();
        imanes.AddRange(imanesEnEscena);
    }

    void Update()
    {
            foreach (Iman iman in imanes)
            {
                foreach (Iman otroIman in imanes)
                {
                    if (iman != otroIman)
                    {
                        // Calcular la fuerza entre el imán actual y otro imán
                        Vector3 fuerza = iman.CalcularFuerza(otroIman.transform.position, otroIman.polaridad, h, friccion, fuerzaImanes);

                        // Aplicar la fuerza al imán actual
                        iman.AplicarFuerza(fuerza, gravedad);
                    }
                }
            }
        }
}
