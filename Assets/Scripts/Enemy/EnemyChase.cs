using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform target;          // Player
    public float chaseRange = 12f;    // Pursuit range
    public float updateRate = 0.15f;  // Update frequency

    private NavMeshAgent agent;
    private float nextUpdate;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Automatically find Player
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (GameManager.IsGameOver) return;
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > chaseRange)
        {
            // Stop when it goes beyond the limit
            agent.isStopped = true;
            return;
        }

        // Within the range, pursue
        agent.isStopped = false;

        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + updateRate;
            agent.SetDestination(target.position);
        }
    }
}