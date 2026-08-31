using UnityEngine;

public class spawnDoor : MonoBehaviour
{
    [Header("Hold Settings")]
    [SerializeField] private float Sec = 1;
    [SerializeField] private GameObject enemy;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private TriggerRelay trigger; 

    private int spawnedCount;
    private float yes;
    private bool inTrigger;

    private void Start()
    {
        yes = Sec;

        trigger.OnEnter += HandleTriggerEnter;
        
    }

    private void OnDestroy()
    {
        trigger.OnEnter -= HandleTriggerEnter;
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            inTrigger = true;
        }
    }

    private void Update()
    {
        if (inTrigger && (maxEnemies < 0 || spawnedCount < maxEnemies))
        {
            yes -= Time.deltaTime;

            if (yes <= 0)
            {
                Spawn();
                yes = Sec;
            }
        }
    }

    private void Spawn()
    {
        Instantiate(enemy);
        spawnedCount++;
    }
}