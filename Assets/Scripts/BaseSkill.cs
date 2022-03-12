using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseSkill
{
    public BaseSkill(BaseSkill bs){
        skillName = bs.skillName;
        skillRange = bs.skillRange;
        skillIcon = bs.skillIcon;
        power = bs.power;
        cooldown = bs.cooldown;
        turnLastUsed = bs.turnLastUsed;
        targetType = bs.targetType;
    }
    public string skillName;
    public int power;
    public int cooldown;
    public int? turnLastUsed;
    public TargetTypes targetType;
    public float skillRange;
    public Sprite skillIcon;

}

public enum TargetTypes{
    Enemy,
    Ally,
    AOE
}
