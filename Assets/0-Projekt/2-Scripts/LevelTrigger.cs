using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private string LoadScene;

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player")) SceneManager.LoadScene(LoadScene);
    }
}
