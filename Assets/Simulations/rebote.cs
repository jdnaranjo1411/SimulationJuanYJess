using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebote : MonoBehaviour
{
    private Rigidbody rb;
    private float initialPosY;
    public float dampingFactor = 0.9f;
    private bool IsInverted = false;
    public Vector3 Gravity = new Vector3(0, -0.98f, 0);

    public float fuerzaInicial = 10f;
    public float angulo = 45f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosY = transform.position.y;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        LanzarBola();

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Gravity, ForceMode.Acceleration);

        if (transform.position.y <= 0.2f)
        {
            if (!IsInverted && rb.velocity.y < 0)
            {
                InvertGravity();
            }
        }
        if (transform.position.y < 0)
        {
            resetParticle();
        }
    }

    void InvertGravity()
    {
        rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y * dampingFactor, rb.velocity.z);
        IsInverted = true;

    }
    void resetParticle()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, rb.velocity.z);
        IsInverted = false;
    }

    void LanzarBola()
    {
        float anguloRad = angulo * Mathf.Deg2Rad;

        float velocidadInicialX = fuerzaInicial * Mathf.Cos(anguloRad);
        float velocidadInicialY = fuerzaInicial * Mathf.Sin(anguloRad);

        Vector3 velocidadInicial = new Vector3(velocidadInicialX, velocidadInicialY, 0f);
        rb.AddForce(velocidadInicial, ForceMode.VelocityChange);
    }

}