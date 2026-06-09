using UnityEngine;
using UnityEngine.InputSystem;

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
        if (Gamepad.current.leftTrigger.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Dash();
        }
    }

    private void Dash()
    {
        
    }
}
