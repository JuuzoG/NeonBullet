using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject munitionDropPrefab;

    private Rigidbody rb;
    private float lifetime = 2f;
    private float speed = 5f;
    private float damage = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.linearVelocity = transform.forward * speed;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            EndTravel();
    }

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable =
            collision.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null)
            damageable.TakeDamage(damage);

        EndTravel();
    }

    void EndTravel()
    {
        if (munitionDropPrefab != null)
            Instantiate(munitionDropPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}