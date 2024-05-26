using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCabello : MonoBehaviour
{

    private float h = 0.01f;
    private float friction =  0.2f;
    private float gravity = -9.8f;
    public List<Resorte> resortes = new List<Resorte>();

    void Start()
    {
        Resorte[] resortesEnEscena = FindObjectsOfType<Resorte>();
        resortes.AddRange(resortesEnEscena);
    }

    void Update()
    {
        foreach (Resorte resorte in resortes)
        {
            resorte.Simulate(h, friction, gravity);
        }
    }
}
