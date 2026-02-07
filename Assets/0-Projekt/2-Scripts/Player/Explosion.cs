using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float duration = 2f;
    private int damage = 5;
    private float knockbackPower = 8;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            
            Vector3 direction = other.transform.position - transform.position;
            Debug.Log(direction.normalized);
            enemy.RecieveHit(damage, direction.normalized, knockbackPower);
        }
    }


}
