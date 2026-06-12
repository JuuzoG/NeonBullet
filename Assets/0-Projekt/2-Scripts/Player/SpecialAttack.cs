using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    public float energyCost;
    public float cooldownTime;
    public GameObject attackPrefab;
    private Player player;
    private float cooldown = 0;
    public Button MouseButton;
    [Header("Color")]
    [SerializeField] private Color pressedColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;

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

        if (Input.GetKeyDown(player.Ability))
        {
            GameObject obj =
                Instantiate(attackPrefab, transform.position, Quaternion.identity);

            Explosion explosion = obj.GetComponent<Explosion>();
            if (explosion != null)
                explosion.SetOwner(gameObject);

            cooldown = cooldownTime;

            player.GainEnergy(-energyCost);
            FlashButtonColor();
        }
    }
    private IEnumerator FlashButtonColor()
    {
        if (MouseButton == null)
            yield break;

        MouseButton.image.color = pressedColor;

        yield return new WaitForSecondsRealtime(0.15f);

        MouseButton.image.color = normalColor;
    }
}