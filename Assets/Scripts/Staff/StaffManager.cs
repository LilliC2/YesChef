using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : Singleton<StaffManager>
{
    [Header("Work Zones")]
    public Vector3 staffRoomZone, kitchenZone, FOHZone;

    public float returnToWorkSpeed;
    public float casualSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
