using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CustomerData : GameBehaviour
{
    public GameObject order;
    public bool hasBeenAttened;
    Image orderDisplay;

    GameObject seat;
    public GameObject plateSpot;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindSeat();

        orderDisplay = GetComponentInChildren<Image>();

        _GM.event_startOfDay.AddListener(OrderFood);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ServedFood()
    {
        print("YUM!");
    }

    void OrderFood()
    {
        order = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        _FM.orderedFood.Add(order);
        orderDisplay.sprite = order.GetComponent<FoodData>().foodData.pfp;

    }

    void FindSeat()
    {
        seat = _CustM.emptyChairQueue[0];
        agent.SetDestination(seat.transform.position);
        _CustM.emptyChairQueue.Remove(_CustM.emptyChairQueue[0]);

        plateSpot = seat.transform.GetChild(0).gameObject;
    }

    void LeaveResturant()
    {
        _CustM.customersList.Remove(gameObject);
        _CustM.emptyChairQueue.Add(seat);
        agent.SetDestination(_CustM.resturantDoor.transform.position);
    }
}
