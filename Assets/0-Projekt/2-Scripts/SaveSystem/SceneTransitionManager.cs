using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public CanvasGroup fadeCanvasGroup;

    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;

    private bool isTransitioning;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (fadeCanvasGroup == null)
            fadeCanvasGroup = GetComponent<CanvasGroup>();

        SetAlpha(0f);
    }

    public void FadeAndLoad(string targetScene, Action onSceneLoaded = null)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("SceneTransitionManager: already transitioning, ignoring request.");
            return;
        }

        StartCoroutine(FadeAndLoadRoutine(targetScene, onSceneLoaded));
    }

    private IEnumerator FadeAndLoadRoutine(string targetScene, Action onSceneLoaded)
    {
        isTransitioning = true;
        fadeCanvasGroup.blocksRaycasts = true;

        yield return Fade(0f, 1f, fadeOutDuration);

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        while (!op.isDone)
            yield return null;

        onSceneLoaded?.Invoke();

        yield return Fade(1f, 0f, fadeInDuration);

        fadeCanvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            SetAlpha(to);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetAlpha(float alpha)
    {
        fadeCanvasGroup.alpha = alpha;
    }
}