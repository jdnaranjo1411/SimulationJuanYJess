using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Velociity;
    public float Radius;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position;
        Radius = transform.localScale.y * 0.5f;
    }
}
