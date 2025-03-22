using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float wanderTimer = 3f;
    public Transform centerPoint; // Assign this to the center of the pen

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(centerPoint.position, wanderRadius, NavMesh.AllAreas);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir.y = 0; // Keep it on the same level
        randDir += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, dist, layermask);
        return navHit.position;
    }
}
