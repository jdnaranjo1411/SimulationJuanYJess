using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    public Transform CameraTransform;
    private ObjInteract CurrentObjInteract;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Vector3 moveDirection = CameraTransform.forward * moveSpeed;
        Vector3 moveDirection = CameraTransform.forward * moveZ + CameraTransform.right * moveX;
        moveDirection.y = 0;

        rb.velocity = moveDirection.normalized * moveSpeed;

        if(Input.GetKey(KeyCode.E) && CurrentObjInteract)
        {
            CurrentObjInteract.Interact();
        }
    }

    public void SetInteractableObject(ObjInteract intObj)
    {
        CurrentObjInteract = intObj;
    }
}
