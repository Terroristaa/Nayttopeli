using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    public Vector3 playerMoveDirection;
    

    void Update()
    {
        float inputx = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        playerMoveDirection = new Vector2(inputx, inputY).normalized;

        rb.linearVelocity = new Vector3(playerMoveDirection.x * moveSpeed, playerMoveDirection.y * moveSpeed * Time.deltaTime);
        animator.SetFloat("moveX", inputx); 
        animator.SetFloat("moveY", inputY);

        if (playerMoveDirection == Vector3.zero)
        {
            animator.SetBool("moving", false);
        } else {
            animator.SetBool("moving", true);
        }
    }
    
    void FixedUpdate ()
    {
        rb.linearVelocity = new Vector2(playerMoveDirection.x * moveSpeed, playerMoveDirection.y * moveSpeed);
    }
}
