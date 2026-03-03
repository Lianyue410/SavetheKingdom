using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    public int hp;

    [Header("UI")]
    public TMP_Text hpText;  

    [Header("Invincible")]
    public float invincibleTime = 0.8f;
    private float invincibleUntil;

    void Start()
    {
        hp = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int dmg)
    {
        if (Time.time < invincibleUntil) return;

        hp -= dmg;
        if (hp < 0) hp = 0;

        invincibleUntil = Time.time + invincibleTime;

        UpdateUI();

        if (hp <= 0)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.Lose("HP=0");
        }
    }

    private void UpdateUI()
    {
        if (hpText != null)
            hpText.text = $"HP: {hp}/{maxHP}";
    }
}