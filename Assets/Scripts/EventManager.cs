using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{

    [Header("Waiters")]
    public UnityEvent event_orderTaken;

    [Header("Customers")]
    public UnityEvent event_customerReadyToBeSeated;


}
