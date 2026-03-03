using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;

    private int currentHP;

    public int CurrentHP => currentHP;
    public float Normalized => maxHP <= 0 ? 0f : Mathf.Clamp01((float)currentHP / maxHP);

    public event Action<float> OnHpChanged;

    void Awake()
    {
        ResetHP();
    }

    void OnEnable()
    {
        ResetHP();
    }

    public void ResetHP()
    {
        if (maxHP < 1) maxHP = 1;
        currentHP = maxHP;
        OnHpChanged?.Invoke(Normalized);
    }

    void OnValidate()
    {
        if (!Application.isPlaying) return;
        ResetHP(); // Run editable
    }

    public void TakeDamage(int dmg)
    {
        if (dmg <= 0) return;
        if (currentHP <= 0) return;

        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;

        OnHpChanged?.Invoke(Normalized);

        if (currentHP == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}