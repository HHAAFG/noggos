using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int initiative;
    public int attack;
    public int defence;
    
    public float attackRange;
    bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        int calcDamage = Random.Range(amount - defence, amount);
        if(calcDamage < 1){
             calcDamage = 1;
        }
        curHp -= calcDamage;
        Debug.Log($"SIEG HEIL {calcDamage}");
        CheckDead();
    }

    public void CheckDead()
    {
        if(curHp <= 0)
        {
            //deadasShieet
            canMove = false;

        }

    }
}
