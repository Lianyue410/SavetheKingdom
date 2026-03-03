using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public float hitCooldown = 0.8f;

    private float nextHitTime;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < nextHitTime) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                nextHitTime = Time.time + hitCooldown;
            }
        }
    }
    void Update()
    {
        if (GameManager.IsGameOver) return;
    }
}