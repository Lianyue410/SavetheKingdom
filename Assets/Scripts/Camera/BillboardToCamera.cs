using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    public bool lockX = true; // Whether to lock the rotation of the X-axis 
    public bool lockZ = true; // Whether to lock the rotation of the Z-axis 

    private Transform cam;

    private void LateUpdate()
    {
        // Obtain the main camera
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }

        Vector3 dir = transform.position - cam.position;
        Quaternion look = Quaternion.LookRotation(dir);

        Vector3 e = look.eulerAngles;
        if (lockX) e.x = 0f;
        if (lockZ) e.z = 0f;

        transform.rotation = Quaternion.Euler(e);
    }
}