using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureItemHolder : GameBehaviour
{
    public enum Status { Unoccupied, Occupied}
    public Status status;

    [SerializeField] Transform holdObjectSpot;
    Vector3 holdSpotV3;


    private void Awake()
    {
        if (status == Status.Unoccupied) _WSM.ChangeToUnoccupied(gameObject);
        holdObjectSpot = transform.Find("HoldItem").transform;
        holdSpotV3 = holdObjectSpot.position;
    }

    public Status SetStatus { set { status = value; } }
    public Status ReturnStatus { get { return status; } }

    public Vector3 ReturnHoldSpotV3 { get { return holdSpotV3; } }
    public Transform ReturnHoldSpotTransform { get { return holdObjectSpot; } }
}
