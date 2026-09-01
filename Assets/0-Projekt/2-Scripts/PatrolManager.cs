using UnityEngine;
using System.Collections.Generic;

public class PatrolManager : MonoBehaviour
{
    public static PatrolManager Instance { get; private set; }

    [Header("Patrol Points")]
    public Transform[] patrolPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Transform GetNextPoint(int currentIndex)
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return null;

        int nextIndex = (currentIndex + 1) % patrolPoints.Length;

        return patrolPoints[nextIndex];
    }

    public Transform GetRandomPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return null;

        int randomIndex = Random.Range(0, patrolPoints.Length);

        return patrolPoints[randomIndex];
    }

    public Transform GetClosestPoint(Vector3 position)
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return null;

        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform point in patrolPoints)
        {
            if (point == null)
                continue;

            float distance = Vector3.Distance(position, point.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}
