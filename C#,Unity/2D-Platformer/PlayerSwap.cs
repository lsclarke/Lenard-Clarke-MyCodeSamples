using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSwap : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Playable Character Variables")]
    [Space(5)]

    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] private PlayerController playerController;


    [Space(5)]
    [Header("List Containing Players")]
    [Space(5)]
    private GameObject[] playerArray;

    [Space(5)]
    [Header("Swap Variables")]
    [Space(5)] 

    private bool canSwap;
    private float swapTime;

    void Start()
    {
        canSwap = true;
        swapTime = 3f;
        playerArray = new GameObject[2];

        playerArray[0] = player1;
        playerArray[1] = player2;

        playerController.trailRender.startColor = new Color(0, 0, 255, 1);
        playerController.trailRender.endColor = new Color(0, 0, 255, 0);





    }

    public void SwapCharacters()
    {
        for (int i = -1; i < 0; i++)
        {
            if (canSwap && playerArray[0].active == true)
            {
                canSwap = false;
                Debug.Log("True");
                playerArray[0].gameObject.SetActive(false);
                playerArray[1].SetActive(true);
                playerArray[1].transform.position = playerArray[0].transform.position;
                //Red
                StartCoroutine(SwapCoolDown(swapTime));
                break;
            }

            if (canSwap && playerArray[1].active == true)
            {
                canSwap = false;
                Debug.Log("True");
                playerArray[1].gameObject.SetActive(false);
                playerArray[0].SetActive(true);
                playerArray[0].transform.position = playerArray[1].transform.position;
                //Blue

                StartCoroutine(SwapCoolDown(swapTime));
                break;
            }
        }



    }

    private IEnumerator SwapCoolDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSwap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Debug.Log("Swap: True");
            SwapCharacters();
        }

        if (canSwap/*player1.active == true && player2.active != true*/)
        {
            playerController.trailRender.startColor = new Color(0, 0, 255, 1);
            playerController.trailRender.endColor = new Color(0, 0, 255, 0);
        }else if (player2.active == true && player1.active != true)
        {
            playerController.trailRender.startColor = new Color(255, 0, 0, 1);
            playerController.trailRender.endColor = new Color(255, 0, 0, 0);
        }


    }
}