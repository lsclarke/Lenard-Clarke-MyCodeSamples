using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public GameObject Player_1;
    [SerializeField] public GameObject Player_2;
    [SerializeField] public GameObject goal_Obj;
    [SerializeField] public GameObject teleporter_Obj; 
    [SerializeField] public GameObject teleporter_Obj2;
    [SerializeField] public TrailRenderer tRenderer;
    [SerializeField] public Transform spawnPoint;

    [SerializeField] public Goal goalScript;

    [SerializeField] public PlayerMovement playerMovement;
    public Color playerOneColor;
    public Color playerTwoColor;

    public bool canSpin;
    public bool canTeleport;

    public bool isPlayerOne;
    public bool isPlayerTwo;

    public float rotationSpeed;

    private void Awake()
    {
        tRenderer = GetComponent<TrailRenderer>();
        canSpin = false;
        canTeleport = false;
    }

    private void FixedUpdate()
    {
        if(PlayerOneEnable() && !PlayerTwoEnable())
        {

            tRenderer.startColor = playerOneColor;
            tRenderer.endColor = playerOneColor;
        }else if(PlayerTwoEnable() && !PlayerOneEnable()) 
        {
            tRenderer.startColor = playerTwoColor;
            tRenderer.endColor = playerTwoColor;
        }
        else
        {
            tRenderer.startColor = new Color(0, 0, 0);
        }
    }
    private bool PlayerOneEnable()
    {
        return Player_1.active;
    }

    private bool PlayerTwoEnable()
    {
        return Player_2.active;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "movingPlatform")
        {
            transform.position += collision.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            Debug.Log("Next Level");
            canSpin = true;
        }

        if (collision.tag == "trap")
        {
            transform.position = spawnPoint.position;
        }

    }

    public void EndSpin()
    {
        if (canSpin)
        {
            transform.Rotate(0, 0, rotationSpeed);
            transform.position = new Vector2(goal_Obj.transform.position.x, goal_Obj.transform.position.y + 1);
            playerMovement.enabled = false;
            StartCoroutine(NextLevelTimer());
        }
    }

    private IEnumerator NextLevelTimer()
    {
        yield return new WaitForSeconds(2f);
        goalScript.NextLevel();
    }



    private void Update()
    {
        EndSpin();
        //TeleportSpin();
    }
}
