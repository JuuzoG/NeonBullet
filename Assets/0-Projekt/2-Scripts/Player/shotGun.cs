using UnityEngine;

public class shotGun : MonoBehaviour
{
    public GameObject prefab;
    public void GunShot()
    {
        SpawnBullet(new Vector3(0.011f,  1.247f, 0.796f));
        SpawnBullet(new Vector3(-0.094f, 1.252f, 0.796f));
        SpawnBullet(new Vector3(0.134f,  1.255f, 0.796f));
        SpawnBullet(new Vector3(0.014f,  1.125f, 0.796f));
        SpawnBullet(new Vector3(0.007f,  1.374f, 0.796f));
    }

    void SpawnBullet(Vector3 localPos)
    {
        GameObject bullet = Instantiate(prefab, transform);
        bullet.transform.localPosition = localPos;
        bullet.transform.localRotation = Quaternion.identity;
        bullet.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);

        bullet.transform.SetParent(null);

        bullet.GetComponent<ShotGunProjectile>()?.SetOwner(gameObject);
    }
}
