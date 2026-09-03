using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrolling : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("How close the enemy needs to get before it considers the spot reached.")]
    [SerializeField] private float pointReachedDistance = 0.5f;

    [Tooltip("How far from the patrol point the enemy can wander.")]
    [SerializeField] private float wanderRadius = 5f;

    [Tooltip("Use random patrol points instead of moving through them in order.")]
    [SerializeField] private bool randomPatrol = false;

    [Header("Waiting")]
    [Tooltip("Minimum time the enemy waits at a random spot.")]
    [SerializeField] private float minWaitTime = 1.5f;

    [Tooltip("Maximum time the enemy waits at a random spot.")]
    [SerializeField] private float maxWaitTime = 4f;

    [Header("Spots Per Patrol Point")]
    [Tooltip("Minimum number of random spots to visit around each patrol point.")]
    [SerializeField] private int minSpotsPerPoint = 2;

    [Tooltip("Maximum number of random spots to visit around each patrol point.")]
    [SerializeField] private int maxSpotsPerPoint = 4;

    [Header("Patrol Speed")]
    [Tooltip("Minimum movement speed while patrolling.")]
    [SerializeField] private float minPatrolSpeed = 1.5f;

    [Tooltip("Maximum movement speed while patrolling.")]
    [SerializeField] private float maxPatrolSpeed = 2.5f;

    private NavMeshAgent agent;
    private int currentPointIndex = -1;
    private int spotsVisited;
    private int spotsToVisit;
    private float waitTimer;
    private bool waiting;
    private float normalSpeed;
    public bool IsPatrolling { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        normalSpeed = agent.speed;
    }

    public void StartPatrol()
    {
        if (agent == null)
            return;

        if (PatrolManager.Instance == null)
            return;

        if (PatrolManager.Instance.patrolPoints == null ||
            PatrolManager.Instance.patrolPoints.Length == 0)
            return;

        IsPatrolling = true;

        waiting = false;
        waitTimer = 0f;

        spotsVisited = 0;
        spotsToVisit = Random.Range(
            minSpotsPerPoint,
            maxSpotsPerPoint + 1
        );

        agent.isStopped = false;

        // Find the closest patrol point
        Transform closestPoint =
            PatrolManager.Instance.GetClosestPoint(
                transform.position
            );

        if (closestPoint != null)
        {
            currentPointIndex =
                System.Array.IndexOf(
                    PatrolManager.Instance.patrolPoints,
                    closestPoint
                );

            SetRandomPatrolSpeed();

            GoToRandomSpot();
        }
    }

    public void StopPatrol()
    {
        IsPatrolling = false;

        waiting = false;
        waitTimer = 0f;

        // Return to normal enemy movement speed.
        if (agent != null)
            agent.speed = normalSpeed;
    }

    private void Update()
    {
        if (!IsPatrolling)
            return;

        if (agent == null ||
            !agent.enabled ||
            !agent.isOnNavMesh)
            return;


        if (waiting)
        {
            agent.isStopped = true;

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false;

                spotsVisited++;

                if (spotsVisited >= spotsToVisit)
                {
                    MoveToNextPatrolPoint();
                }
                else
                {
                    GoToRandomSpot();
                }
            }

            return;
        }


        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= pointReachedDistance)
        {
            BeginWaiting();
        }
    }

    private void BeginWaiting()
    {
        waiting = true;
        waitTimer = Random.Range(
            minWaitTime,
            maxWaitTime
        );

        agent.isStopped = true;
        agent.ResetPath();
    }

    private void GoToRandomSpot()
    {
        if (!IsPatrolling)
            return;

        PatrolManager manager =
            PatrolManager.Instance;

        if (manager == null)
            return;

        if (manager.patrolPoints == null ||
            manager.patrolPoints.Length == 0)
            return;

        if (currentPointIndex < 0 ||
            currentPointIndex >= manager.patrolPoints.Length)
            return;

        Transform patrolPoint =
            manager.patrolPoints[currentPointIndex];

        if (patrolPoint == null)
            return;
        //try locating

        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle =
                Random.insideUnitCircle * wanderRadius;

            Vector3 randomPosition =
                patrolPoint.position +
                new Vector3(
                    randomCircle.x,
                    0f,
                    randomCircle.y
                );

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                randomPosition,
                out hit,
                wanderRadius,
                NavMesh.AllAreas))
            {
                agent.isStopped = false;

                agent.SetDestination(
                    hit.position
                );

                return;
            }
        }
        //retry

    }

    private void MoveToNextPatrolPoint()
    {
        PatrolManager manager =
            PatrolManager.Instance;

        if (manager == null)
            return;

        Transform nextPoint;

        if (randomPatrol)
        {
            nextPoint =
                manager.GetRandomPoint();
        }
        else
        {
            nextPoint =
                manager.GetNextPoint(
                    currentPointIndex
                );
        }

        if (nextPoint == null)
            return;

        currentPointIndex =
            System.Array.IndexOf(
                manager.patrolPoints,
                nextPoint
            );

        // Reset spot counter.
        spotsVisited = 0;

        // Pick a new number of spots for fun
        spotsToVisit = Random.Range(
            minSpotsPerPoint,
            maxSpotsPerPoint + 1
        );
        SetRandomPatrolSpeed();

        GoToRandomSpot();
    }

    private void SetRandomPatrolSpeed()
    {
        if (agent == null)
            return;

        agent.speed = Random.Range(
            minPatrolSpeed,
            maxPatrolSpeed
        );
    }
}
