using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float maxHealth;
    public float movmentSpeed;
    public float maxEnergy;
    public float energyRecoverRate;

}