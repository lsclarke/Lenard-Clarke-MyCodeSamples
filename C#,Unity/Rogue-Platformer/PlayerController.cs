using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Title for speed and health variables 
    [Header("Player Variables")]
    [Space(5)]

    [Space(5)]
    [Header("GameObject/Script Reference Variables")]
    [Space(5)]

    [SerializeField] public PlayerCombat combat1;
    [SerializeField] public GameObject combat1OBJ;
    [SerializeField] public PlayerAttackScript attackScript;


    [Space(5)]
    [Header("Movement Variables")]
    [Space(5)]
    //Movement Var
    [SerializeField] public Rigidbody2D rb;
    public float movementSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float inputHor;
    public float inputVer;

    [Space(5)]
    [Header("Render Variables")]
    [Space(5)]

    //Render Var
    private SpriteRenderer spriteRenderer;
    [SerializeField] public TrailRenderer trailRender;
    public bool facing_right;

    [Space(5)]
    [Header("Jump and Ground Variables")]
    [Space(5)]

    //Jumping Var

    private bool isJumping;
    public bool isInAir;
    public int extrajumpCounter = 1;
    public float jumpTimeCounter;
    public float jumpTime;
    public bool isGrounded;
    [SerializeField] public Transform groundcheckLoc;
    public float groundcheckRadius;
    [SerializeField] public LayerMask groundLayer;

    [Space(5)]
    [Header("Dash Variables")]
    [Space(5)]

    //Dashing Var
    public bool canDash = true;
    public bool isDashing;
    public float dashSpeed;
    public float dashTime;
    public float dashCoolDown;
    public float dashupCoolDown;
    private int maxDashCount = 3;
    public int dashCounter;
    public int upDashCounter;

    [Space(5)]
    [Header("UI Stats Variables")]
    [Space(5)]

    //Stats UI Var
    [SerializeField] public HealthUIScript healthScript;
    [SerializeField] public ManaUIScript manaScript;
    public float maxHealth = 100f;
    public float maxMana = 100f;
    public float currentHealth;
    public float currentMana;

    public bool canMove;

    [Space(5)]
    [Header("Respawn Variables")]
    [Space(5)]

    //Respawn Var
    [SerializeField] public GameObject respawnPoint;

    [Space(5)]
    [Header("Audio")]
    [Space(5)]

    public AudioSource audio;
    public AudioClip[] groundSounds;

    // Start is called before the first frame update
    void Start()
    {
        combat1 = combat1OBJ.GetComponent<PlayerCombat>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        canDash = true;
        facing_right = true;



        currentHealth = maxHealth;
        currentMana = maxMana;

        audio = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {
        rb.gravityScale = 1f;

        isGrounded = Physics2D.OverlapCircle(groundcheckLoc.position, groundcheckRadius, groundLayer);

        if (isGrounded == false) { isInAir = true; Debug.Log("Is in Air"); }

        rb.freezeRotation = true;

        if (isDashing)
        {
            return;
        }

        //Fliping sprite

        if (inputHor > 0 && !facing_right)
        {
            Flip();
        }
        if (inputHor < 0 && facing_right)
        {
            Flip();
        }

        if (inputHor > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (inputHor < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Counter Checks

        if (isGrounded)
        {
            dashCounter = 2;
            upDashCounter = 1;
            extrajumpCounter = 1;
            //  UnityEngine.Debug.Log("dashCounter " + dashCounter);
        }


    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale *= -1;
        facing_right = !facing_right;
    }

    void MovmentHandler()
    {

        //=====================================Horizontal Movement==========================================================//
        inputHor = Input.GetAxis("Horizontal");
        inputVer = Input.GetAxis("Vertical");

        if (Input.GetKey("a") && (!isDashing && !combat1.attacking && !combat1.blocking && !combat1.isCharging))
        {
            canMove = true;
            if (canMove)
            {
                rb.velocity = new Vector2(inputHor * movementSpeed, rb.velocity.y);

            }
        }


        if (Input.GetKey("d") && (!isDashing && !combat1.attacking && !combat1.blocking && !combat1.isCharging))
        {
            canMove = true;
            if (canMove) 
            {
                rb.velocity = new Vector2(inputHor * movementSpeed, rb.velocity.y);

            }
        }

        if (combat1.isCharging)
        {
            canMove = false;
        }

        //====================================Jumping Mechanic===========================================================//

        if (isGrounded == true && Input.GetButton("Jump"))
        {
            audio.clip = groundSounds[0];
            audio.Play();
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        }

        /*When player is in air regardless of if he jumped off the ground or is falling from the edge. 
          The player can press the jump button mid air, but only once!*/
        if (Input.GetButtonDown("Jump") && isInAir)
        {
            if (extrajumpCounter > 0)
            {
                audio.clip = groundSounds[0];
                audio.Play();
                Debug.Log("Double Jump!");
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpSpeed);
                extrajumpCounter -= 1;
            }

        }


    }



    void DashHandler()
    {
        if (currentMana > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown("w") && dashCounter > 0)
            {

                if (!combat1.attacking || !combat1.blocking)
                {


                    Debug.Log("Dashing");
                    if (Input.GetKey("d") && canDash && !combat1.blocking && !combat1.attacking)
                    {
                        audio.clip = groundSounds[0];
                        audio.Play();
                        StartCoroutine(DashingMechanicRight());
                        UseMana(10f);
                    }
                    else if (Input.GetKey("a") && canDash && !combat1.blocking && !combat1.attacking)
                    {
                        audio.clip = groundSounds[0];
                        audio.Play();
                        StartCoroutine(DashingMechanicLeft());
                        UseMana(10f);
                    }

                    if (Input.GetKeyUp("d") && isDashing && !combat1.blocking && !combat1.attacking)
                    {
                        movementSpeed = 0;
                    }
                    else if (Input.GetKeyUp("a") && isDashing && !combat1.blocking && !combat1.attacking)
                    {
                        movementSpeed = 0;
                    }
                }

                if ((Input.GetKey("w") && Input.GetKeyDown(KeyCode.LeftShift) && upDashCounter > 0) && !combat1.blocking && !combat1.attacking)
                {
                    if (!isGrounded)
                    {
                        audio.clip = groundSounds[0];
                        audio.Play();
                        UnityEngine.Debug.Log("Is Dashing Up: " + dashCounter);
                        StartCoroutine(DashingMechanicUp());
                        UseMana(10f);
                    }
                }
            }
        }
    }

    public void DashMovementRight()
    {
        StartCoroutine(DashingMechanicRight());

    }

    public void DashMovementLeft()
    {
        StartCoroutine(DashingMechanicLeft());

    }


    public IEnumerator DashingMechanicRight()
    {

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        /* Debug code to test mana use*/
        //UseMana(10f);
        trailRender.emitting = true;
        dashCounter -= 1;
        yield return new WaitForSeconds(dashTime);
        trailRender.emitting = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;


    }
    public IEnumerator DashingMechanicLeft()
    {

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(-transform.localScale.x * dashSpeed, 0f);
        /* Debug code to test mana use*/
        //UseMana(10f);
        trailRender.emitting = true;
        dashCounter -= 1;
        yield return new WaitForSeconds(dashTime);
        trailRender.emitting = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;


    }

    public IEnumerator DashingMechanicUp()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, transform.localScale.y * dashSpeed);
        /* Debug code to test mana use*/
        //UseMana(10f);
        trailRender.emitting = true;
        upDashCounter -= 1;
        yield return new WaitForSeconds(dashTime);
        trailRender.emitting = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0);
        rb.gravityScale = 1;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown - 0.5f);
        canDash = true;


    }

    private IEnumerator InvincibilityFrames(float waitTime)
    {
        TakeDamage(25f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.enabled = true;


    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;
        healthScript.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
            manaScript.setMana(currentMana);
            healthScript.setHealth(currentHealth);
            transform.position = respawnPoint.transform.position;
            Debug.Log("Respawn");
        }
    }

    public void UseMana(float subtractAmount)
    {
        currentMana = currentMana - subtractAmount;
        manaScript.setMana(currentMana);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            TakeDamage(11f);
            StartCoroutine(InvincibilityFrames(.2f));


            if (facing_right)
            {
                rb.velocity = new Vector2(rb.velocity.x * -0.5f, jumpSpeed / 2);
            }

            if (!facing_right)
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.5f, jumpSpeed / 2);
                InvincibilityFrames(1f);
            }

        }

        if(collision.gameObject.tag == "Door")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


        if (collision.gameObject.tag == "Dead")
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundcheckLoc.position, groundcheckRadius);
    }

    // Update is called once per frame
    void Update()
    {
        MovmentHandler();
        DashHandler();

    }
}
