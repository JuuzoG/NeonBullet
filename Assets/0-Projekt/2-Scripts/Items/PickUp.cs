using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int minAmount;
    public int maxAmount;
    private int amount;
    public ResourceTypes type;

    void Start()
    {
        amount = Random.Range(minAmount,maxAmount+1);
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            switch (type)
            {
                case ResourceTypes.munition:
                    player.GainMunition(amount);
                    break;
                case ResourceTypes.energy:
                    player.GainEnergy(amount);
                    break;
                case ResourceTypes.health:
                    player.GainHealth(amount);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

public enum ResourceTypes { munition, energy, health}
