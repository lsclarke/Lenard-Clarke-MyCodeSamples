using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [Header("Combat Variables Variables")]
    [Space(5)]

    public float attackForce;
    public float manaForce;
    public float defenseForce;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void setAtk(float atk)
    {
        attackForce = atk;

    }

    public float getAtk()
    {
        return attackForce;
    }

    public void setManaAtk(float mAtk)
    {
        manaForce = mAtk;
    }

    public float getManaAtk()
    {
        return manaForce;
    }

    public void setDefense(float d)
    {
       defenseForce = d;
    }

    public float getDefense()
    {
        return defenseForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
