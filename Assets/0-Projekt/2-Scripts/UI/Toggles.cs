using UnityEngine;

public class Toggles : MonoBehaviour
{
    public bool WorldMovement = false;
    private PlayerCharacterController player;
    void Start()
    {
        player = GetComponent<PlayerCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        player.WorldMove = WorldMovement;
    }
}
