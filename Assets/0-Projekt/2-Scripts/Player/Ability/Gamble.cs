using System.Collections;
using UnityEngine;

public class Gamble : MonoBehaviour
{
    [Header("Symbols")]
    public Sprite heartSprite;
    public Sprite xSprite;

    [Header("Gamble Settings")]
    public float rollTime = 1.5f;
    public float rollSpeed = 0.1f;

    [Header("Symbol Position")]
    public float symbolSpacing = 0.6f;
    public float symbolHeight = 1.5f;

    [Header("Effect Duration")]
    public float effectDuration = 10f;

    private bool gambling = false;

    private GameObject symbol1;
    private GameObject symbol2;
    private GameObject symbol3;

    private SpriteRenderer renderer1;
    private SpriteRenderer renderer2;
    private SpriteRenderer renderer3;

    private Camera mainCamera;
    private Player player;


    void Start()
    {
        Debug.Log("[GAMBLE] Start() called.");

        mainCamera = Camera.main;
        player = GetComponent<Player>();

        if (mainCamera == null)
            Debug.LogError("[GAMBLE] Main Camera not found!");

        if (player == null)
            Debug.LogError("[GAMBLE] Player component not found!");

        if (heartSprite == null)
            Debug.LogError("[GAMBLE] Heart Sprite is missing!");

        if (xSprite == null)
            Debug.LogError("[GAMBLE] X Sprite is missing!");


        // Create symbols

        symbol1 = new GameObject("GambleSymbol1");
        renderer1 = symbol1.AddComponent<SpriteRenderer>();
        renderer1.sortingOrder = 100;

        symbol2 = new GameObject("GambleSymbol2");
        renderer2 = symbol2.AddComponent<SpriteRenderer>();
        renderer2.sortingOrder = 100;

        symbol3 = new GameObject("GambleSymbol3");
        renderer3 = symbol3.AddComponent<SpriteRenderer>();
        renderer3.sortingOrder = 100;


        // Hide symbols

        symbol1.SetActive(false);
        symbol2.SetActive(false);
        symbol3.SetActive(false);

        Debug.Log("[GAMBLE] Three symbols created.");
    }


    void LateUpdate()
    {
        if (mainCamera == null)
            return;

        if (symbol1 == null || symbol2 == null || symbol3 == null)
            return;


        // Position above player

        Vector3 centerPosition =
            transform.position +
            Vector3.up * symbolHeight;

        Vector3 right = mainCamera.transform.right;


        symbol1.transform.position =
            centerPosition - right * symbolSpacing;

        symbol2.transform.position =
            centerPosition;

        symbol3.transform.position =
            centerPosition + right * symbolSpacing;


        // Face camera

        symbol1.transform.rotation = mainCamera.transform.rotation;
        symbol2.transform.rotation = mainCamera.transform.rotation;
        symbol3.transform.rotation = mainCamera.transform.rotation;
    }


    public void ActivateGamble()
    {

        if (gambling)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        StartCoroutine(GambleRoll());
    }


    private IEnumerator GambleRoll()
    {
        gambling = true;

        symbol1.SetActive(true);
        symbol2.SetActive(true);
        symbol3.SetActive(true);

        float timer = 0f;


        while (timer < rollTime)
        {
            renderer1.sprite =
                Random.value > 0.5f
                ? heartSprite
                : xSprite;

            renderer2.sprite =
                Random.value > 0.5f
                ? heartSprite
                : xSprite;

            renderer3.sprite =
                Random.value > 0.5f
                ? heartSprite
                : xSprite;


            yield return new WaitForSeconds(rollSpeed);

            timer += rollSpeed;
        }


        bool firstHeart = Random.value > 0.5f;
        bool secondHeart = Random.value > 0.5f;
        bool thirdHeart = Random.value > 0.5f;


        renderer1.sprite =
            firstHeart ? heartSprite : xSprite;

        renderer2.sprite =
            secondHeart ? heartSprite : xSprite;

        renderer3.sprite =
            thirdHeart ? heartSprite : xSprite;


        int hearts = 0;

        if (firstHeart)
            hearts++;

        if (secondHeart)
            hearts++;

        if (thirdHeart)
            hearts++;


        Debug.Log("[GAMBLE] Hearts: " + hearts);



        if (hearts == 3)
        {

            StartCoroutine(JackpotEffect());
        }
        else if (hearts == 0)
        {

            StartCoroutine(LoserEffect());
        }
        else
        {

            StartCoroutine(MixedEffect(hearts));
        }


        gambling = false;


        // Keep symbols visible

        yield return new WaitForSeconds(2f);

        symbol1.SetActive(false);
        symbol2.SetActive(false);
        symbol3.SetActive(false);
    }


    private IEnumerator JackpotEffect()
    {
        PlayerStats stats = player.stats;

        float originalDamage = stats.damage;
        float originalSpeed = stats.movmentSpeed;
        float originalEnergyRecovery = stats.energyRecoverRate;


        stats.damage *= 2f;
        stats.movmentSpeed *= 1.5f;
        stats.energyRecoverRate *= 1.5f;


        Debug.Log("JACKPOT!! BUFF APPLIED!");
        Debug.Log("[GAMBLE] Damage: " + stats.damage);
        Debug.Log("[GAMBLE] Movement Speed: " + stats.movmentSpeed);


        yield return new WaitForSeconds(effectDuration);


        stats.damage = originalDamage;
        stats.movmentSpeed = originalSpeed;
        stats.energyRecoverRate = originalEnergyRecovery;


    }

    private IEnumerator LoserEffect()
    {
        PlayerStats stats = player.stats;

        float originalDamage = stats.damage;
        float originalSpeed = stats.movmentSpeed;
        float originalEnergyRecovery = stats.energyRecoverRate;


        stats.damage *= 0.5f;
        stats.movmentSpeed *= 0.5f;
        stats.energyRecoverRate *= 0.5f;


        Debug.Log("LOSER!! DEBUFF APPLIED!");


        yield return new WaitForSeconds(effectDuration);


        stats.damage = originalDamage;
        stats.movmentSpeed = originalSpeed;
        stats.energyRecoverRate = originalEnergyRecovery;
    }


    private IEnumerator MixedEffect(int hearts)
    {
        PlayerStats stats = player.stats;

        float originalDamage = stats.damage;
        float originalSpeed = stats.movmentSpeed;


        if (hearts == 2)
        {

            stats.damage *= 1.5f;
            stats.movmentSpeed *= 1.25f;
        }
        else
        { 
            stats.damage *= 0.75f;
            stats.movmentSpeed *= 0.75f;
        }


        yield return new WaitForSeconds(effectDuration);


        stats.damage = originalDamage;
        stats.movmentSpeed = originalSpeed;
    }
}
