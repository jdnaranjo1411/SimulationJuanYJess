using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCaidaLibre : MonoBehaviour
{
    private float h;
    private float friction;
    private float gravity;
    public GameObject[] GobjCaidaLibre;

    public void Initialize(float h, float friction, float gravity)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        GobjCaidaLibre = GameObject.FindGameObjectsWithTag("CaidaLibre");
    }

    public void Simulate()
    {
        foreach (GameObject obj in GobjCaidaLibre)
        {
            obj.GetComponent<rebote>().Shoot(); // Llamar al método LanzarBola en lugar de Shoot
        }
    }
}
