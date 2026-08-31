using UnityEngine;

public class AllEnemysDead : MonoBehaviour
{
    [SerializeField] private TriggerRelay trigger;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject door;

    private int enemyCount;

    private void Start()
    {
        if (trigger != null)
        {
            trigger.OnEnter += HandleTriggerEnter;
            trigger.OnExit += HandleTriggerExit;
        }
    }

    private void OnDestroy()
    {
        if (trigger != null)
        {
            trigger.OnEnter -= HandleTriggerEnter;
            trigger.OnExit -= HandleTriggerExit;
        }
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemyCount++;
        }
    }

    private void HandleTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemyCount--;

            if (enemyCount <= 0 && door != null)
            {
                Destroy(door);
            }
        }
    }
}