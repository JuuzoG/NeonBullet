using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    [Header("Explosion Ability")]
    public float energyCost;
    public float cooldownTime;
    public GameObject attackPrefab;

    [Header("UI Buttons")]
    public Button QButton;
    public Button EButton;

    [Header("Cooldown UI")]
    [SerializeField] private Image cooldownImage;

    [Header("Unlocked")]
    public bool Dash = false;
    public bool Explosion = false;
    public bool Gambeling = true;

    private Player player;
    private DashAbility dash;
    private float cooldown = 1f;


    void Start()
    {
        dash = GetComponent<DashAbility>();
        player = GetComponent<Player>();
    }

    void Awake()
    {
        GameManager.instance.specialAttack = this;
    }



    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (GameManager.instance.state == GameStates.hacking) return;

        cooldown -= Time.deltaTime;

        if (cooldown > 1f)
        {
            cooldownImage.fillAmount = cooldown / cooldownTime;
        }
        else
        {
            cooldown = 0f;
            cooldownImage.fillAmount = 0f;
        }

        if (Input.GetKeyDown(player.Q))
        {
            if(Explosion) TriggerExplosion();
            if(Dash) dash.Dash();
            if(Gambeling) Debug.Log("ItsGambelingTime");
        }
            
    }


    public void TriggerExplosion()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (cooldown > 0) return;
        if (energyCost > player.GainEnergy(0)) return;

        GameObject obj = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        Explosion explosion = obj.GetComponent<Explosion>();
        if (explosion != null)
            explosion.SetOwner(gameObject);

        cooldown = cooldownTime;
        player.GainEnergy(-energyCost);
    }

    public void ItsGambelingTime()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        //if (cooldown > 0) return;
        //if (energyCost > player.GainEnergy(0)) return;


    }
}