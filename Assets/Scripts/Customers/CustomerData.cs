using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CustomerData : GameBehaviour
{
    public enum Task { Queue, WaitToBeSeated,FollowWaiter ,SelectFromMenu, WaitForFood, EatFood, PayAndLeave}
    public Task task;

    //when interacting with waiters, this will let the other waiters know if the customer is already undergoing a task
    public bool beingAttened = false;

    [Header("Movement")]
    NavMeshAgent agent;

    [Header("Queue")]
    [SerializeField]
    int queueIndex;

    [Header("Follow Waiter")]
    [SerializeField] float waiterFollowDistance; //follow x distance behind waiter
    GameObject waiterFollow;
    public Transform plateSpot;

    [Header("Select From Menu")]
    public OrderClass order;
    public GameObject orderGO;
    bool hasSelectedOrder;

    [Header("Eat Food")]
    bool isEating;

    [Header("Stopwatch")]
    double queueWaitTime, takeOrderWaitTime, orderArrivalWaitTime;
    [SerializeField]
    float currentTime = 0;
    bool isTimerActive = true;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //track index when customer is spawned
        queueIndex = _CustM.customerOutsideQueueSpots.Count - 1;
    }

    private void Update()
    {
        if(isTimerActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }

        switch(task)
        {
            //In any position other than 0
            case Task.Queue:

                //start timer
                if(!isTimerActive) StartTimer();


                //if not at correct queue position
                if (queueIndex != _CustM.customersInQueue.IndexOf(gameObject))
                {
                    agent.SetDestination(_CustM.customerOutsideQueueSpots[queueIndex].position);

                    //check if at position
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {

                        //move down queue
                        queueIndex--;
                        agent.SetDestination(_CustM.customerOutsideQueueSpots[queueIndex].position);

                        //check if at front of queue
                        if(queueIndex == 0)
                        {
                            _EM.event_customerReadyToBeSeated.Invoke();
                            queueWaitTime = StopTimer();
                            task = Task.WaitToBeSeated;
                        }
                    }
                }


                break;
            case Task.WaitToBeSeated:
            
                
                break;
            case Task.FollowWaiter:

                //remove from outside queue
                if(_CustM.customersInQueue.Contains(gameObject)) _CustM.customersInQueue.Remove(gameObject);

                if(waiterFollow != null)
                {
                    if (Vector3.Distance(transform.position, waiterFollow.transform.position) > waiterFollowDistance)
                    {
                        agent.SetDestination(waiterFollow.transform.position);
                    }
                }
                

                break;
            case Task.SelectFromMenu:

                //check they have sat down
                if (agent.remainingDistance <= agent.stoppingDistance) agent.isStopped = true;

                if(!hasSelectedOrder)
                {
                    //start timer
                    if (!isTimerActive) StartTimer();

                    hasSelectedOrder = true;
                    order = _FM.menu[UnityEngine.Random.Range(0, _FM.menu.Count-1)];
                    order.customer = gameObject; //set customer as itself
                    print("Want to order " +  order.foodPrefab.name);
                    _CustM.customersReadyToOrder.Add(gameObject);
                }


                break;
            case Task.WaitForFood:
                if (!isTimerActive) StartTimer();

                break;
            case Task.EatFood:

                if(!isEating)
                {
                    orderArrivalWaitTime = StopTimer();

                    isEating = true;
                    ExecuteAfterSeconds(order.eatTime, () => FinishedEating());
                }


                break;
            case Task.PayAndLeave:

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    _CustM.RemoveCustomer(gameObject);
                }


                break;
        }
    }

    public void OrderHasBeenTaken()
    {
        takeOrderWaitTime = StopTimer();
        task = Task.WaitForFood;


    }

    void StartTimer()
    {
        currentTime = 0;
        isTimerActive = true;

    }

    double StopTimer()
    {
        isTimerActive = false;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        print(time.Minutes.ToString() + ":" + time.Seconds.ToString());
        return time.TotalSeconds;
    }

    void FinishedEating()
    {
        orderGO.GetComponent<FoodData>().foodState = FoodData.FoodState.Dirty;
        PayForOrder();
    }

    void PayForOrder()
    {
        _GM.PlayerMoneyIncrease(order.orderCost);

        //set leave destination
        agent.SetDestination(_CustM.leavePoints[UnityEngine.Random.Range(0, _CustM.leavePoints.Length)].position);

        //calculate rating
        _CustM.CalculateResturantRating(queueWaitTime, takeOrderWaitTime, orderArrivalWaitTime);

        task = Task.PayAndLeave;

    }

    public void StartFollowWaiter(GameObject _waiter)
    {
        print("Follow waiter");
        waiterFollow = _waiter;
        task = Task.FollowWaiter;
    }

    /// <summary>
    /// Find chair avalible chair at given table
    /// </summary>
    public void BeSeated(GameObject _table)
    {
        print("seated");
        var tableData = _table.GetComponent<Table>();
        var targetChair = tableData.unoccupiedSeats.FirstOrDefault();

        //plate spot first child of chair
        plateSpot = targetChair.GetChild(0);

        tableData.ChangeToOccupied(targetChair);

        agent.SetDestination(targetChair.position);
        ExecuteAfterSeconds(1, () => task = Task.SelectFromMenu);



    }

}
