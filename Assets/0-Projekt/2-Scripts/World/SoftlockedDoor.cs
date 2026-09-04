using UnityEngine;

public class SoftlockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject Door;
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(Door);
        }
    }
}
