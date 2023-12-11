using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodClass
{
    public string name;
    public string description;
    public Sprite pfp;

    public bool isCooked;
    public bool isBeingPickedUp;

    public float orderCost;
    public float unlockCost;
    public float reputationLoss;

    public bool needsKneading;
    public float maxKneedPrepPoints;
    public float kneedPrepPoints;
    
    public bool needsCutting;
    public float maxCutPrepPoints;
    public float cutPrepPoints;
    
    public bool needsCooking;
    public float maxCookPrepPoints;
    public float cookPrepPoints;
    
    public bool needsMixing;
    public float maxMixPrepPoints;
    public float mixPrepPoints;

    public FoodClass()
    {

    }
}
