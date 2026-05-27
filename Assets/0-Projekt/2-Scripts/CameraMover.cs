using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector3 defaultPos;
    private const float radius = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = GameManager.instance.player.transform.position;
        Vector3 direction = playerPosition - transform.position + defaultPos; 
        if (direction.magnitude > radius)
        {
            transform.position += direction*Time.deltaTime*5;
        }
    }
}
