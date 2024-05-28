using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera _cam;

    public enum CAMERA_TYPE { FREE_LOOK, LOCKED }

    public CAMERA_TYPE type = CAMERA_TYPE.FREE_LOOK;

    [Range(0.1f, 2.0f)]
    public float sensitivity = 1.0f;
    public bool invertXAxis = false;
    public bool invertYAxis = false;

    public Transform lookAt;

    #region Camera Transitions
    private bool inTransition;
    private CameraState startState;
    private CameraState endState;
    private float transitionTime = 0.0f;
    private float transitionDuration;

    public Transform aimCam;
    private struct CameraState
    {
        public Vector3 position;
        public Vector3 rotation;
        public Transform lookAt;
    }
    #endregion

    private void Awake()
    {
        if (type == CAMERA_TYPE.LOCKED)
        {
            _cam.transform.parent = transform;
        }
    }

    private void FixedUpdate()
    {
        if (!inTransition)
        {
            HandleCameraMovement();
        }
        else
        {
            HandleCameraTransition();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TransitionTo(aimCam.position, aimCam.rotation.eulerAngles, lookAt, 1.5f);
        }
    }

    private void HandleCameraMovement()
    {
        // Read input
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        // Settings
        h = (invertXAxis) ? -h : h;
        v = (invertYAxis) ? -v : v;

        // Horizontal movement
        if (h != 0)
        {
            if (type == CAMERA_TYPE.LOCKED)
                transform.Rotate(Vector3.up, h * 90 * sensitivity * Time.deltaTime);
            else if (type == CAMERA_TYPE.FREE_LOOK)
                _cam.transform.RotateAround(transform.position, transform.up, h * 90 * sensitivity * Time.deltaTime);
        }

        // Vertical movement with clamping
        if (v != 0)
        {
            float desiredAngleX = _cam.transform.eulerAngles.x + v * 90 * sensitivity * Time.deltaTime;
            if (desiredAngleX < 180f) desiredAngleX = Mathf.Clamp(desiredAngleX, 0f, 80f);
            else desiredAngleX = Mathf.Clamp(desiredAngleX, 360f - 80f, 360f);

            _cam.transform.RotateAround(transform.position, transform.right, v * 90 * sensitivity * Time.deltaTime);
        }

        _cam.transform.LookAt(lookAt);

        // Fix Z-rotation issues
        Vector3 ea = _cam.transform.rotation.eulerAngles;
        _cam.transform.rotation = Quaternion.Euler(new Vector3(ea.x, ea.y, 0));
    }

    private void HandleCameraTransition()
    {
        float t = (Time.time - transitionTime) / transitionDuration;
        _cam.transform.position = Vector3.Lerp(startState.position, endState.position, t);
        _cam.transform.eulerAngles = Vector3.Lerp(startState.rotation, endState.rotation, t);
        _cam.transform.LookAt(endState.lookAt);

        if (t >= 1)
            inTransition = false;
    }

    public void TransitionTo(Vector3 finalPosition, Vector3 finalRotation, Transform finalLookAt, float duration)
    {
        startState = new CameraState
        {
            position = _cam.transform.position,
            rotation = _cam.transform.rotation.eulerAngles,
            lookAt = lookAt
        };

        endState = new CameraState
        {
            position = finalPosition,
            rotation = finalRotation,
            lookAt = finalLookAt
        };

        transitionTime = Time.time;
        transitionDuration = duration;
        inTransition = true;
    }

    public Camera GetCamera() { return _cam; }
}
