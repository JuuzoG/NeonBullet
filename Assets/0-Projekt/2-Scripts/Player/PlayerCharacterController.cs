using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        stats = GameManager.instance.player.stats;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        
        // Look towards the Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        {
            Vector3 target = new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z);
            transform.LookAt(target);
        }

        // Move the Character
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(x, 0f, y).normalized;

        rb.linearVelocity = new Vector3(
            moveDirection.x * stats.movmentSpeed,
            rb.linearVelocity.y,
            moveDirection.z * stats.movmentSpeed
        );

        animator.SetFloat("moveX", -y);
        animator.SetFloat("moveY", x);

    }
}
