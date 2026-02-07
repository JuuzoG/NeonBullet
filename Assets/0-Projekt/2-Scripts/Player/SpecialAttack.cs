using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public KeyCode inputKey;
    public float energyCost;
    public float cooldownTime;
    public GameObject attackPrefab;


    private Player player;
    private float cooldown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        
        cooldown -= Time.deltaTime;
        if (energyCost > player.GainEnergy(0)) return;
        if (cooldown > 0) return;
        if (Input.GetKeyDown(inputKey))
        {
            Instantiate(attackPrefab, transform.position, Quaternion.identity);
            cooldown = cooldownTime;
            player.GainEnergy(energyCost*-1);
        }
    }
}
