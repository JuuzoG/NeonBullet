using UnityEngine;

public class DashAbility : MonoBehaviour
{
    private Player player;

    void Start()
    {
        GameObject playerGEt = GameObject.FindGameObjectWithTag("Player");
        player = playerGEt.GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(player.Ability))
        {
            Dash();
        }
    }

    private void Dash()
    {
        
    }
}
