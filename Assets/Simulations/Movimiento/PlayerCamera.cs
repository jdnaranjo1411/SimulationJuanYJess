using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Vel = 100f;
    float RotX = 0f;
    public Transform Player;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * Vel * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * Vel * Time.deltaTime;

        RotX -= MouseY;
        RotX = Mathf.Clamp(RotX, -45f, 45f);
        transform.localRotation = Quaternion.Euler(RotX, 0, 0);

        Player.Rotate(Vector3.up * MouseX);
    }
}
