using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector3 defaultPos;
    private const float radius = 0.1f;

    [Header("Mouse Pan")]
    [SerializeField] private float boundBoxWidth = 400f;
    [SerializeField] private float boundBoxHeight = 250f;
    [SerializeField] private float mousePanSpeed = 3f;
    [SerializeField] private float maxMouseOffset = 3f;

    void Start()
    {
        defaultPos = transform.position;
    }

    void Update()
    {
        Vector3 playerPosition = GameManager.instance.player.transform.position;

        Vector3 direction = playerPosition - transform.position + defaultPos;
        if (direction.magnitude > radius)
        {
            transform.position += direction * Time.deltaTime * 5;
        }

        ApplyMousePan(playerPosition);
    }

    private void ApplyMousePan(Vector3 playerPosition)
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        float halfWidth = boundBoxWidth / 2f;
        float halfHeight = boundBoxHeight / 2f;

        float offsetX = mousePos.x - screenCenter.x;
        float offsetY = mousePos.y - screenCenter.y;

        float panX = 0f;
        float panZ = 0f;

        if (Mathf.Abs(offsetY) > halfHeight)
        {
            float over = (Mathf.Abs(offsetY) - halfHeight) / (screenCenter.y - halfHeight);
            panX = -Mathf.Sign(offsetY) * Mathf.Clamp01(over);
        }

        if (Mathf.Abs(offsetX) > halfWidth)
        {
            float over = (Mathf.Abs(offsetX) - halfWidth) / (screenCenter.x - halfWidth);
            panZ = Mathf.Sign(offsetX) * Mathf.Clamp01(over);
        }

        Vector3 desiredPos = transform.position + new Vector3(panX, 0f, panZ) * mousePanSpeed * Time.deltaTime;

        Vector3 fromPlayer = desiredPos - playerPosition;
        fromPlayer = Vector3.ClampMagnitude(fromPlayer, maxMouseOffset);

        transform.position = playerPosition + fromPlayer;
    }
}