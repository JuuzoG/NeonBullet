using UnityEngine;

public class Railgun : MonoBehaviour
{
    [Header("Railgun Settings")]
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 2f;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayers;

    [Header("Beam")]
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private float beamDuration = 0.1f;

    [Header("Fire Point")]
    private Transform firePoint;

    void Start()
    {
        firePoint = GetComponent<Transform>();
    }
    public void Fire()
    {
        //Debug.Log("RAILGUN PEWW");

        Vector3 start = firePoint != null
            ? firePoint.position
            : transform.position;

        Vector3 direction = firePoint != null
            ? firePoint.forward
            : transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(
            start,
            direction,
            range,
            ~0,
            QueryTriggerInteraction.Collide
        );

        if (hits.Length == 0)
        {
            CreateBeam(
                start,
                start + direction * range
            );

            return;
        }
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Vector3 beamEnd = start + direction * range;

        foreach (RaycastHit hit in hits)
        {

            if (((1 << hit.collider.gameObject.layer) & wallLayers) != 0)
            {
                // Wall stops 
                beamEnd = hit.point;
                break;
            }

            IDamageable damageable =
                hit.collider.GetComponentInParent<IDamageable>();

            if (damageable == null)
            {
                damageable =
                    hit.collider.transform.root.GetComponentInChildren<IDamageable>();
            }

            if (damageable != null)
            {
                DamageInfo info = new DamageInfo(
                    damage,
                    gameObject,
                    hit.point,
                    direction
                );

                damageable.TakeDamage(info);

                //Debug.Log("Railgun damaged " +hit.collider.name +" for " +damage);
            }
        }

        CreateBeam(start, beamEnd);
    }



    private void CreateBeam(Vector3 start, Vector3 end)
    {
        if (beamPrefab == null)
        {
            Debug.LogError("Railgun: Beam Prefab is NULL!");
            return;
        }

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance <= 0.001f)
            return;

        GameObject beam = Instantiate(
            beamPrefab,
            (start + end) / 2f,
            Quaternion.LookRotation(direction)
        );

        beam.transform.localScale = new Vector3(
            1f,
            1f,
            distance
        );

        Destroy(beam, beamDuration);
    }
}
