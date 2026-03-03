using UnityEngine;
using UnityEngine.EventSystems;
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Rotate (Hold RMB)")]
    public float mouseXSensitivity = 3f;
    public float mouseYSensitivity = 2f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 2.5f;
    public float maxDistance = 8f;
    public float zoomSpeed = 4f;

    [Header("Smooth")]
    public float followSmooth = 12f;

    private float yaw;
    private float pitch;

    private bool inputEnabled = true;
    private bool isRotating; 

    private void Start()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;

        SetCursor(false);
    }

    private void Update()
    {
        if (!inputEnabled) return;

        // Only when you hold down the right button will the mouse be locked and rotated
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            SetCursor(true); // Lock and hide
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            SetCursor(false); // Unlock and display
        }

        // Only allow the main camera to be zoomed when the mouse is not on any UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > 0.0001f)
            {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }
        }

        // Rotation is only activated when the right mouse button is held down. Mouse X/Y is read
        if (isRotating)
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            yaw += mx * mouseXSensitivity;
            pitch -= my * mouseYSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position + targetOffset - (rot * Vector3.forward * distance);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            1f - Mathf.Exp(-followSmooth * Time.deltaTime)
        );
        transform.rotation = rot;
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        if (!enabled)
        {
            isRotating = false;
            SetCursor(false);
        }
    }

    public Quaternion GetYawRotation()
    {
        return Quaternion.Euler(0f, yaw, 0f);
    }

    private void SetCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}