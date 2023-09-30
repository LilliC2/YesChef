using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodClass
{
    public string name;
    public string description;
    public bool isCooked;
    public float orderCost;
    public float unlockCost;
    public float reputationLoss;

    public GameObject rawFood;
    public GameObject cookedFood;

    public bool needsKneeding;

    public float maxKneedPrepPoints;
    public float kneedPrepPoints;

    public FoodClass()
    {

    }
}
