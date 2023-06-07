using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PlayerCombat : MonoBehaviour
{

    [Header("GameObject/Script Reference Variables")]
    [Space(5)]

    [SerializeField] public PlayerController playerControl;
    [SerializeField] public AnimationController animControl;
    [SerializeField] public GameObject playerOBJ;
    private PlayerAttackScript playerAttackScript;
    [SerializeField] public GameObject playerAttackScriptObj;

    [SerializeField] public GameObject ShieldOBJ;

    [Space(5)]
    [Header("Combat Variables")]
    [Space(5)]

    [SerializeField] public bool attacking;
    [SerializeField] public bool blocking;
    [SerializeField] public bool isCharging;
    [SerializeField] public bool releaseCharge;
    public int meleeCombo;
    public int airCombo;

    private float attackValue;

    [Space(5)]
    [Header("Charge Attack Variables")]
    [Space(5)]
    public float timer;
    public float holdTime = 1f;

    [Space(5)]
    [Header("Audio")]
    [Space(5)]

    public AudioSource audio;
    public AudioClip[] attackSounds;




    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        attackValue = 7f;
        playerAttackScript = playerAttackScriptObj.GetComponent<PlayerAttackScript>();

        playerAttackScript.setAtk(attackValue);

        animControl = GetComponent<AnimationController>();
        playerControl = playerOBJ.GetComponent<PlayerController>();
        attacking = false;
        isCharging= false;

    }

    private void FixedUpdate()
    {
        if (releaseCharge)
        {
            attackValue += 3;
        }
    }

    public void StartCombo()
    {
        attacking = false;

        if (playerControl.isGrounded && meleeCombo < 3)
        {
            StopMove();
            meleeCombo++;            
        } 
        else if(playerControl.isInAir && airCombo < 1)
        {       
            airCombo++;          
        }

    }

    public void mainAttackCombo()
    {
        //Ground Slash combo
        if (Input.GetMouseButtonDown(0) && !attacking && playerControl.isGrounded && !blocking)
        {
            attacking = true;
            audio.clip = attackSounds[meleeCombo];
            audio.Play();
            playerControl.canDash = false;
            animControl.anim.SetTrigger("melee"+meleeCombo);
        }
        else
        {
            playerControl.inputHor = Input.GetAxis("Horizontal");
            playerControl.movementSpeed = 4f;
            playerControl.canDash = true;
            Debug.Log("Attack not working" + meleeCombo);
        }

        //Air Slash Combo

        if (Input.GetMouseButtonDown(0) && !attacking && playerControl.isInAir && !blocking)
        {
            audio.clip = attackSounds[airCombo];
            audio.Play();
            attacking = true;
            playerControl.canDash = false;
            animControl.anim.SetTrigger("airCombo" + airCombo);
        }
        else
        {
            playerControl.canDash = true;
            Debug.Log("Attack not working" + airCombo);
        }
    }

    public void leftclickChargeAttack()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    timer += Time.deltaTime;
        //}

        if(Input.GetMouseButton(0) /*&& timer >= holdTime*/)
        {
            playerControl.canDash = false;
            attackValue += 3;
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                attackValue += 3;
                isCharging = true;
                animControl.anim.SetBool("isCharging", isCharging);
                Debug.Log("Charging");
            }
            //isCharging = true;
            //animControl.anim.SetBool("isCharging", isCharging);
            //Debug.Log("Charging");

        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            
            timer = 0f;
            releaseCharge = true;
            if (releaseCharge)
            {
                audio.clip = attackSounds[2];
                audio.Play();
                playerAttackScript.setAtk(10f);
                animControl.anim.SetBool("isReleased", releaseCharge);            
                Debug.Log("Charge Attack!");
                timer = 0f;
            }

        }
        else if(Input.GetMouseButtonUp(0) && !isCharging)
        {
            
            timer = 0f;
            playerAttackScript.setAtk(7f);
            
            releaseCharge = false;
            if (!releaseCharge)
            {
                playerAttackScript.setAtk(7f);
                playerControl.canDash = true;
            }
            isCharging = false;
        }
        
    }

    public void FinishAnim()
    {
        isCharging = false;
        attacking = false;
        releaseCharge= false;
        meleeCombo = 0;
        airCombo = 0;
        timer = 0f;
        animControl.anim.SetBool("isCharging", isCharging);
        animControl.anim.SetBool("isReleased", releaseCharge);

        playerControl.inputHor = Input.GetAxis("Horizontal");
        playerControl.movementSpeed = 4f;
    }

    public void StopMove()
    {
        playerControl.inputHor = 0f;
        playerControl.movementSpeed = 0f;

    }


    public void BlockingMechanic()
    {
        if (playerControl.isGrounded)
        {
            if (Input.GetMouseButtonDown(1) && !blocking)
            {
                blocking = true;
                if (blocking)
                {
                    playerControl.inputHor = 0f;
                    playerControl.jumpSpeed = 0f;
                    playerControl.movementSpeed = 0f;
                    animControl.anim.SetBool("isBlocking", blocking);
                    ShieldOBJ.SetActive(true);
                }
                Debug.Log("Is Blocking!");
            }
            else if (Input.GetMouseButtonUp(1) && blocking)
            {
                blocking = false;
                if (!blocking)
                {
                    playerControl.inputHor = Input.GetAxis("Horizontal");
                    playerControl.movementSpeed = 4f;
                    playerControl.jumpSpeed = 7.5f;
                    animControl.anim.SetBool("isBlocking", blocking);
                    ShieldOBJ.SetActive(false);
                }
                Debug.Log("Not Blocking!");
            }
        }
        
    }

    public void CreateObjectInstanstes(GameObject obj)
    {
  
        obj.transform.SetParent(this.transform);
        Instantiate(obj, gameObject.transform.position, Quaternion.identity);
    
    }

    public void DestroyObjectInstances(GameObject obj)
    {
       // DestroyObject(obj);
        Object.Destroy(obj);
    }


    // Update is called once per frame
    void Update()
    {
        mainAttackCombo();
        leftclickChargeAttack();
        BlockingMechanic();

        //if (playerControl.isGrounded)
        //{
        //    airCombo = 0;
        //}

        if (attacking)
        {
            playerControl.canMove = false;
        }
        if (isCharging)
        {
            playerControl.canMove = false;
        }
        if (!releaseCharge)
        {
            playerAttackScript.setAtk(7f);
        }
    }
}
