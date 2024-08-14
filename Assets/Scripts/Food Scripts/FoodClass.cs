using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodClass
{
    //for player
    public string name;
    public string description;
    public Sprite pfp;

    public float orderCost;
    public float unlockCost;
    public float reputationLoss;

    //for waiter
    public bool isComplete;
    public bool isBeingPickedUp;

    //for customers
    public float eatTime;

    //for chefs
    public bool needsKneading;
    public float kneadWorkTime;
    public bool kneadedWorkComplete;
    
    public bool needsCutting;
    public float cutWorkTime;
    public bool cutWorkComplete;

    public bool needsCooking;
    public float cookWorkTime;
    public bool cookWorkComplete;

    public bool needsMixing;
    public float mixWorkTime;
    public bool mixWorkComplete;

    public FoodClass()
    {

    }
}
