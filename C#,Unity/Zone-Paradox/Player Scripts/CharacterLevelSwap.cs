using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLevelSwap : MonoBehaviour
{
    private bool canSwap;
    private bool swap;
    public float swapCoolDownTime = 5f;

    public GameObject Player_1;
    public GameObject Player_2;

    public GameObject Layout_1;
    public GameObject Layout_2;

    public bool isLvlLayoutOne; 
    public bool isLvlLayoutTwo;

    public bool isPlayerOne;
    public bool isPlayerTwo;


    private void Awake()
    {
        canSwap = true;

    }

    private void FixedUpdate()
    {
        isLvlLayoutOne = LayOutOneEnable();
        isLvlLayoutTwo = LayOutTwoEnable();

        isPlayerOne = PlayerOneEnable();
        isPlayerTwo = PlayerTwoEnable();
    }

    public void Swap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Swap();
        }

    }

    public void Swap()
    {
        if (isPlayerOne && isLvlLayoutOne && canSwap)
        {
            /*Set Player 1 to inactive
             Set Player 2 to active*/
            Debug.Log("Switch to Player 2");
            canSwap = false;
            Player_1.SetActive(false);
            Layout_1.SetActive(false);

            Player_2.SetActive(true);
            Layout_2.SetActive(true);

            Player_2.transform.position = Player_1.transform.position;
            StartCoroutine(SwapCoolDown());

        }
        if (isPlayerTwo && isLvlLayoutTwo && canSwap)
        {
            /*Set Player 1 to inactive
             Set Player 2 to active*/
            canSwap = false;
            Debug.Log("Switch to Player 1");
            Player_2.SetActive(false);
            Layout_2.SetActive(false);

            Player_1.SetActive(true);
            Layout_1.SetActive(true);

            Player_1.transform.position = Player_2.transform.position;
            StartCoroutine(SwapCoolDown());

        }
        else
        {
            Debug.Log("No one to switch to :(");
        }
    }

    private IEnumerator SwapCoolDown()
    {
        yield return new WaitForSeconds(swapCoolDownTime);
        canSwap = true;
    }


    private bool PlayerOneEnable()
    {
        return Player_1.active;
    }

    private bool PlayerTwoEnable()
    {
        return Player_2.active;
    }

    private bool LayOutOneEnable() 
    {
        return Layout_1.active;
    }

    private bool LayOutTwoEnable()
    {
        return Layout_2.active;
    }

}
