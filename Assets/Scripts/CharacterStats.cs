using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int initiative;
    public int attack;
    public int defence;
    public int currentTurn;
    bool canMove;

    bool isTargeted;
    public List<string> avaiableSkills = new List<string>();

    public List<BaseSkill> availableSkills = new List<BaseSkill>();

    public bool player;
    public BaseSkill selectedSkill;
    public GameObject hpBarObject;
    public Image hpBarImage;
    bool showBar = false;
    float barTimer = 2;
    float counter = 0;
    // Update is called once per frame
    void Start()
    {

        hpBarImage.fillAmount = (float)curHp / (float)maxHp;
    }
    void Update()
    {
        if (showBar)
        {
            counter += Time.deltaTime;
            if (counter >= barTimer)
            {
                hpBarObject.SetActive(false);
                counter = 0;
                showBar = false;
            }
        }

    }

    public void TakeDamage(int amount)
    {
        int calcDamage = UnityEngine.Random.Range(amount - defence, amount);
        if (calcDamage < 1)
        {
            calcDamage = 1;
        }
        curHp -= calcDamage;
        hpBarImage.fillAmount = (float)curHp / (float)maxHp;
        Debug.Log($"You took {calcDamage}! You got HP: {curHp} left");
        CheckDead();
    }

    public void CheckDead()
    {
        if (curHp <= 0)
        {

        }

    }
    public void ShowHpBar()
    {
        hpBarObject.SetActive(true);
    }
}
