using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage;

    [Range(0f, 1f)]
    public float debugFill = 1f;

    public void SetFill01(float t)
    {
        if (fillImage == null) return;
        fillImage.fillAmount = Mathf.Clamp01(t);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying && fillImage != null)
            fillImage.fillAmount = Mathf.Clamp01(debugFill);
    }
#endif
}