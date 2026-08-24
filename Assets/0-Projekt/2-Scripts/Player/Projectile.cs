using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject munitionDropPrefab;

    private Rigidbody rb;
    private float lifetime = 2f;
    public float speed = 5f;
    private float damage = 2f;

    private GameObject owner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
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
        if (collision.gameObject == owner) return;

        IDamageable damageable =
            collision.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Vector3 dir = transform.forward;

            DamageInfo info = new DamageInfo(
                damage,
                owner,
                collision.GetContact(0).point,
                dir
            );

            damageable.TakeDamage(info);
        }

        EndTravel();
    }

    void EndTravel()
    {
        if (munitionDropPrefab != null)
            Instantiate(munitionDropPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}