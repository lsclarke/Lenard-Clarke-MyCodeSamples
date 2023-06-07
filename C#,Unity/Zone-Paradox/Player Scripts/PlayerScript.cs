using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [Header("General Player Variables")]
    [SerializeField] private static float hpCount;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    public void setHP(float hp)
    {
        hpCount = hp;
    }

    public float getHP()
    {
        return hpCount;
    }

    public void setSPD(float spd)
    {
        speed = spd;
    }

    public float getSPD()
    {
        return speed;
    }

    public void setJMP(float jmp)
    {
        jumpForce = jmp;
    }

    public float getJMP()
    {
        return jumpForce;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}