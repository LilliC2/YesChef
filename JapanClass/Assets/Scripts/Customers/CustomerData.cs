using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerData : GameBehaviour
{
    GameObject order;
    bool served;

    GameObject seat;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        order = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        FindSeat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindSeat()
    {
        seat = _CustM.emptyChairQueue[0];
        agent.SetDestination(seat.transform.position);
        _CustM.emptyChairQueue.Remove(_CustM.emptyChairQueue[0]);
    }

    void LeaveResturant()
    {
        _CustM.emptyChairQueue.Add(seat);
        agent.SetDestination(_CustM.resturantDoor.transform.position);
    }
}
