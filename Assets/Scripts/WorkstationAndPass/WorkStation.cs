using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : GameBehaviour
{
    public enum Status { Unoccupied, Occupied}
    public Status status;

    public Transform holdFoodPos;

    private void Awake()
    {
        if (status == Status.Unoccupied) _WSM.AddToUnoccupiedList(gameObject);
        holdFoodPos = transform.Find("HoldFoodSpot").transform;
    }
}
