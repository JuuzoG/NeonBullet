using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
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
            if (dashTimer <= 0f) isDashing = false;
        }
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }
    /*public void Dash()
    {
        if(!isDashing && cooldownTimer <= 0f)
        {
            float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (playerCharCon.WorldMove)
            dashDirection = (Vector3.right * -y + Vector3.forward * x).normalized;
        else
            dashDirection = (transform.forward * y + transform.right * x).normalized;

        if (dashDirection == Vector3.zero)
            dashDirection = playerCharCon.WorldMove ? transform.forward : transform.forward;

        playerCharCon.rb.linearVelocity = new Vector3(dashDirection.x * dashSpeed, playerCharCon.rb.linearVelocity.y, dashDirection.z * dashSpeed);

        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        }
    }
    */
}
