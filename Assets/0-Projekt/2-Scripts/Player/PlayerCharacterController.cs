using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public Rigidbody rb;
    private Animator animator;
    private PlayerStats stats;
    public bool WorldMove = true;
    

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        stats = GameManager.instance.player.stats;
    }

    // Update is called once per frame
    void Update()
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
        if (!WorldMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection =
                (transform.forward * y + transform.right * x).normalized;

                rb.linearVelocity = new Vector3(
                moveDirection.x * stats.movmentSpeed,
                rb.linearVelocity.y,
                moveDirection.z * stats.movmentSpeed
                );
                animator.SetFloat("moveX", -y);
                animator.SetFloat("moveY", x);
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 direction = (Vector3.right * -y + Vector3.forward * x).normalized + new Vector3(0,rb.linearVelocity.y,0);
            rb.linearVelocity = direction * stats.movmentSpeed;
            animator.SetFloat("moveX", -y);
        animator.SetFloat("moveY", x);
        }        
    }

    public void WorldMoveToggle(bool yes)
    {
        WorldMove = yes;
    }

    
}
