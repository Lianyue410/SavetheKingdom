using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Camera miniMapCamera;
    public float zoomSpeed = 2f;
    public float minSize = 3f;
    public float maxSize = 20f;

    private bool isPointerOver;

    void Update()
    {
        if (!isPointerOver) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.0001f)
        {
            miniMapCamera.orthographicSize -= scroll * zoomSpeed;
            miniMapCamera.orthographicSize =
                Mathf.Clamp(miniMapCamera.orthographicSize, minSize, maxSize);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }
}