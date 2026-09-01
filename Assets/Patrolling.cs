using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float pointReachedDistance = 0.5f;
    public bool randomPatrol = false;

    private NavMeshAgent agent;

    private int currentPointIndex = -1;

    public bool IsPatrolling { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartPatrol()
    {
        if (PatrolManager.Instance == null)
            return;

        if (PatrolManager.Instance.patrolPoints == null ||
            PatrolManager.Instance.patrolPoints.Length == 0)
            return;

        IsPatrolling = true;
        agent.isStopped = false;

        // Find the closest patrol point when starting patrol.
        Transform closestPoint =
            PatrolManager.Instance.GetClosestPoint(transform.position);

        if (closestPoint != null)
        {
            currentPointIndex =
                System.Array.IndexOf(
                    PatrolManager.Instance.patrolPoints,
                    closestPoint
                );

            GoToNextPoint();
        }
    }

    public void StopPatrol()
    {
        IsPatrolling = false;
    }

    private void Update()
    {
        if (!IsPatrolling)
            return;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= pointReachedDistance)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        PatrolManager manager = PatrolManager.Instance;

        if (manager == null)
            return;

        Transform nextPoint;

        if (randomPatrol)
        {
            nextPoint = manager.GetRandomPoint();
        }
        else
        {
            nextPoint = manager.GetNextPoint(currentPointIndex);
        }

        if (nextPoint == null)
            return;

        // Keep track of the point
        currentPointIndex =
            System.Array.IndexOf(
                manager.patrolPoints,
                nextPoint
            );

        agent.SetDestination(nextPoint.position);
    }
}