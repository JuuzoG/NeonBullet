using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth;
    public float maxEnergy;

    [Header("Current Stats")]
    public float movmentSpeed;
    public float energyRecoverRate;
    public float health;
    public float energy;

    private void OnEnable()
    {
        health = maxHealth;
        energy = maxEnergy;
    }
}