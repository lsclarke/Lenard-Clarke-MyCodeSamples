using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions input;

    [Header("Class Reference Variables")]
    [SerializeField] private PlayerScript pScript;
    [SerializeField] private DashScript dashScript;
    [SerializeField] public Rigidbody2D rb;

    [SerializeField] public GameObject dashScriptOBJ;

    [Header("Movement Variables")]
    public float moveX;
    public float moveY;
    public float currentSpeed;
    private float moveSpeed = 4f;
    private float jumpForce = 6f;

    [Header("Collision Variables")]
    [SerializeField] public Transform groundcheck;
    public float groundcheckRadius;
    [SerializeField] public LayerMask groundLayer;
    public bool grounded;

    [SerializeField] public Transform rightWallCheck;
    public float rightWallCheckRadius;
    public bool onRightWall;

    [SerializeField] public Transform leftWallCheck;
    public float leftWallCheckRadius;
    public bool onLeftWall;

    [SerializeField] public LayerMask wallLayer;

    [Header("Sprite Refernce Variables")]
    public bool facing_right;

    private void Awake()
    {
        input = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();

        dashScript = dashScriptOBJ.GetComponent<DashScript>();

        pScript.setSPD(moveSpeed);
        pScript.setJMP(jumpForce);
        moveSpeed = pScript.getSPD();
        jumpForce = pScript.getJMP();

        facing_right = true;

        currentSpeed = moveSpeed;
        

    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y);

        grounded = isGrounded();
        onLeftWall = isTouchingLeftSide();
        onRightWall = isTouchingRightSide();

        if (moveX > 0 && !facing_right)
        {
            Flip();
        }
        if (moveX < 0 && facing_right)
        {
            Flip();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveX = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {

        context.ReadValue<float>();

        if (context.performed && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

    }

    public void Dash(InputAction.CallbackContext context)
    {

        if (context.performed && dashScript.canDash)
        {
            StartCoroutine(dashScript.Dash());
        }
    }

    private void Flip()
    {
        //Flip the sprite of the player when changing movement directions

        facing_right = !facing_right;
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

    }

    private bool isGrounded()
    {
        //Check for ground collision
        return Physics2D.OverlapCircle(groundcheck.position, groundcheckRadius, groundLayer);
    }
    private bool isTouchingRightSide()
    {
        //Check for ground collision
        return Physics2D.OverlapCircle(rightWallCheck.position, rightWallCheckRadius, wallLayer);
    }

    private bool isTouchingLeftSide()
    {
        //Check for ground collision
        return Physics2D.OverlapCircle(leftWallCheck.position, leftWallCheckRadius, wallLayer);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundcheck.position, groundcheckRadius);
        Gizmos.DrawWireSphere(rightWallCheck.position, rightWallCheckRadius);
        Gizmos.DrawWireSphere(leftWallCheck.position, leftWallCheckRadius);
    }
}
