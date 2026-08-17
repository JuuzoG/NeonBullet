using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;

    private GameObject owner;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner || other.CompareTag("Enemy")) return;

        GameObject hitObject = other.gameObject;
        Debug.Log("Triggered by: " + hitObject.name);

        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Vector3 dir = transform.forward;

            DamageInfo info = new DamageInfo(
                damage,
                owner,
                other.ClosestPoint(transform.position),
                dir
            );

            damageable.TakeDamage(info);
        }

        Destroy(gameObject);
    }
}