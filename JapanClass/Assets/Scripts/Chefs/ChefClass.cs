using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChefClass 
{
    public string name;
    public string description;
    public float hireCost;

    public Sprite pfp;

    public float range;

    public bool kneedSkill;
    public float kneedEffectivness;
    
    public bool cutSkill;
    public float cutEffectivness;
    
    public bool mixSkill;
    public float mixEffectivness;

    public bool cookSkill;
    public float cookEffectivness;

    public ChefClass()
    {

    }
}
