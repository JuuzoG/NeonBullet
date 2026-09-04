using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;

    public bool IsDashing => isDashing;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;
    private PlayerCharacterController playerCharCon;

    void Start()
    {
        playerCharCon = GetComponent<PlayerCharacterController>();
    }

    void Update()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                Vector3 v = playerCharCon.rb.linearVelocity;
                playerCharCon.rb.linearVelocity = new Vector3(0f, v.y, 0f);
            }
        }
    }

    public bool Dash()
    {
        if (isDashing)
            return false;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Transform cam = playerCharCon.Cam;
        Vector3 camForward = cam.forward; camForward.y = 0f;
        Vector3 camRight = cam.right; camRight.y = 0f;

        Vector3 rawDirection = camForward.normalized * y + camRight.normalized * x;

        dashDirection = rawDirection.sqrMagnitude > 0.0001f
            ? rawDirection.normalized
            : transform.forward;

        Vector3 currentVel = playerCharCon.rb.linearVelocity;
        playerCharCon.rb.linearVelocity = new Vector3(
            dashDirection.x * dashSpeed,
            currentVel.y,
            dashDirection.z * dashSpeed
        );

        isDashing = true;
        dashTimer = dashDuration;
        return true;
    }
}