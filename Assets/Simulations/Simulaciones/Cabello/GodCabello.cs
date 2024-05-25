using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCabello : MonoBehaviour
{
    private float h;
    private float friction;
    private float gravity;
    public List<Resorte> resortes = new List<Resorte>();

    public void Initialize(float h, float friction, float gravity)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        Resorte[] resortesEnEscena = FindObjectsOfType<Resorte>();
        resortes.AddRange(resortesEnEscena);
    }

    public void Simulate()
    {
        foreach (Resorte resorte in resortes)
        {
            resorte.Simulate(h, friction, gravity);
        }
    }
}
