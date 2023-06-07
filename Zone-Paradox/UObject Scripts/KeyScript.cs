using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject[] destroyObjects;
    public GameObject[] activateObjects;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Destroy objects in list");
            for(int i = 0; i < destroyObjects.Length; i++)
            {
                Destroy(destroyObjects[i]);
                
            }
            for (int i = 0; i < activateObjects.Length; i++)
            {
                activateObjects[i].SetActive(true);

            }
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

