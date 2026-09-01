using UnityEngine;
using UnityEngine.Playables;

public class EndingsceneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneTimeline;
    [SerializeField] private GameObject Player;
    private PlayableDirector playable;

    void Start()
    {
        playable = cutsceneTimeline.GetComponent<PlayableDirector>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutsceneTimeline.SetActive(true);
            playable.Play();

            Player.SetActive(false);
        }
    }
}
