using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHPBarWorld : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 worldOffset = new Vector3(0f, 1.8f, 0f);

    private EnemyHealth enemyHealth;
    private Image fillImage;
    private TMP_Text hpText;
    private Transform barRoot;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null) { enabled = false; return; }
    }

    void Start()
    {
        if (hpBarPrefab == null) { enabled = false; return; }

        var barObj = Instantiate(hpBarPrefab);
        barRoot = barObj.transform;
        barRoot.position = transform.position + worldOffset;

        var fillTf = barRoot.Find("BG/Fill");
        if (fillTf != null) fillImage = fillTf.GetComponent<Image>();

        hpText = barRoot.GetComponentInChildren<TMP_Text>(true);

        enemyHealth.OnHpChanged += Refresh;
        Refresh(enemyHealth.Normalized);
    }

    void LateUpdate()
    {
        if (GameManager.IsGameOver) return;
        if (barRoot == null) return;
        barRoot.position = transform.position + worldOffset;
        if (Camera.main != null) barRoot.forward = Camera.main.transform.forward;
    }

    void Refresh(float normalized)
    {
        if (fillImage != null) fillImage.fillAmount = normalized;
        if (hpText != null) hpText.text = $"{enemyHealth.CurrentHP}/{enemyHealth.maxHP}";
    }

    void OnDestroy()
    {
        if (enemyHealth != null) enemyHealth.OnHpChanged -= Refresh;
        if (barRoot != null) Destroy(barRoot.gameObject);
    }
}