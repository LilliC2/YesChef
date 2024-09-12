using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChefClass 
{
    public string name;
    public float movSpeed;

    public Sprite pfp;

    public bool kneadSkill;
    public float kneadEffectivness;
    public float kneadUpgradeCost;
    
    public bool cutSkill;
    public float cutEffectivness;
    public float cutUpgradeCost;
    
    public bool mixSkill;
    public float mixEffectivness;
    public float mixUpgradeCost;

    public bool cookSkill;
    public float cookEffectivness;
    public float cookUpgradeCost;

    public ChefClass()
    {

    }
}
