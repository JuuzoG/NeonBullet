using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public KeyCode inputKey;
    public float energyCost;
    public float cooldownTime;
    public GameObject attackPrefab;

    private Player player;
    private float cooldown = 0;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        cooldown -= Time.deltaTime;

        if (cooldown > 0) return;

        if (energyCost > player.GainEnergy(0)) return;

        if (Input.GetKeyDown(inputKey))
        {
            GameObject obj =
                Instantiate(attackPrefab, transform.position, Quaternion.identity);

            Explosion explosion = obj.GetComponent<Explosion>();
            if (explosion != null)
                explosion.SetOwner(gameObject);

            cooldown = cooldownTime;

            player.GainEnergy(-energyCost);
        }
    }
}