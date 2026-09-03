using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public Rigidbody rb;
    private Animator animator;
    private PlayerStats stats;
    [SerializeField] private Transform cam;
    

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
        if (GameManager.instance.state == GameStates.hacking) return;
        
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

            Vector3 camfoward =cam.forward;
            //Debug.Log(camfoward);
            Vector3 camright =cam.right;
            camfoward.y =0;
            camright.y =0;

            Vector3 forwardRealitive = y *camfoward;
            Vector3 rightRealitive = x *camright;

            Vector3 moveDir = forwardRealitive + rightRealitive;

            rb.linearVelocity = new Vector3(moveDir.x,rb.linearVelocity.y, moveDir.z).normalized * stats.movmentSpeed;
            
            animator.SetFloat("moveX", -y);
            animator.SetFloat("moveY", x);
              
    }
}
