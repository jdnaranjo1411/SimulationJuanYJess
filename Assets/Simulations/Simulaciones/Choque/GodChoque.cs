using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodChoque : MonoBehaviour
{
    private float h = 0.01f;
    private float friction = 5f;
    private float gravity = -9.8f;
    public GameObject[] GobjChoque;

    public void Initialize(float h, float friction, float gravity)
    {
        this.h = h;
        this.friction = friction;
        this.gravity = gravity;
        GobjChoque = GameObject.FindGameObjectsWithTag("Choque");
    }

    public void Simulate()
    {
        for (int i = 0; i < GobjChoque.Length - 1; i++)
        {
            Bolita bolita1 = GobjChoque[i].GetComponent<Bolita>();

            for (int j = i + 1; j < GobjChoque.Length; j++)
            {
                Bolita bolita2 = GobjChoque[j].GetComponent<Bolita>();

                if (bolita1 != null && bolita2 != null)
                {
                    bolita1.Shoot(h, friction, gravity, bolita2);
                    bolita2.Shoot(h, friction, gravity, bolita1);
                }
            }
        }
    }
}
