using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject munitionDropPrefab;
    private Rigidbody rb;
    private float remainingLifetime = 2; 
    private float movementSpeed = 5;
    private float damage = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * movementSpeed;
        
        remainingLifetime -= Time.deltaTime;
        if (remainingLifetime <= 0)
        {
            EndTravel();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.RecieveHit(damage, Vector3.zero, 0);
        }    
        EndTravel();    
    }

    void EndTravel()
    {
        Instantiate(munitionDropPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
