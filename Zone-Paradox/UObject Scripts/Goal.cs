using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [SerializeField] public GameObject teleporterB;
    public bool canTeleport;

    private void Awake()
    {
        canTeleport = false;
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NextLocation()
    {
        player.transform.position = new Vector2(teleporterB.transform.position.x, teleporterB.transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Next Location");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Next Location");
            canTeleport = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Next Location");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Next Location");
            canTeleport = true;
        }
    }



    public void TeleportSpin()
    {
        if (canTeleport)
        {
            player.transform.position = new Vector3(transform.position.x, transform.position.y, -3);
            StartCoroutine(NextLocationTimer());
        }
    }
    private IEnumerator NextLocationTimer()
    {
        yield return new WaitForSeconds(1f);
        canTeleport = false;
        player.transform.position = new Vector3(teleporterB.transform.position.x, teleporterB.transform.position.y, -3);
        //goalScript.NextLocation();
    }

    private void Update()
    {
        TeleportSpin();
    }
}
