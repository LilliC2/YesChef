using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public enum Status { Unoccupied, Occupied}
    public Status status;

    public Transform holdFoodPos;

    private void Awake()
    {
        holdFoodPos = transform.Find("HoldFoodSpot").transform;
    }
}
