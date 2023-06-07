using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [Header("GameObject/ Script Reference Variables")]
    [Space(5)]

    [SerializeField] public PlayerController playerControl;
    [SerializeField] public GameObject playerObj;

    


    [SerializeField] public Animator anim;
    [SerializeField] public AnimatorStateInfo animStateInfo;
    [SerializeField] public  PlayerCombat playerCombat;
    [SerializeField] public GameObject combat1OBJ;


    //States Integers           [  0  :    1   :    2   :   3    :    4   :   5   ]
    private enum MovementStates { Idle, Running, Jumping, Dashing, Falling, AirDash};



    // Start is called before the first frame update
    void Start()
    {
        playerControl = playerObj.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        playerCombat = combat1OBJ.GetComponent<PlayerCombat>();
        
    }


    public void GroundStateUpdate()
    {
        MovementStates states;

      //  anim.SetFloat("Speed", Mathf.Abs(playerControl.movementSpeed));
        if(playerControl.inputHor > 0f && playerControl.isGrounded && !playerCombat.attacking && !playerCombat.blocking)
        {
            states = MovementStates.Running;
            Debug.Log("State: " + states);

        }
        else if (playerControl.inputHor < 0f && playerControl.isGrounded && !playerCombat.attacking && !playerCombat.blocking)
        {
            states = MovementStates.Running;
            Debug.Log("State: " + states);

        }
        else
        {
            states = MovementStates.Idle;
            Debug.Log("State: " + states);
        }

        //Jumping

        if (playerControl.rb.velocity.y > .01f && !playerControl.isGrounded)
        {
            states = MovementStates.Jumping;
            Debug.Log("State: " + states);
        }
        else if (playerControl.rb.velocity.y < -.01f && !playerControl.isGrounded)
        {
            states = MovementStates.Falling;
            Debug.Log("State: " + states);
        }


        //Dashing

        if (playerControl.isDashing && playerControl.isGrounded && !playerCombat.attacking && !playerCombat.blocking)
        {
            states = MovementStates.Dashing;
        }

        if (playerControl.isDashing && !playerControl.isGrounded && !playerCombat.attacking && !playerCombat.blocking)
        {
            states = MovementStates.AirDash;
        }

        //Initialize states
        anim.SetInteger("GroundStates", (int)states);
    }

    // Update is called once per frame
    void Update()
    {
        GroundStateUpdate();
    }
}
