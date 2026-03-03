using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 6.0f;
    public int damage = 1;          
    public float cooldown = 0.4f;
    public LayerMask enemyLayer;

    [Header("Rotate")]
    public float rotateSpeed = 20f;
    private Transform rotateTarget;
    private float rotateLockTimer;

    float cd;

    void Update()
    {

        if (GameManager.IsGameOver) return;
        cd -= Time.deltaTime;

        // Rotate after attack
        if (rotateTarget != null && rotateLockTimer > 0f)
        {
            rotateLockTimer -= Time.deltaTime;
            FaceTarget(rotateTarget);
        }

        if (Input.GetMouseButtonDown(0) && cd <= 0f)
        {
            cd = cooldown;
            DoAttack();
        }
    }

    void DoAttack()
    {
        Vector3 center = transform.position + transform.forward * 1.0f;
        Collider[] hits = Physics.OverlapSphere(center, attackRange, enemyLayer);

        foreach (var col in hits)
        {
            var eh = col.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                // First determine the orientation
                rotateTarget = eh.transform;
                rotateLockTimer = 0.12f;

                eh.TakeDamage(damage);
                break;
            }
        }
    }

    void FaceTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f; // Just change to Y
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }
}