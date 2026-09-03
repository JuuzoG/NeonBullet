using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public GameStates state = GameStates.inGame;
    public Inventory inventory;
    public WeaponSelector WeaponSelect;
    public Player player;
    public SpecialAttack specialAttack;
    public static GameManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

public enum GameStates
{
    inGame, paused, GameOver, title, inventory, hacking
}