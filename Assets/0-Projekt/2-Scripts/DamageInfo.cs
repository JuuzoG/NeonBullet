using UnityEngine;

public struct DamageInfo
{
    public float damage;
    public GameObject source;
    public Vector3 hitPoint;
    public Vector3 direction;

    public DamageInfo(float damage, GameObject source, Vector3 hitPoint, Vector3 direction)
    {
        this.damage = damage;
        this.source = source;
        this.hitPoint = hitPoint;
        this.direction = direction;
    }
}