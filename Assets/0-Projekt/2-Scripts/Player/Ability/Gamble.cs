using System.Collections;
using UnityEngine;

public class Gamble : MonoBehaviour
{
    public Sprite heartSprite;
    public Sprite xSprite;

    public float rollTime = 1.5f;
    public float rollSpeed = 0.1f;

    private bool gambling = false;

    public void ActivateGamble()
    {
        if (gambling)
            return;

        StartCoroutine(GambleRoll());
    }

    private IEnumerator GambleRoll()
    {
        gambling = true;

        float timer = 0f;

        while (timer < rollTime)
        {
            // Random result for first symbol
            Sprite first = Random.value > 0.5f ? heartSprite : xSprite;

            // Random result for second symbol
            Sprite second = Random.value > 0.5f ? heartSprite : xSprite;

            Debug.Log("First: " + first.name);
            Debug.Log("Second: " + second.name);

            yield return new WaitForSeconds(rollSpeed);

            timer += rollSpeed;
        }

        // Final result
        bool firstHeart = Random.value > 0.5f;
        bool secondHeart = Random.value > 0.5f;

        Sprite finalFirst = firstHeart ? heartSprite : xSprite;
        Sprite finalSecond = secondHeart ? heartSprite : xSprite;

        Debug.Log("FINAL: " + finalFirst.name + " | " + finalSecond.name);

        // Determine outcome
        if (firstHeart && secondHeart)
        {
            Debug.Log("JACKPOT!");
        }
        else if (!firstHeart && !secondHeart)
        {
            Debug.Log("LOSER!");
        }
        else
        {
            Debug.Log("MIXED!");
        }

        gambling = false;
    }
}
