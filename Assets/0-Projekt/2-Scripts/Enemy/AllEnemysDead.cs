using System.Collections.Generic;
using UnityEngine;

public class AllEnemysDead : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject door;

    private readonly HashSet<Collider> seenThisFrame = new HashSet<Collider>();
    private bool everHadEnemy;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            seenThisFrame.Add(other);
        }
    }

    private void FixedUpdate()
    {
        if (seenThisFrame.Count > 0)
        {
            everHadEnemy = true;
        }
        else if (everHadEnemy && door != null)
        {
            Destroy(door);
            everHadEnemy = false;
        }

        seenThisFrame.Clear();
    }
}