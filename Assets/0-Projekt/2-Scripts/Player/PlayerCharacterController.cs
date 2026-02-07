using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private float movementSpeed = 3;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direction = (transform.forward * y + transform.right * x).normalized;
        rb.linearVelocity = direction * movementSpeed;
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }
}
