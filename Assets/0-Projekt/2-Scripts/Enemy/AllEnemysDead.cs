using UnityEngine;

public class AllEnemysDead : MonoBehaviour
{
    [SerializeField] private TriggerRelay trigger; 
    [SerializeField] private string Tag = "Enemy";
    [SerializeField] private GameObject door;

    private void Start()
    {
        if (trigger != null)
        {
            trigger.OnEnter += HandleTriggerEnter;
        }
    }

    private void OnDestroy()
    {
        if (trigger != null)
        {
            trigger.OnEnter -= HandleTriggerEnter;
        }
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(Tag) || other.CompareTag(Tag))
        {
            Debug.Log("");
        }
        else Destroy(door);
    }
}
