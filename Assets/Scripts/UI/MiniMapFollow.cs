using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow")]
    public float height = 15f;
    public bool rotateWithTarget = true;

    [Header("Optional Offset")]
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position + offset;
        pos.y = target.position.y + height;

        transform.position = pos;

        if (rotateWithTarget)
        {
            // The top view angle is generally related only to the Y-axis
            Vector3 e = transform.eulerAngles;
            e.y = target.eulerAngles.y;
            transform.eulerAngles = e;
        }
        else
        {
            // Keep facing upwards and downward
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}