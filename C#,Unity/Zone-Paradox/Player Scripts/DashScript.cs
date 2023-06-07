using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DashScript : MonoBehaviour
{
    [Header("Dash Variables")]
    public bool canDash = true;
    public bool isDashing;
    private float dashSpeed = 20f;
    private float dashTime = 0.2f;
    private float dashCoolDown = 1f;
    private float dashupCoolDown;

    private PlayerInputActions input;
    private PlayerMovement playerMovement;
    private CharacterLevelSwap characterSwap;

    public GameObject characterSwapOBJ;
    public GameObject playerMovementOBJ;

    private void Awake()
    {
        input = new PlayerInputActions();
        playerMovement = playerMovementOBJ.GetComponent<PlayerMovement>();
        characterSwap = characterSwapOBJ.GetComponent<CharacterLevelSwap>();
    }

    private void FixedUpdate()
    {
        if (isDashing && playerMovement.facing_right)
        {
            playerMovement.rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        }
        if (isDashing && !playerMovement.facing_right)
        {
            playerMovement.rb.velocity = new Vector2(-transform.localScale.x * -dashSpeed, 0f);

        }

        if (isDashing)
        {
            return;
        }
    }

    public IEnumerator Dash()
    {
        Debug.Log("DASHING");
        canDash = false;
        isDashing = true;
        float originalGravity = playerMovement.rb.gravityScale;
        playerMovement.rb.gravityScale = 0f;
        playerMovement.enabled = false;
        characterSwap.enabled = false;
        yield return new WaitForSeconds(dashTime);


        isDashing = false;
        playerMovement.enabled = true;
        characterSwap.enabled = true;
        playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, playerMovement.rb.velocity.y);
        playerMovement.rb.gravityScale = originalGravity;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;


    }



}