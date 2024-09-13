using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CustomerData : GameBehaviour
{
    public enum Task { Queue, WaitToBeSeated,FollowWaiter ,SelectFromMenu, ReadyToOrder, WaitForFood, EatFood, PayAndLeave}
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
    GameObject table;

    [Header("Select From Menu")]
    public GameObject order;
    public OrderClass orderClass;
    bool hasSelectedOrder;

    [Header("Eat Food")]
    bool isEating;

    [Header("Leave")]
    Vector3 leavePos;

    [Header("Stopwatch")]
    double queueWaitTime, takeOrderWaitTime, orderArrivalWaitTime;
    [SerializeField]
    float currentTime = 0;
    bool isTimerActive = true;

    [SerializeField] TMP_Text currentState;

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

        if(_GM.playState == GameManager.PlayState.Open)
        {
            currentState.text = task.ToString();

            switch (task)
            {
                //In any position other than 0
                case Task.Queue:

                    //start timer
                    if (!isTimerActive) StartTimer();


                    //if not at correct queue position
                    agent.SetDestination(_CustM.customerOutsideQueueSpots[_CustM.customersInQueue.IndexOf(gameObject)].position);

                    //check if at front of queue
                    if (_CustM.customersInQueue.IndexOf(gameObject) == 0 && Vector3.Distance(transform.position,_CustM.customerOutsideQueueSpots[0].position)<=1.5f)
                    {
                        _CustM.customerIsWaiting = true;
                        _EM.event_customerReadyToBeSeated.Invoke();
                        queueWaitTime = StopTimer();
                        task = Task.WaitToBeSeated;
                    }


                    break;
                case Task.WaitToBeSeated:


                    break;
                case Task.FollowWaiter:

                    //check if resturant is open


                    //remove from outside queue
                    if (_CustM.customersInQueue.Contains(gameObject))
                    {
                        _CustM.customerIsWaiting = false;
                        _CustM.customersInQueue.Remove(gameObject);
                    }

                    if (waiterFollow != null)
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

                    if (!hasSelectedOrder)
                    {
                        //start timer
                        if (!isTimerActive) StartTimer();

                        hasSelectedOrder = true;
                        if (_FM.avalibleMenu.Count != 0)
                        {
                            order = _FM.avalibleMenu[UnityEngine.Random.Range(0, _FM.avalibleMenu.Count - 1)];
                            orderClass = order.GetComponent<FoodData>().order;
                                                         //print("Want to order " +  order.foodPrefab.name);
                            _CustM.customersReadyToOrder.Add(gameObject);
                            task = Task.ReadyToOrder;
                        }
                        else
                        {
                            //if no avalible items, leave
                            LeaveResturant();
                        }
                    }


                    break;
                case Task.ReadyToOrder:
                    break;
                case Task.WaitForFood:
                    if (!isTimerActive) StartTimer();

                    break;
                case Task.EatFood:

                    if (!isEating)
                    {
                        orderArrivalWaitTime = StopTimer();



                        isEating = true;
                        ExecuteAfterSeconds(orderClass.eatTime, () => FinishedEating());
                    }


                    break;
                case Task.PayAndLeave:

                    agent.SetDestination(leavePos);
                    if (Vector3.Distance(transform.position,leavePos)<1f)
                    {
                        print("Should be removed");
                        _CustM.RemoveCustomer(gameObject);
                    }


                    break;
            }
        }
        else
        {
            if (agent.destination == null) LeaveResturant();
            print(Vector3.Distance(transform.position, agent.destination));
            if (Vector3.Distance(transform.position, agent.destination) <=2)
            {
                _CustM.RemoveCustomer(gameObject);
            }
        }
       
    }

    public void OrderHasBeenTaken()
    {
        _CustM.customersReadyToOrder.Remove(gameObject);

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
        //print(time.Minutes.ToString() + ":" + time.Seconds.ToString());
        return time.TotalSeconds;
    }

    void FinishedEating()
    {
        //orderGO.GetComponent<FoodData>().foodState = FoodData.FoodState.Dirty;
        _FM.RemoveFood(order);
        
        PayForOrder();
    }

    void PayForOrder()
    {
        if (_CustM.customersInQueue.Contains(gameObject)) _CustM.customersInQueue.Remove(gameObject);
        if (_CustM.customersReadyToOrder.Contains(gameObject)) _CustM.customersReadyToOrder.Remove(gameObject);

        _GM.PlayerMoneyIncrease(orderClass.orderCost);

        
        //calculate rating
        _CustM.CalculateCustomersResturantRating(queueWaitTime, takeOrderWaitTime, orderArrivalWaitTime);

        LeaveResturant();
    }

    void LeaveResturant()
    {
        agent.isStopped = false;
        //set table as unoccupied
        _FOHM.ChangeToUnoccupied(table);

        leavePos = _CustM.leavePoints[UnityEngine.Random.Range(0, _CustM.leavePoints.Length)].position;
        //set leave destination
        
        task = Task.PayAndLeave;

    }

    public void StartFollowWaiter(GameObject _waiter)
    {
        //print("Follow waiter");
        waiterFollow = _waiter;
        task = Task.FollowWaiter;
    }

    /// <summary>
    /// Find chair avalible chair at given table
    /// </summary>
    public void BeSeated(GameObject _table)
    {
        //print("seated");
        var tableData = _table.GetComponent<Table>();
        var targetChair = tableData.unoccupiedSeats.FirstOrDefault();

        //plate spot first child of chair
        plateSpot = targetChair.GetChild(0);

        table = _table;
        tableData.ChangeToOccupied(targetChair);

        agent.SetDestination(targetChair.position);
        ExecuteAfterSeconds(1, () => task = Task.SelectFromMenu);



    }

}
