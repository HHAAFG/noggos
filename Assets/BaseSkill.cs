using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseSkill
{
    public string skillName;
    public int power;
    public int cooldown;

    public float skillRange;
    public Sprite skillIcon;

   public virtual void UseSkill(){
       //Gör en spell
       Debug.Log("Used: " + skillName);

   }
}
