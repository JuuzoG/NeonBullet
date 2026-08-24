using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    [Header("Explosion Ability")]
    public float energyCost;
    public float cooldownTime;
    public GameObject attackPrefab;

    [Header("Dash Reference")]
    public DashAbility dashAbility;

    [Header("Railgun")]
    public Railgun railgun;

    [Header("UI Buttons")]
    public Button QButton;
    public Button DashButton;

    [Header("Cooldown UI")]
    [SerializeField] private Image cooldownImage;

    [Header("Flash Colors")]
    [SerializeField] private Color pressedColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private float flashDuration = 0.15f;

    private Player player;
    private float cooldown = 1f;


    void Start()
    {
        player = GetComponent<Player>();

        if (QButton != null)
            QButton.onClick.AddListener(TriggerExplosion);
        if (DashButton != null)
            DashButton.onClick.AddListener(TriggerDash);
    }

    void OnDestroy()
    {
        if (QButton != null)
            QButton.onClick.RemoveListener(TriggerExplosion);
        if (DashButton != null)
            DashButton.onClick.RemoveListener(TriggerDash);
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

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

        if (Input.GetKeyDown(player.Ability))
            TriggerExplosion();

        if (Input.GetKeyDown(player.Dash))
            TriggerDash();

        if (Input.GetKeyDown(KeyCode.R))
            FireRailgun();
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
        StartCoroutine(FlashButton(QButton));
    }

    public void TriggerDash()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (dashAbility == null) return;

        //dashAbility.Dash();
        StartCoroutine(FlashButton(DashButton));
    }

    public void FireRailgun()
    {
        Debug.Log("RAILGUN INPUT RECEIVED");

        if (GameManager.instance.state == GameStates.GameOver)
            return;

        if (GameManager.instance.state == GameStates.paused)
            return;

        if (railgun == null)
        {
            Debug.LogError("SpecialAttack: Railgun component is NULL!");
            return;
        }

        Debug.Log("Calling Railgun.Fire()");

        railgun.Fire();
    }

    private IEnumerator FlashButton(Button button)
    {
        if (button == null) yield break;
        button.image.color = pressedColor;
        yield return new WaitForSecondsRealtime(flashDuration);
        button.image.color = normalColor;
    }
}