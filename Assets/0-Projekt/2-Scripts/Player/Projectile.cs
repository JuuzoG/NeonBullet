using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject munitionDropPrefab;

    private Rigidbody rb;
    private float lifetime = 2f;

    public float speed = 5f;

    private float damage = 2f;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;

        Player player = owner.GetComponent<Player>();

        if (player != null && player.stats != null)
        {
            damage = player.stats.damage;

            Debug.Log("[PROJECTILE] Damage: " + damage);
        }
        else
        {
            Debug.LogWarning("[PROJECTILE] Player or PlayerStats not found.");
        }
    }

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
        if (collision.gameObject == owner)
            return;

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

            Debug.Log(
                "[PROJECTILE] Hit " +
                collision.gameObject.name +
                " for " +
                damage +
                " damage."
            );
        }
        else
        {
            Debug.Log(
                "[PROJECTILE] Hit " +
                collision.gameObject.name +
                " but it has no IDamageable."
            );
        }

        EndTravel();
    }

    void EndTravel()
    {
        if (munitionDropPrefab != null)
        {
            Instantiate(
                munitionDropPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
