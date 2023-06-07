using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public float speed;
    public int startPosition;
    public Transform[] locations;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = locations[startPosition].position; 
    }

    public void MoveToPosition()
    {
        if(Vector2.Distance(transform.position, locations[i].position) < 0.02f)
        {
            i++;
            if(i == locations.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, locations[i].position, speed * Time.deltaTime);

        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition();
    }
}
