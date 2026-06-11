using UnityEngine;

public class shotGun : MonoBehaviour
{
    public GameObject prefab;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) GunShot();
    }
    public void GunShot()
    {
        GameObject Bullet = Instantiate(prefab, transform);
        Bullet.transform.localPosition = new Vector3(0.011f, 1.247f, 0.796f);
        Bullet.transform.localRotation = Quaternion.identity;
        Bullet.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);

        GameObject Bullet2 = Instantiate(prefab, transform);
        Bullet2.transform.localPosition = new Vector3(-0.094f, 1.252f, 0.796f);
        Bullet2.transform.localRotation = Quaternion.identity;
        Bullet2.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);

        GameObject Bullet3 = Instantiate(prefab, transform);
        Bullet3.transform.localPosition = new Vector3(0.134f, 1.255f, 0.796f);
        Bullet3.transform.localRotation = Quaternion.identity;
        Bullet3.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);

        GameObject Bullet4 = Instantiate(prefab, transform);
        Bullet4.transform.localPosition = new Vector3(0.014f, 1.125f, 0.796f);
        Bullet4.transform.localRotation = Quaternion.identity;
        Bullet4.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);

        GameObject Bullet5 = Instantiate(prefab, transform);
        Bullet5.transform.localPosition = new Vector3(0.007f, 1.374f, 0.796f);
        Bullet5.transform.localRotation = Quaternion.identity;
        Bullet5.transform.localScale = new Vector3(0.04474f, 0.10285f, 0.42595f);
    }
}
